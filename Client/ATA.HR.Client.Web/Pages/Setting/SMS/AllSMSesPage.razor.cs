using ATA.HR.Client.Web.Contracts;
using ATA.HR.Client.Web.Enums;
using ATA.HR.Client.Web.Extensions;
using ATA.HR.Shared.Dtos.AppGeneric;
using ATA.HR.Shared.Dtos.Contract;
using ATA.HR.Shared.Enums.AppGeneric;
using ATABit.Helper.Extensions;
using ATABit.Helper.Utils;
using ATABit.Shared;
using Bit.Http.Contracts;
using BootstrapBlazor.Components;
using ExcelWizard.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;
using Telerik.Blazor.Components;
using Telerik.Blazor.Extensions;

namespace ATA.HR.Client.Web.Pages.Setting.SMS;

[Authorize]
public partial class AllSMSesPage : IDisposable
{
    // Consts
    private const int PageSize = 30;

    // Props
    private bool IsLoading { get; set; } = true;
    private string? LoadingText { get; set; }
    //public List<string> AllUnits { get; set; } = new();
    public OperationType PageOperationType { get; set; } = OperationType.Filter;
    public bool ResetPagination { get; set; }
    public bool IsRebindCalledBySearchSubscriber { get; set; }
    public int TotalCount { get; set; }
    public bool IsAdmin { get; set; }

    #region Grid Filter Props

    private CancellationTokenSource _searchCancellationTokenSource = new();
    public AllAppSMSesFilterArgs AllAppSMSesFilter { get; set; } = new();
    public Subject<string> SearchSubject { get; } = new();

    #endregion

    //private List<SelectListItem> UnitsSource { get; set; } = new();
    //private List<SelectListItem> WorkLocationsSource { get; set; } = new();
    private List<SelectListItem> AppSMSTypesSource => EnumMapping.ToSelectListItems<AppSMSType>();

    // Index
    private TelerikGrid<AppSMSReadDto> AppSMSesGridRef { get; set; }

    // Inject
    [Inject] public IAppDataService AppDataService { get; set; }
    [Inject] public IExcelWizardService ExcelWizardService { get; set; }
    [Inject] public NavigationManager NavigationManager { get; set; }
    [Inject] public INotificationService NotificationService { get; set; }


    [CascadingParameter]
    private Task<AuthenticationState> AuthenticationStateTask { get; set; }

    // Life Cycles
    protected override async Task OnInitializedAsync(CancellationToken cancellationToken)
    {
        IsLoading = true;

        try
        {
            IsAdmin = await AuthenticationStateTask.IsAdminUser();

            SearchSubscriber();

            // Units
            //AllUnits = await HttpClient.User().GetAllUnits(cancellationToken: cancellationToken);

            //UnitsSource = AllUnits.Select(u => new SelectListItem(u, u)).ToList();

            // WorkLocations
            //var workLocations = await HttpClient.User().GetAllWorkLocations(cancellationToken: cancellationToken);

            //WorkLocationsSource = workLocations.Select(wl => new SelectListItem(wl, wl)).ToList();
        }
        finally
        {
            IsLoading = false;

            StateHasChanged();
        }

        await base.OnInitializedAsync(cancellationToken);
    }

    private void OnStateInitHandler(GridStateEventArgs<AppSMSReadDto> obj)
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

        ResetPagination = resetPagination;

        IsRebindCalledBySearchSubscriber = isCalledBySearchSubscriber;

        return AppSMSesGridRef.SetState(AppSMSesGridRef.GetState());
    }

    public async Task LoadData(GridReadEventArgs args)
    {
        try
        {
            if (ResetPagination)
            {
                args.Request.Skip = 0;
                args.Request.Page = 1;
                AppSMSesGridRef.Page = 1;
            }

            var oDataQuery = args.Request.ToODataString();

            var context = new ODataContext { Query = oDataQuery };

            var cancellationToken = IsRebindCalledBySearchSubscriber ? _searchCancellationTokenSource.Token : CancellationToken.None;

            args.Data = await HttpClient.AppSMS().GetAllAppSMSes(AllAppSMSesFilter, context, cancellationToken);

            args.Total = TotalCount = (int)(context.TotalCount ?? 0);
        }

        finally
        {
            IsLoading = false;

            ResetPagination = false;

            IsRebindCalledBySearchSubscriber = false;

            StateHasChanged();
        }
    }

    public Task ApplyFilters => RebindGrid(true);

    private void ChangeToFilterMode() => PageOperationType = OperationType.Filter;

    private void SearchSubscriber()
    {
        SearchSubject
            .Throttle(TimeSpan.FromMilliseconds(500))
            .Where(t => t.IsNotNullOrEmpty() && (t!.Length > 2 || t.IsInt()) || string.IsNullOrEmpty(t))
            .Subscribe(async _ =>
            {
                try
                {
                    IsLoading = true;

                    _searchCancellationTokenSource.Cancel();

                    _searchCancellationTokenSource.Dispose();

                    _searchCancellationTokenSource = new CancellationTokenSource();

                    await RebindGrid(true, true);
                }
                catch (Exception exp)
                {
                    ExceptionHandler.OnExceptionReceived(exp);
                }
                finally
                {
                    StateHasChanged();
                }
            });
    }

    public void SearchTermChanged(object? searchObject)
    {
        var searchTerm = searchObject?.ToString();

        if (AllAppSMSesFilter.SearchTerm != searchTerm)
            AllAppSMSesFilter.SearchTerm = searchTerm;
        else
            return;

        SearchSubject.OnNext(AllAppSMSesFilter.SearchTerm);
    }

    public void Dispose()
    {
        SearchSubject.Dispose();
    }

    private void OnGridRowRender(GridRowRenderEventArgs obj)
    {
        var user = (AppSMSReadDto)obj.Item;

        //if (((UserDto)user.OrderStatus).IsDelete())
        //    obj.Class = "deleted-order";
    }
}