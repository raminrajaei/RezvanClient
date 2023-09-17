using ATA.HR.Client.Web.Contracts;
using ATA.HR.Client.Web.Enums;
using ATA.HR.Shared;
using ATA.HR.Shared.Dtos;
using ATABit.Helper.Extensions;
using ATABit.Shared;
using Bit.Http.Contracts;
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

namespace ATA.HR.Client.Web.Pages.Signature;

[Authorize(Claims.GatherSignaturesPermission)]
public partial class SignaturesPage : IDisposable
{
    // Consts
    private const int PageSize = 50;

    // Props
    public int TotalUsersCounts { get; set; }
    private bool IsLoading { get; set; } = true;
    public List<string> AllUnits { get; set; } = new();
    public OperationType PageOperationType { get; set; } = OperationType.Filter;
    public bool ResetPagination { get; set; }
    public bool IsRebindCalledBySearchSubscriber { get; set; }
    public UserWithSignatureDto? UserGettingSignature { get; set; }
    public bool IsVisibleTakeSignatureWindow { get; set; }
    public int TotalCount { get; set; }
    public int SignedCount { get; set; }
    public int NotSignedCount => TotalCount - SignedCount;

    public string? Result { get; set; }

    #region Grid Filter Props

    private CancellationTokenSource _searchCancellationTokenSource = new();
    public SignatureFilterArgs SignatureFilters { get; set; } = new();
    public Subject<string> SearchSubject { get; } = new();

    #endregion

    private List<SelectListItem> UnitsSource { get; set; } = new();

    // Index
    private TelerikGrid<UserWithSignatureDto> PersonnelGridRef { get; set; }

    // Inject
    [Inject] public IAppDataService AppDataService { get; set; }
    [Inject] public IExcelWizardService ExcelWizardService { get; set; }
    [Inject] public INotificationService NotificationService { get; set; }

    [CascadingParameter]
    private Task<AuthenticationState> AuthenticationStateTask { get; set; }

    // Life Cycles
    protected override async Task OnInitializedAsync(CancellationToken cancellationToken)
    {
        IsLoading = true;

        try
        {
            SearchSubscriber();

            // Units
            AllUnits = await HttpClient.User().GetAllUnits(cancellationToken: cancellationToken);

            UnitsSource = AllUnits.Select(u => new SelectListItem(u, u)).ToList();
        }
        finally
        {
            IsLoading = false;

            StateHasChanged();
        }

        await base.OnInitializedAsync(cancellationToken);
    }

    private void OnStateInitHandler(GridStateEventArgs<UserWithSignatureDto> obj)
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

        return PersonnelGridRef.SetState(PersonnelGridRef.GetState());
    }

    public async Task LoadData(GridReadEventArgs args)
    {
        try
        {
            if (ResetPagination)
            {
                args.Request.Skip = 0;
                args.Request.Page = 1;
                PersonnelGridRef.Page = 1;
            }

            var oDataQuery = args.Request.ToODataString();

            var context = new ODataContext { Query = oDataQuery };

            var cancellationToken = IsRebindCalledBySearchSubscriber ? _searchCancellationTokenSource.Token : CancellationToken.None;

            args.Data = await HttpClient.User().GetPersonnelToGetSignatures(SignatureFilters, context, cancellationToken);

            args.Total = TotalCount = TotalUsersCounts = (int)(context.TotalCount ?? 0);

            SignedCount = await HttpClient.User().GetPersonnelSignedCount(SignatureFilters, context, cancellationToken);
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

        if (SignatureFilters.SearchTerm != searchTerm)
            SignatureFilters.SearchTerm = searchTerm;
        else
            return;

        SearchSubject.OnNext(SignatureFilters.SearchTerm);
    }

    public void Dispose()
    {
        SearchSubject.Dispose();
    }

    private void OnGridRowRender(GridRowRenderEventArgs obj)
    {
        var user = (UserWithSignatureDto)obj.Item;

        if (user.HasUserSignature is false)
            obj.Class = "contract-ending-alert";
    }

    private void RegisterSignature(UserWithSignatureDto user)
    {
        UserGettingSignature = user;

        IsVisibleTakeSignatureWindow = true;
    }

    private async Task OnResult(string result)
    {
        Result = result;

        if (string.IsNullOrWhiteSpace(result))
        {
            // Clean the signature
            return;
        }

        IsLoading = true;

        try
        {
            await HttpClient.User().RegisterUserSignature(new UserSignatureDto
            {
                PersonnelCode = UserGettingSignature!.PersonnelCode,
                SignatureData = result
            });

            await RebindGrid(false);
        }
        catch
        {
            // ignored
        }
        finally
        {
            IsLoading = false;

            IsVisibleTakeSignatureWindow = false;

            StateHasChanged();
        }
    }
}