using ATA.HR.Client.Web.Contracts;
using ATA.HR.Client.Web.Enums;
using ATA.HR.Client.Web.Extensions;
using ATA.HR.Shared.Dtos;
using ATA.HR.Shared.Dtos.Contract;
using ATA.HR.Shared.Enums.Contract;
using ATA.HR.Shared.Enums.Workflow;
using ATABit.Helper.Extensions;
using ATABit.Helper.Utils;
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

namespace ATA.HR.Client.Web.Pages.Contract;

[Authorize]
public partial class SendContractPage : IDisposable
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
    public string? UserNameSendingContractTo { get; set; }
    public List<int> EmploymentStatusIdAllowedToSendContractToList { get; set; } = new();

    public List<SelectListItem> BoxStatusSource { get; set; } = EnumMapping.ToSelectListItems<UserBoxStatus>();
    public List<SelectListItem> HokmTypesToSendContractToSource { get; set; } = EnumMapping.ToSelectListItems<HokmTypeStatusToSendContract>();

    #region Grid Filter Props

    private CancellationTokenSource _searchCancellationTokenSource = new();
    public PersonnelToSendContractFilterArgs PersonnelToSendContractFilterArgs { get; set; } = new();
    public Subject<string> SearchSubject { get; } = new();

    #endregion

    private List<SelectListItem> UnitsSource { get; set; } = new();

    public bool IsVisibleDeleteContractConfirmDialog { get; set; }
    public int DeletingContractId { get; set; }
    public string? DeletingContractPersonnelName { get; set; }

    // Selection
    private string? MultiActionModeSelectedValue { get; set; } = GridSelectionMode.SelectAll.ToString("D");
    private bool IsMultiActionOnCustomSelection => MultiActionModeSelectedValue == GridSelectionMode.CustomSelect.ToString("D");
    private IEnumerable<UserToSendContractDto> SelectedItems { get; set; } = Enumerable.Empty<UserToSendContractDto>();
    public bool IsVisibleMultipleActionButton => TotalUsersCounts > 0;

    // Index
    private TelerikGrid<UserToSendContractDto> PersonnelGridRef { get; set; }

    // Inject
    [Inject] public IAppDataService AppDataService { get; set; }
    [Inject] public IExcelWizardService ExcelWizardService { get; set; }
    [Inject] public INotificationService NotificationService { get; set; }
    [Inject] public NavigationManager NavigationManager { get; set; }

    [CascadingParameter]
    private Task<AuthenticationState> AuthenticationStateTask { get; set; }

    public bool IsAdmin { get; set; }

    // Life Cycles
    protected override async Task OnInitializedAsync(CancellationToken cancellationToken)
    {
        IsLoading = true;

        try
        {
            IsAdmin = await AuthenticationStateTask.IsAdminUser();

            SearchSubscriber();

            // Units
            AllUnits = await HttpClient.User().GetAllUnits(cancellationToken: cancellationToken);

            EmploymentStatusIdAllowedToSendContractToList = await HttpClient.Contract().GetEmploymentStatusIdListAllowedToSendContractTo(cancellationToken: cancellationToken);

            UnitsSource = AllUnits.Select(u => new SelectListItem(u, u)).ToList();
        }
        finally
        {
            IsLoading = false;

            StateHasChanged();
        }

        await base.OnInitializedAsync(cancellationToken);
    }

    private void OnStateInitHandler(GridStateEventArgs<UserToSendContractDto> obj)
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

        SelectedItems = Enumerable.Empty<UserToSendContractDto>();

        MultiActionModeSelectedValue = GridSelectionMode.SelectAll.ToString("D");

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

            args.Data = await HttpClient.User().GetATAPersonnelSuitableToSendContract(PersonnelToSendContractFilterArgs, context, cancellationToken);

            args.Total = TotalUsersCounts = (int)(context.TotalCount ?? 0);
        }

        finally
        {
            IsLoading = false;

            ResetPagination = false;

            IsRebindCalledBySearchSubscriber = false;

            StateHasChanged();
        }
    }

    public async Task ApplyFilters()
    {
        await RebindGrid(true);
    }

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
                    //ExceptionHandler.OnExceptionReceived(exp);
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

        if (PersonnelToSendContractFilterArgs.SearchTerm != searchTerm)
            PersonnelToSendContractFilterArgs.SearchTerm = searchTerm;
        else
            return;

        SearchSubject.OnNext(PersonnelToSendContractFilterArgs.SearchTerm);
    }

    public void Dispose()
    {
        SearchSubject.Dispose();
    }

    private void OnGridRowRender(GridRowRenderEventArgs obj)
    {
        var user = (UserToSendContractDto)obj.Item;

        ContractReadDto? userLastContract = user.Contracts.MaxBy(c => c.ContractDetailsExecutionDate);

        if (userLastContract is not null && userLastContract.ContractDetailsValidityDateJalali!.ToDateTime() - DateTime.Now < TimeSpan.FromDays(30))
            obj.Class = "contract-ending-alert";
    }

    private async Task SendContract(UserToSendContractDto user)
    {
        IsLoading = true;

        try
        {
            var addedContractId = await HttpClient.Contract().SendContractToAUser(new StartContractFlowDto { UserId = user.UserId });

            NotificationService.Toast(NotificationType.Success, $"قرارداد {user.FullName} با موفقیت  ارسال شد");

            await RebindGrid(false);
        }
        catch
        {
            // ignored
        }
        finally
        {
            IsLoading = false;
        }
    }

    private async Task OpenPreviewContract(int userId)
    {
        IsLoading = true;

        try
        {
            NotificationService.Toast(NotificationType.Success, "در حال آماده سازی پیش نمایش، لطفا منتظر بمانید");

            var addedPreviewContractId = await HttpClient.Contract().AddNewContractPreview(new ContractPreviewDto { UserId = userId });

            IsLoading = false;

            await Task.Delay(500);

            NavigationManager.NavigateTo(PageUrls.ContractFlowFormsPreviewPage(addedPreviewContractId));
        }
        catch
        {
            // ignored
        }
        finally
        {
            IsLoading = false;
        }
    }

    private async Task SendAllFilteredContracts()
    {
        if (PersonnelToSendContractFilterArgs.BoxStatusSelectedValue != UserBoxStatus.HasBox.ToString("d"))
        {
            NotificationService.Toast(NotificationType.Error, "برای ارسال همزمان قراردادها، فیلتر را روی کاربران دارای جایگاه قرار دهید. امکان ارسال قرارداد به کاربران بدون جایگاه وجود ندارد");

            return;
        }

        if (PersonnelToSendContractFilterArgs.HokmStatusSelectedValue != HokmTypeStatusToSendContract.Allowed.ToString("d"))
        {
            NotificationService.Toast(NotificationType.Error, "برای ارسال همزمان قراردادها، فیلتر را روی کاربرانی که نوع حکم مجاز دارند قرار دهید.");

            return;
        }

        try
        {
            IsLoading = true;

            List<UserToSendContractDto> usersToSendContract = new();

            Guid batchIdentifier = Guid.NewGuid();

            if (IsMultiActionOnCustomSelection is false)
            {
                usersToSendContract = await HttpClient.User().GetATAPersonnelSuitableToSendContract(PersonnelToSendContractFilterArgs);
            }
            else
            {
                usersToSendContract = SelectedItems.ToList();
            }

            int allUsersCount = usersToSendContract.Count;

            foreach (var user in usersToSendContract)
            {
                UserNameSendingContractTo = user.FullName;

                StateHasChanged();

                await HttpClient.Contract().SendContractToAUser(new StartContractFlowDto
                {
                    UserId = user.UserId,
                    BatchIdentifier = batchIdentifier
                });
            }

            NotificationService.Toast(NotificationType.Success, $"تمامی {allUsersCount} قرارداد با موفقیت ارسال شد");

            await RebindGrid(true);
        }
        catch
        {
            // ignored
        }
        finally
        {
            UserNameSendingContractTo = "";

            IsLoading = false;

            StateHasChanged();
        }
    }

    private async Task DeleteContract(UserToSendContractDto user)
    {
        var contractId = user.Contracts.FirstOrDefault()?.Id;

        if (contractId.HasValue is false)
            return;

        DeletingContractId = contractId.Value;

        DeletingContractPersonnelName = user.FullName;

        IsVisibleDeleteContractConfirmDialog = true;
    }

    private async Task DeleteContractById(int contractId)
    {
        IsLoading = true;

        try
        {
            await HttpClient.Contract().DeleteContract(new DeleteContractArgs { ContractId = contractId });

            NotificationService.Toast(NotificationType.Success, "حذف قرارداد با موفقیت انجام شد");

            IsLoading = false;

            StateHasChanged();

            await Task.Delay(1000);

            await RebindGrid(false);
        }
        catch
        {
            // Ignored
        }
        finally
        {
            IsLoading = false;

            StateHasChanged();
        }
    }
}