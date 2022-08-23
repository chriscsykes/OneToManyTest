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
using OneToManyTest.Customers;
using OneToManyTest.Permissions;
using OneToManyTest.Shared;

namespace OneToManyTest.Blazor.Pages
{
    public partial class Customers
    {
        protected List<Volo.Abp.BlazoriseUI.BreadcrumbItem> BreadcrumbItems = new List<Volo.Abp.BlazoriseUI.BreadcrumbItem>();
        protected PageToolbar Toolbar {get;} = new PageToolbar();
        private IReadOnlyList<CustomerWithNavigationPropertiesDto> CustomerList { get; set; }
        private int PageSize { get; } = LimitedResultRequestDto.DefaultMaxResultCount;
        private int CurrentPage { get; set; } = 1;
        private string CurrentSorting { get; set; }
        private int TotalCount { get; set; }
        private bool CanCreateCustomer { get; set; }
        private bool CanEditCustomer { get; set; }
        private bool CanDeleteCustomer { get; set; }
        private CustomerCreateDto NewCustomer { get; set; }
        private Validations NewCustomerValidations { get; set; }
        private CustomerUpdateDto EditingCustomer { get; set; }
        private Validations EditingCustomerValidations { get; set; }
        private Guid EditingCustomerId { get; set; }
        private Modal CreateCustomerModal { get; set; }
        private Modal EditCustomerModal { get; set; }
        private GetCustomersInput Filter { get; set; }
        private DataGridEntityActionsColumn<CustomerWithNavigationPropertiesDto> EntityActionsColumn { get; set; }
        protected string SelectedCreateTab = "customer-create-tab";
        protected string SelectedEditTab = "customer-edit-tab";
        private IReadOnlyList<LookupDto<Guid?>> OrdersNullable { get; set; } = new List<LookupDto<Guid?>>();

        public Customers()
        {
            NewCustomer = new CustomerCreateDto();
            EditingCustomer = new CustomerUpdateDto();
            Filter = new GetCustomersInput
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
            await GetNullableOrderLookupAsync();


        }

        protected virtual ValueTask SetBreadcrumbItemsAsync()
        {
            BreadcrumbItems.Add(new Volo.Abp.BlazoriseUI.BreadcrumbItem(L["Menu:Customers"]));
            return ValueTask.CompletedTask;
        }

        protected virtual ValueTask SetToolbarItemsAsync()
        {
            Toolbar.AddButton(L["ExportToExcel"], async () =>{ await DownloadAsExcelAsync(); }, IconName.Download);
            
            Toolbar.AddButton(L["NewCustomer"], async () =>
            {
                await OpenCreateCustomerModalAsync();
            }, IconName.Add, requiredPolicyName: OneToManyTestPermissions.Customers.Create);

            return ValueTask.CompletedTask;
        }

        private async Task SetPermissionsAsync()
        {
            CanCreateCustomer = await AuthorizationService
                .IsGrantedAsync(OneToManyTestPermissions.Customers.Create);
            CanEditCustomer = await AuthorizationService
                            .IsGrantedAsync(OneToManyTestPermissions.Customers.Edit);
            CanDeleteCustomer = await AuthorizationService
                            .IsGrantedAsync(OneToManyTestPermissions.Customers.Delete);
        }

        private async Task GetCustomersAsync()
        {
            Filter.MaxResultCount = PageSize;
            Filter.SkipCount = (CurrentPage - 1) * PageSize;
            Filter.Sorting = CurrentSorting;

            var result = await CustomersAppService.GetListAsync(Filter);
            CustomerList = result.Items;
            TotalCount = (int)result.TotalCount;
        }

        protected virtual async Task SearchAsync()
        {
            CurrentPage = 1;
            await GetCustomersAsync();
            await InvokeAsync(StateHasChanged);
        }

        private  async Task DownloadAsExcelAsync()
        {
            var token = (await CustomersAppService.GetDownloadTokenAsync()).Token;
            NavigationManager.NavigateTo($"/api/app/customers/as-excel-file?DownloadToken={token}", forceLoad: true);
        }

        private async Task OnDataGridReadAsync(DataGridReadDataEventArgs<CustomerWithNavigationPropertiesDto> e)
        {
            CurrentSorting = e.Columns
                .Where(c => c.SortDirection != SortDirection.Default)
                .Select(c => c.Field + (c.SortDirection == SortDirection.Descending ? " DESC" : ""))
                .JoinAsString(",");
            CurrentPage = e.Page;
            await GetCustomersAsync();
            await InvokeAsync(StateHasChanged);
        }

        private async Task OpenCreateCustomerModalAsync()
        {
            NewCustomer = new CustomerCreateDto{
                
                
            };
            await NewCustomerValidations.ClearAll();
            await CreateCustomerModal.Show();
        }

        private async Task CloseCreateCustomerModalAsync()
        {
            NewCustomer = new CustomerCreateDto{
                
                
            };
            await CreateCustomerModal.Hide();
        }

        private async Task OpenEditCustomerModalAsync(CustomerWithNavigationPropertiesDto input)
        {
            var customer = await CustomersAppService.GetWithNavigationPropertiesAsync(input.Customer.Id);
            
            EditingCustomerId = customer.Customer.Id;
            EditingCustomer = ObjectMapper.Map<CustomerDto, CustomerUpdateDto>(customer.Customer);
            await EditingCustomerValidations.ClearAll();
            await EditCustomerModal.Show();
        }

        private async Task DeleteCustomerAsync(CustomerWithNavigationPropertiesDto input)
        {
            await CustomersAppService.DeleteAsync(input.Customer.Id);
            await GetCustomersAsync();
        }

        private async Task CreateCustomerAsync()
        {
            try
            {
                if (await NewCustomerValidations.ValidateAll() == false)
                {
                    return;
                }

                await CustomersAppService.CreateAsync(NewCustomer);
                await GetCustomersAsync();
                await CloseCreateCustomerModalAsync();
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
            }
        }

        private async Task CloseEditCustomerModalAsync()
        {
            await EditCustomerModal.Hide();
        }

        private async Task UpdateCustomerAsync()
        {
            try
            {
                if (await EditingCustomerValidations.ValidateAll() == false)
                {
                    return;
                }

                await CustomersAppService.UpdateAsync(EditingCustomerId, EditingCustomer);
                await GetCustomersAsync();
                await EditCustomerModal.Hide();                
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
        

        private async Task GetNullableOrderLookupAsync(string newValue = null)
        {
            OrdersNullable = (await CustomersAppService.GetOrderLookupAsync(new LookupRequestDto { Filter = newValue })).Items;
        }


    }
}
