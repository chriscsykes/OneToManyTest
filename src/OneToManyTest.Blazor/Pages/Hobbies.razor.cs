using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blazorise;
using Blazorise.DataGrid;
using Volo.Abp.BlazoriseUI.Components;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Components.Web.Theming.PageToolbars;
using OneToManyTest.Hobbies;
using OneToManyTest.Permissions;
using OneToManyTest.Shared;

namespace OneToManyTest.Blazor.Pages
{
    public partial class Hobbies
    {
        protected List<Volo.Abp.BlazoriseUI.BreadcrumbItem> BreadcrumbItems = new List<Volo.Abp.BlazoriseUI.BreadcrumbItem>();
        protected PageToolbar Toolbar {get;} = new PageToolbar();
        private IReadOnlyList<HobbyWithNavigationPropertiesDto> HobbyList { get; set; }
        private int PageSize { get; } = LimitedResultRequestDto.DefaultMaxResultCount;
        private int CurrentPage { get; set; } = 1;
        private string CurrentSorting { get; set; }
        private int TotalCount { get; set; }
        private bool CanCreateHobby { get; set; }
        private bool CanEditHobby { get; set; }
        private bool CanDeleteHobby { get; set; }
        private HobbyCreateDto NewHobby { get; set; }
        private Validations NewHobbyValidations { get; set; }
        private HobbyUpdateDto EditingHobby { get; set; }
        private Validations EditingHobbyValidations { get; set; }
        private Guid EditingHobbyId { get; set; }
        private Modal CreateHobbyModal { get; set; }
        private Modal EditHobbyModal { get; set; }
        private GetHobbiesInput Filter { get; set; }
        private DataGridEntityActionsColumn<HobbyWithNavigationPropertiesDto> EntityActionsColumn { get; set; }
        protected string SelectedCreateTab = "hobby-create-tab";
        protected string SelectedEditTab = "hobby-edit-tab";
        private IReadOnlyList<LookupDto<Guid>> Customers { get; set; } = new List<LookupDto<Guid>>();
        
        private string SelectedCustomerId { get; set; }
        
        private string SelectedCustomerText { get; set; }

        private List<LookupDto<Guid>> SelectedCustomers { get; set; } = new List<LookupDto<Guid>>();
        public Hobbies()
        {
            NewHobby = new HobbyCreateDto();
            EditingHobby = new HobbyUpdateDto();
            Filter = new GetHobbiesInput
            {
                MaxResultCount = PageSize,
                SkipCount = (CurrentPage - 1) * PageSize,
                Sorting = CurrentSorting
            };
        }

        protected override async Task OnInitializedAsync()
        {
            await SetToolbarItemsAsync();
            await SetBreadcrumbItemsAsync();
            await SetPermissionsAsync();
        }

        protected virtual ValueTask SetBreadcrumbItemsAsync()
        {
            BreadcrumbItems.Add(new Volo.Abp.BlazoriseUI.BreadcrumbItem(L["Menu:Hobbies"]));
            return ValueTask.CompletedTask;
        }

        protected virtual ValueTask SetToolbarItemsAsync()
        {
            Toolbar.AddButton(L["ExportToExcel"], async () =>{ await DownloadAsExcelAsync(); }, IconName.Download);
            
            Toolbar.AddButton(L["NewHobby"], async () =>
            {
                await OpenCreateHobbyModalAsync();
            }, IconName.Add, requiredPolicyName: OneToManyTestPermissions.Hobbies.Create);

            return ValueTask.CompletedTask;
        }

        private async Task SetPermissionsAsync()
        {
            CanCreateHobby = await AuthorizationService
                .IsGrantedAsync(OneToManyTestPermissions.Hobbies.Create);
            CanEditHobby = await AuthorizationService
                            .IsGrantedAsync(OneToManyTestPermissions.Hobbies.Edit);
            CanDeleteHobby = await AuthorizationService
                            .IsGrantedAsync(OneToManyTestPermissions.Hobbies.Delete);
        }

        private async Task GetHobbiesAsync()
        {
            Filter.MaxResultCount = PageSize;
            Filter.SkipCount = (CurrentPage - 1) * PageSize;
            Filter.Sorting = CurrentSorting;

            var result = await HobbiesAppService.GetListAsync(Filter);
            HobbyList = result.Items;
            TotalCount = (int)result.TotalCount;
        }

