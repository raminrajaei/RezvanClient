using ATA.HR.Client.Web.Contracts;
using ATA.HR.Shared.Dtos;
using ATA.HR.Shared.Dtos.Contract;
using Bit.Http.Contracts;
using ExcelWizard.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http;
using System.Threading;
using Telerik.Blazor.Components;
using Telerik.Blazor.Extensions;

namespace ATA.HR.Client.Web.Pages.Contract;

[Authorize]
public partial class MyContractsPage
{
    // Consts
    private const int PageSize = 20;

    // Props
    private bool IsLoading { get; set; } = true;

    #region Grid Filter Props

    public MyContractsFilterArgs MyContractsFilter { get; set; } = new();

    #endregion

    // Index
    private TelerikGrid<ContractReadDto> MyContractGridRef { get; set; }

    // Inject
    [Inject] public IAppDataService AppDataService { get; set; }
    [Inject] public IExcelWizardService ExcelWizardService { get; set; }
    [Inject] private NavigationManager NavigationManager { get; set; }

    [CascadingParameter]
    private Task<AuthenticationState> AuthenticationStateTask { get; set; }


    // Life Cycles
    protected override Task OnInitializedAsync(CancellationToken cancellationToken)
    {
        IsLoading = true;

        try
        {

        }
        finally
        {
            IsLoading = false;

            StateHasChanged();
        }

        return base.OnInitializedAsync(cancellationToken);
    }

    private void OnStateInitHandler(GridStateEventArgs<ContractReadDto> obj)
    {

    }

    // Methods

    protected async Task OnReadHandler(GridReadEventArgs args)
    {
        await LoadData(args);
    }

    // Make the grid call OnRead to request data again
    // As part of our 3.0.1 release we introduced the Rebind method to the component reference. This would make the rest of the approaches in this article obsolete.
    private Task RebindGrid(bool resetPagination, bool isCalledBySearchSubscriber = false)
    {
        IsLoading = true;

        return MyContractGridRef.SetState(MyContractGridRef.GetState());
    }

    public async Task LoadData(GridReadEventArgs args)
    {
        try
        {
            var oDataQuery = args.Request.ToODataString();

            var context = new ODataContext { Query = oDataQuery };

            args.Data = await HttpClient.Contract().GetMyContracts(MyContractsFilter, context);

            args.Total = (int)(context.TotalCount ?? 0);
        }

        finally
        {
            IsLoading = false;

            StateHasChanged();
        }
    }

    public Task ApplyFilters => RebindGrid(true);

    private void OpenContractFlowFormsPage(int contractId)
    {
        NavigationManager.NavigateTo(PageUrls.ContractFlowFormsPage(contractId));
    }

    private Task GetMyContracts(bool isActive, bool isPending, bool isAll)
    {
        MyContractsFilter.Active = isActive;

        MyContractsFilter.Pending = isPending;

        MyContractsFilter.All = isAll;

        return ApplyFilters;
    }
}