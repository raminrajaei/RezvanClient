using ATA.HR.Client.Web.Contracts;
using ATA.HR.Client.Web.Enums;
using ATA.HR.Shared.Dtos;
using ATA.HR.Shared.Enums.GuestHouse;
using ATABit.Helper.Extensions;
using Bit.Core.Exceptions;
using Bit.Http.Contracts;
using BootstrapBlazor.Components;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using System.Net.Http;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;
using Telerik.Blazor.Components;
using Telerik.Blazor.Extensions;

namespace ATA.HR.Client.Web.Pages.GuestHouse;

[Authorize]
public partial class BuildingPage : IDisposable
{
    // Consts
    private const int PageSize = 40;

    // Props
    public int TotalBuildingCounts { get; set; }
    public bool IsLoading { get; set; } = true;
    public bool ResetPagination { get; set; }
    public bool IsRebindCalledBySearchSubscriber { get; set; }
    public OperationType PageOperationType { get; set; } = OperationType.Filter;

    public ReferenceCity RefCity { get; set; }
    public string CityDisplay { get; set; }

    #region Grid Filter Props

    private CancellationTokenSource _searchCancellationTokenSource = new();
    public BuildingFilterArgs BuildingDataFilter { get; set; } = new();
    public Subject<string> SearchSubject { get; } = new();

    #endregion

    // Index
    private TelerikGrid<BuildingReadDto> BuildingGridRef { get; set; }
    public BuildingDto Building { get; set; } = new();

    [Inject] public IAppDataService AppDataService { get; set; }
    [Inject] public INotificationService NotificationService { get; set; }
    [Inject] public NavigationManager NavigationManager { get; set; }

    [Parameter] public string City { get; set; }

    // Life Cycles
    protected override Task OnInitializedAsync(CancellationToken cancellationToken)
    {
        IsLoading = true;

        try
        {
            if (Enum.TryParse(typeof(ReferenceCity), City, out var referenceCity))
            {
                RefCity = (ReferenceCity)Enum.Parse(typeof(ReferenceCity), City);

                BuildingDataFilter.CityId = (int)RefCity;
                CityDisplay = RefCity.ToDisplayName(true);
            }
            else
                throw new DomainLogicException("Invalid city query string");

            SearchSubscriber();
        }
        finally
        {
            IsLoading = false;

            StateHasChanged();
        }

        return base.OnInitializedAsync(cancellationToken);
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

        return BuildingGridRef.SetState(BuildingGridRef.GetState());
    }

    private void ChangeToFilterMode() => PageOperationType = OperationType.Filter;
    private void ChangeToAddBuildingMode() => PageOperationType = OperationType.Add;
    public Task ApplyFilters => RebindGrid(true);

    public async Task LoadData(GridReadEventArgs args)
    {
        try
        {
            if (ResetPagination)
            {
                args.Request.Skip = 0;
                args.Request.Page = 1;
                BuildingGridRef.Page = 1;
            }

            var oDataQuery = args.Request.ToODataString();

            var context = new ODataContext { Query = oDataQuery };

            args.Data = await HttpClient.Building().GetCityBuildings(BuildingDataFilter, context);

            args.Total = TotalBuildingCounts = (int)(context.TotalCount ?? 0);
        }

        finally
        {
            IsLoading = false;

            ResetPagination = false;

            IsRebindCalledBySearchSubscriber = false;

            StateHasChanged();
        }
    }

    public async Task OnBuildingSubmit()
    {
        try
        {
            Building.CityId = (int)RefCity;

            await HttpClient.Building().AddBuilding(Building);

            NotificationService.Toast(NotificationType.Success, $"ساختمان {Building.Title} با موفقیت ذخیره گردید");

            ChangeToFilterMode();
            Building = new();

            await RebindGrid(false);
        }
        catch (Exception exp)
        {
            ExceptionHandler.OnExceptionReceived(exp);
        }
    }

    private void SearchSubscriber()
    {
        SearchSubject
            .Throttle(TimeSpan.FromMilliseconds(500))
            .Where(t => t.IsNotNullOrEmpty() && t!.Length > 2 || string.IsNullOrEmpty(t))
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

        if (BuildingDataFilter.SearchTerm != searchTerm)
            BuildingDataFilter.SearchTerm = searchTerm;
        else
            return;

        SearchSubject.OnNext(BuildingDataFilter.SearchTerm);
    }

    public void OpenUnitOrRoomPage(BuildingReadDto building)
    {
        if (building == null) return;

        if (building.BuildingType == (int)BuildingType.Unit)
            NavigationManager.NavigateTo(PageUrls.UnitPage(building.Id));
        else
            NavigationManager.NavigateTo(PageUrls.RoomPage(building.Id, 0));
    }

    public void Dispose()
    {
        SearchSubject.Dispose();
    }
}
