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
using OneToManyTest.Orders;
using OneToManyTest.Permissions;
using OneToManyTest.Shared;

namespace OneToManyTest.Blazor.Pages
{
    public partial class Orders
    {
        protected List<Volo.Abp.BlazoriseUI.BreadcrumbItem> BreadcrumbItems = new List<Volo.Abp.BlazoriseUI.BreadcrumbItem>();
        protected PageToolbar Toolbar {get;} = new PageToolbar();
        private IReadOnlyList<OrderDto> OrderList { get; set; }
        private int PageSize { get; } = LimitedResultRequestDto.DefaultMaxResultCount;
        private int CurrentPage { get; set; } = 1;
        private string CurrentSorting { get; set; }
        private int TotalCount { get; set; }
        private bool CanCreateOrder { get; set; }
        private bool CanEditOrder { get; set; }
        private bool CanDeleteOrder { get; set; }
        private OrderCreateDto NewOrder { get; set; }
        private Validations NewOrderValidations { get; set; }
        private OrderUpdateDto EditingOrder { get; set; }
        private Validations EditingOrderValidations { get; set; }
        private Guid EditingOrderId { get; set; }
        private Modal CreateOrderModal { get; set; }
        private Modal EditOrderModal { get; set; }
        private GetOrdersInput Filter { get; set; }
        private DataGridEntityActionsColumn<OrderDto> EntityActionsColumn { get; set; }
        protected string SelectedCreateTab = "order-create-tab";
        protected string SelectedEditTab = "order-edit-tab";
        
        public Orders()
        {
            NewOrder = new OrderCreateDto();
            EditingOrder = new OrderUpdateDto();
            Filter = new GetOrdersInput
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
            BreadcrumbItems.Add(new Volo.Abp.BlazoriseUI.BreadcrumbItem(L["Menu:Orders"]));
            return ValueTask.CompletedTask;
        }

        protected virtual ValueTask SetToolbarItemsAsync()
        {
            Toolbar.AddButton(L["ExportToExcel"], async () =>{ await DownloadAsExcelAsync(); }, IconName.Download);
            
            Toolbar.AddButton(L["NewOrder"], async () =>
            {
                await OpenCreateOrderModalAsync();
            }, IconName.Add, requiredPolicyName: OneToManyTestPermissions.Orders.Create);

            return ValueTask.CompletedTask;
        }

        private async Task SetPermissionsAsync()
        {
            CanCreateOrder = await AuthorizationService
                .IsGrantedAsync(OneToManyTestPermissions.Orders.Create);
            CanEditOrder = await AuthorizationService
                            .IsGrantedAsync(OneToManyTestPermissions.Orders.Edit);
            CanDeleteOrder = await AuthorizationService
                            .IsGrantedAsync(OneToManyTestPermissions.Orders.Delete);
        }

        private async Task GetOrdersAsync()
        {
            Filter.MaxResultCount = PageSize;
            Filter.SkipCount = (CurrentPage - 1) * PageSize;
            Filter.Sorting = CurrentSorting;

            var result = await OrdersAppService.GetListAsync(Filter);
            OrderList = result.Items;
            TotalCount = (int)result.TotalCount;
        }

        protected virtual async Task SearchAsync()
        {
            CurrentPage = 1;
            await GetOrdersAsync();
            await InvokeAsync(StateHasChanged);
        }

        private  async Task DownloadAsExcelAsync()
        {
            var token = (await OrdersAppService.GetDownloadTokenAsync()).Token;
            NavigationManager.NavigateTo($"/api/app/orders/as-excel-file?DownloadToken={token}", forceLoad: true);
        }

        private async Task OnDataGridReadAsync(DataGridReadDataEventArgs<OrderDto> e)
        {
            CurrentSorting = e.Columns
                .Where(c => c.SortDirection != SortDirection.Default)
                .Select(c => c.Field + (c.SortDirection == SortDirection.Descending ? " DESC" : ""))
                .JoinAsString(",");
            CurrentPage = e.Page;
            await GetOrdersAsync();
            await InvokeAsync(StateHasChanged);
        }

        private async Task OpenCreateOrderModalAsync()
        {
            NewOrder = new OrderCreateDto{
                
                
            };
            await NewOrderValidations.ClearAll();
            await CreateOrderModal.Show();
        }

        private async Task CloseCreateOrderModalAsync()
        {
            NewOrder = new OrderCreateDto{
                
                
            };
            await CreateOrderModal.Hide();
        }

        private async Task OpenEditOrderModalAsync(OrderDto input)
        {
            var order = await OrdersAppService.GetAsync(input.Id);
            
            EditingOrderId = order.Id;
            EditingOrder = ObjectMapper.Map<OrderDto, OrderUpdateDto>(order);
            await EditingOrderValidations.ClearAll();
            await EditOrderModal.Show();
        }

        private async Task DeleteOrderAsync(OrderDto input)
        {
            await OrdersAppService.DeleteAsync(input.Id);
            await GetOrdersAsync();
        }

        private async Task CreateOrderAsync()
        {
            try
            {
                if (await NewOrderValidations.ValidateAll() == false)
                {
                    return;
                }

                await OrdersAppService.CreateAsync(NewOrder);
                await GetOrdersAsync();
                await CloseCreateOrderModalAsync();
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
            }
        }

        private async Task CloseEditOrderModalAsync()
        {
            await EditOrderModal.Hide();
        }

        private async Task UpdateOrderAsync()
        {
            try
            {
                if (await EditingOrderValidations.ValidateAll() == false)
                {
                    return;
                }

                await OrdersAppService.UpdateAsync(EditingOrderId, EditingOrder);
                await GetOrdersAsync();
                await EditOrderModal.Hide();                
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
        

    }
}