        protected virtual async Task SearchAsync()
        {
            CurrentPage = 1;
            await GetHobbiesAsync();
            await InvokeAsync(StateHasChanged);
        }

        private  async Task DownloadAsExcelAsync()
        {
            var token = (await HobbiesAppService.GetDownloadTokenAsync()).Token;
            NavigationManager.NavigateTo($"/api/app/hobbies/as-excel-file?DownloadToken={token}", forceLoad: true);
        }

        private async Task OnDataGridReadAsync(DataGridReadDataEventArgs<HobbyWithNavigationPropertiesDto> e)
        {
            CurrentSorting = e.Columns
                .Where(c => c.SortDirection != SortDirection.Default)
                .Select(c => c.Field + (c.SortDirection == SortDirection.Descending ? " DESC" : ""))
                .JoinAsString(",");
            CurrentPage = e.Page;
            await GetHobbiesAsync();
            await InvokeAsync(StateHasChanged);
        }

        private async Task OpenCreateHobbyModalAsync()
        {
            SelectedCustomers = new List<LookupDto<Guid>>();
            

            NewHobby = new HobbyCreateDto{
                
                
            };
            await NewHobbyValidations.ClearAll();
            await CreateHobbyModal.Show();
        }

        private async Task CloseCreateHobbyModalAsync()
        {
            NewHobby = new HobbyCreateDto{
                
                
            };
            await CreateHobbyModal.Hide();
        }

        private async Task OpenEditHobbyModalAsync(HobbyWithNavigationPropertiesDto input)
        {
            var hobby = await HobbiesAppService.GetWithNavigationPropertiesAsync(input.Hobby.Id);
            
            EditingHobbyId = hobby.Hobby.Id;
            EditingHobby = ObjectMapper.Map<HobbyDto, HobbyUpdateDto>(hobby.Hobby);
            SelectedCustomers = hobby.Customers.Select(a => new LookupDto<Guid>{ Id = a.Id, DisplayName = a.Email}).ToList();

            await EditingHobbyValidations.ClearAll();
            await EditHobbyModal.Show();
        }

        private async Task DeleteHobbyAsync(HobbyWithNavigationPropertiesDto input)
        {
            await HobbiesAppService.DeleteAsync(input.Hobby.Id);
            await GetHobbiesAsync();
        }

        private async Task CreateHobbyAsync()
        {
            try
            {
                if (await NewHobbyValidations.ValidateAll() == false)
                {
                    return;
                }
                NewHobby.CustomerIds = SelectedCustomers.Select(x => x.Id).ToList();


                await HobbiesAppService.CreateAsync(NewHobby);
                await GetHobbiesAsync();
                await CloseCreateHobbyModalAsync();
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
            }
        }

        private async Task CloseEditHobbyModalAsync()
        {
            await EditHobbyModal.Hide();
        }

        private async Task UpdateHobbyAsync()
        {
            try
            {
                if (await EditingHobbyValidations.ValidateAll() == false)
                {
                    return;
                }
                EditingHobby.CustomerIds = SelectedCustomers.Select(x => x.Id).ToList();


                await HobbiesAppService.UpdateAsync(EditingHobbyId, EditingHobby);
                await GetHobbiesAsync();
                await EditHobbyModal.Hide();                
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
            }
        }

        private void OnSelectedCreateTabChanged(string name)
        {
            SelectedCreateTab = name;
        }

        private void OnSelectedEditTabChanged(string name)
        {
            SelectedEditTab = name;
        }
        

        private async Task GetCustomerLookupAsync(string newValue = null)
        {
            Customers = (await HobbiesAppService.GetCustomerLookupAsync(new LookupRequestDto { Filter = newValue })).Items;
        }

        private void AddCustomer()
        {
            if (SelectedCustomerId.IsNullOrEmpty())
            {
                return;
            }
            
            if (SelectedCustomers.Any(p => p.Id.ToString() == SelectedCustomerId))
            {
                UiMessageService.Warn(L["ItemAlreadyAdded"]);
                return;
            }

            SelectedCustomers.Add(new LookupDto<Guid>
            {
                Id = Guid.Parse(SelectedCustomerId),
                DisplayName = SelectedCustomerText
            });
        }

    }
}
