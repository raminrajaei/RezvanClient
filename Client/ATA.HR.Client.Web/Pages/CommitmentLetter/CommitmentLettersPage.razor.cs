using ATA.HR.Client.Web.Contracts;
using ATA.HR.Client.Web.Enums;
using ATA.HR.Shared.Dtos.CommitmentLetter;
using ATABit.Helper.Extensions;
using ATABit.Shared;
using Bit.Http.Contracts;
using BlazorDownloadFile;
using ExcelWizard.Models;
using ExcelWizard.Models.EWExcel;
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

namespace ATA.HR.Client.Web.Pages.CommitmentLetter;

[Authorize]
public partial class CommitmentLettersPage : IDisposable
{
    // Consts
    private const int PageSize = 30;

    // Props
    public int TotalCommitmentLettersCounts { get; set; }
    private bool IsLoading { get; set; } = true;
    public OperationType PageOperationType { get; set; } = OperationType.Filter;
    public bool ResetPagination { get; set; }
    public bool IsRebindCalledBySearchSubscriber { get; set; }

    #region Grid Filter Props

    private CancellationTokenSource _searchCancellationTokenSource = new();
    public CommitmentLettersFilterArgs CommitmentLettersFilter { get; set; } = new() { UserStatusSelectedValue = "active" };
    public Subject<string> SearchSubject { get; } = new();

    #endregion

    private List<SelectListItem> PersonnelSource { get; set; } = new();
    public CommitmentLetterDto CommitmentLetter { get; set; } = new();
    public bool IsVisibleDeleteCommitmentLetterConfirmDialog { get; set; }
    public int DeletingCommitmentLetterId { get; set; }
    private List<SelectListItem> UserDismissStatusSource { get; set; } = new()
    {
        new SelectListItem("فعال", "active"),
        new SelectListItem("راکد", "dismissed")
    };

    // Index
    private TelerikGrid<CommitmentLetterReadDto> CommitmentLettersGridRef { get; set; }

    // Inject
    [Inject] public IAppDataService AppDataService { get; set; }
    [Inject] public IExcelWizardService ExcelWizardService { get; set; }
    [Inject] public INotificationService NotificationService { get; set; }
    [Inject] public NavigationManager NavigationManager { get; set; }

    [CascadingParameter]
    private Task<AuthenticationState> AuthenticationStateTask { get; set; }

    // Life Cycles
    protected override async Task OnInitializedAsync(CancellationToken cancellationToken)
    {
        IsLoading = true;

        try
        {
            PersonnelSource = await HttpClient.User().GetATAPersonnelSourceSuitableToRegisterCommitmentLetter(cancellationToken: cancellationToken);

            SearchSubscriber();
        }
        finally
        {
            IsLoading = false;

            StateHasChanged();
        }

        await base.OnInitializedAsync(cancellationToken);
    }

    private void OnStateInitHandler(GridStateEventArgs<CommitmentLetterReadDto> obj)
    {

    }

    // Methods

    public string ToAbsoluteUrl(string relativeUrl)
    {
        return $"{HttpClient.BaseAddress}{relativeUrl}";
    }

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

        return CommitmentLettersGridRef.SetState(CommitmentLettersGridRef.GetState());
    }

    public async Task LoadData(GridReadEventArgs args)
    {
        try
        {
            if (ResetPagination)
            {
                args.Request.Skip = 0;
                args.Request.Page = 1;
                CommitmentLettersGridRef.Page = 1;
            }

            var oDataQuery = args.Request.ToODataString();

            var context = new ODataContext { Query = oDataQuery };

            var cancellationToken = IsRebindCalledBySearchSubscriber ? _searchCancellationTokenSource.Token : CancellationToken.None;

            args.Data = await HttpClient.CommitmentLetter().GetAllCommitmentLetters(CommitmentLettersFilter, context, cancellationToken);

            args.Total = TotalCommitmentLettersCounts = (int)(context.TotalCount ?? 0);
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

        if (CommitmentLettersFilter.SearchTerm != searchTerm)
            CommitmentLettersFilter.SearchTerm = searchTerm;
        else
            return;

        SearchSubject.OnNext(CommitmentLettersFilter.SearchTerm);
    }

    public void Dispose()
    {
        SearchSubject.Dispose();
    }

    private void OnGridRowRender(GridRowRenderEventArgs obj)
    {
        var commitmentLetter = (CommitmentLetterReadDto)obj.Item;

        if (commitmentLetter.ValidityDuration < TimeSpan.FromDays(60))
            obj.Class = "contract-ending-alert";
    }

    private void ChangeToAddMode()
    {
        CommitmentLetter = new();

        PageOperationType = OperationType.Add;
    }

    private async Task OnCommitmentLetterSubmit()
    {
        IsLoading = true;

        try
        {
            if (PageOperationType is OperationType.Add)
            {
                await HttpClient.CommitmentLetter().AddCommitmentLetter(CommitmentLetter);

                NotificationService.Toast(NotificationType.Success, "تعهدنامه با موفقیت ذخیره گردید");
            }
            else
            {
                //await HttpClient.PersonnelDocument().EditDocument(new PersonnelDocumentEditDto(PersonnelDocument.Id!.Value!, PersonnelDocument.Description));

                NotificationService.Toast(NotificationType.Success, "سند با موفقیت ویرایش شد");
            }

            await RebindGrid(false);

            ChangeToFilterMode();
        }
        catch
        {
            // ignored
        }
        finally
        {
            IsLoading = false;

            StateHasChanged();
        }
    }

    private async Task DeleteCommitmentLetter(int commitmentLetterId)
    {
        try
        {
            await HttpClient.CommitmentLetter().DeleteCommitmentLetter(new DeleteCommitmentLetterArgs { CommitmentLetterId = commitmentLetterId });

            NotificationService.Toast(NotificationType.Success, "حذف تعهدنامه با موفقیت انجام شد");
        }
        catch
        {
            // ignored
        }
        finally
        {
            await RebindGrid(false);

            StateHasChanged();
        }
    }

    private async Task ChangeToEditMode(int documentId)
    {
        IsLoading = true;

        try
        {
            //PersonnelDocument = await HttpClient.PersonnelDocument().GetDocumentByIdForForm(documentId);

            PageOperationType = OperationType.Edit;
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

    private void ShowDeleteCommitmentLetterConfirmDialog(int commitmentLetterIdToDelete)
    {
        DeletingCommitmentLetterId = commitmentLetterIdToDelete;

        IsVisibleDeleteCommitmentLetterConfirmDialog = true;
    }

    private void OpenUserCommitmentLettersPage(int clUserIdEmployee)
    {
        NavigationManager.NavigateTo(PageUrls.PersonnelDocumentsPageWithParams(clUserIdEmployee, 8, 22, true));
    }

    private async Task<DownloadFileResult> ExportDataToExcel()
    {
        DownloadFileResult downloadFileResult = null;

        try
        {
            IsLoading = true;

            StateHasChanged();

            var reportData = await HttpClient.CommitmentLetter().GetAllCommitmentLetters(CommitmentLettersFilter);

            IExcelBuilder excelBuilder = ExcelBuilder
                .SetGeneratedFileName("تعهدنامه‌های محضری")
                .CreateGridLayoutExcel()
                .WithOneSheetUsingModelBinding(reportData)
                .Build();

            downloadFileResult = await ExcelWizardService.GenerateAndDownloadExcelInBlazor(excelBuilder);
        }
        catch (Exception e)
        {
            //Ignored
        }
        finally
        {
            IsLoading = false;

            StateHasChanged();
        }

        return downloadFileResult;
    }
}