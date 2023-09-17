using System.Threading;
using ATA.HR.Client.Web.APIs;
using ATA.HR.Client.Web.APIs.Insurance.Models.Requests;
using ATA.HR.Client.Web.APIs.Insurance.Models.Responses;
using ATA.HR.Client.Web.Contracts;
using ATA.HR.Client.Web.Enums;
using ATA.HR.Client.Web.Extensions;
using ATABit.Shared;
using BlazorDownloadFile;
using DNTPersianUtils.Core;
using ExcelWizard.Models.EWExcel;
using ExcelWizard.Models;
using ExcelWizard.Service;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using Telerik.Blazor.Components;

namespace ATA.HR.Client.Web.Pages.Insurance;

public partial class FinancialReportPage
{
    // Consts
    private const int PageSize = 50;

    // Props
    private bool IsLoading { get; set; } = true;
    private bool ShowResult { get; set; }
    private int CostsSum => GetCostsSum();

    private int GetCostsSum()
    {
        if (IsMultiActionOnCustomSelection)
        {
            return SelectedItems.Sum(f => f.Amount);
        }
        else
        {
            return FinancialReportRawList.Sum(f => f.Amount);
        }
    }

    private int TotalReportItemsCount { get; set; }

    private OperationType PageOperationType { get; set; } = OperationType.Filter;
    private List<List<FinancialReportDto>> FinancialReportList { get; set; } = new();
    private List<BankPaymentDto> BankPayments { get; set; } = new();
    private List<FinancialReportDto> FinancialReportRawList { get; set; } = new();
    private GetFinancialReportQuery FinancialReportQuery { get; set; } = new();

    private List<SelectListItem> BankPaymentsSource => BankPayments
        .Select(p => new SelectListItem($"#{p.PaymentNo} [ {p.PayedAtJalali} ]", p.Id.ToString())).ToList();

    private string PaymentStatusSelectedValue { get; set; } = "0";

    private List<SelectListItem> PaymentStatusSource { get; set; } = new List<SelectListItem>()
    {
        new("پرداخت نشده", "0"),
        new("پرداخت شده", "1")
    };

    private int? PageActiveBankPaymentNo { get; set; }
    private SaveBankPaymentCommand SaveBankPaymentCommand { get; set; } = new();

    private string? MultiActionModeSelectedValue { get; set; } = GridSelectionMode.SelectAll.ToString("D");
    private bool IsMultiActionOnCustomSelection => MultiActionModeSelectedValue == GridSelectionMode.CustomSelect.ToString("D");
    private bool IsVisibleMultipleActionButton { get; set; }
    private IEnumerable<FinancialReportDto> SelectedItems { get; set; } = Enumerable.Empty<FinancialReportDto>();

    private bool IsVisibleSaveBankPaymentDataModal { get; set; }

    private TelerikGrid<FinancialReportDto> ReportGridRef { get; set; }

    // Inject
    //[Inject] public AppData AppData { get; set; }
    [Inject] public INotificationService NotificationService { get; set; }
    [Inject] public NavigationManager NavigationManager { get; set; }
    [Inject] public IJSRuntime JsRuntime { get; set; }
    [Inject] public ICoreAPIs CoreAPIs { get; set; }
    [Inject] public IExcelWizardService ExcelWizardService { get; set; }

    [Parameter] public string? PageCallback { get; set; }

    [CascadingParameter]
    private Task<AuthenticationState> AuthenticationStateTask { get; set; }

    // Life Cycles
    protected override async Task OnInitializedAsync(CancellationToken cancellationToken)
    {
        IsLoading = true;

        try
        {
            BankPayments = await CoreAPIs.Insurance_GetAllBankPayments();

            await GetReport(null);
        }
        finally
        {
            IsLoading = false;

            StateHasChanged();
        }

        await base.OnInitializedAsync(cancellationToken);
    }

    private async Task ApplyFilters()
    {
        if (PaymentStatusSelectedValue == "0")
        {
            FinancialReportQuery.BankPaymentId = null;
        }
        else
        {
            if (FinancialReportQuery.BankPaymentId.HasValue is false)
            {
                var lastPaymentId = BankPayments.LastOrDefault()?.Id;

                FinancialReportQuery.BankPaymentId = lastPaymentId ?? 0;
            }

            var payment = BankPayments.FirstOrDefault(x => x.Id == FinancialReportQuery.BankPaymentId);

            if (payment is not null)
                PageActiveBankPaymentNo = payment.PaymentNo;
        }

        await GetReport(FinancialReportQuery.BankPaymentId);
    }

    private async Task GetReport(int? bankPaymentId)
    {
        IsLoading = true;

        try
        {
            FinancialReportRawList = await CoreAPIs.Insurance_GetFinancialReport(FinancialReportQuery);

            TotalReportItemsCount = FinancialReportList.Count;

            IsVisibleMultipleActionButton = TotalReportItemsCount > 1;

            var i = 1;

            //CostsSum = FinancialReportRawList.Sum(f => f.Amount);

            FinancialReportList = PaginateData(FinancialReportRawList);

            ShowResult = true;
        }
        catch
        {
            // Ignored

            ShowResult = false;
        }
        finally
        {
            IsLoading = false;

            StateHasChanged();
        }
    }

    private List<List<FinancialReportDto>> PaginateData(List<FinancialReportDto> data)
    {
        List<List<FinancialReportDto>> result = new();

        bool go = true;

        int currentIndex = 0;

        int remainedSize = data.Count;

        while (go)
        {
            int i = 18;

            if (remainedSize is >= 16 and <= 18)
            {
                i = 15;
            }

            var pageData = data.Skip(currentIndex).Take(i).ToList();

            result.Add(pageData);
            currentIndex += i;

            remainedSize -= i;

            if (currentIndex > data.Count)
                go = false;
        }

        return result;
    }

    private async Task OnReadHandlerReportGrid(GridReadEventArgs args)
    {
        await LoadReportData(args);
    }

    private async Task LoadReportData(GridReadEventArgs args)
    {
        //try
        //{
        //    var oDataQuery = args.Request.ToODataString();

        //    var context = new ODataContext { Query = oDataQuery };

        //    var cancellationToken = IsRebindCalledBySearchSubscriber ? _searchCancellationTokenSource.Token : CancellationToken.None;

        //    var data = await HttpClient.WorkHour().GetAllWorkHours(WorkHoursFilter, context, cancellationToken);

        //    args.Data = data;

        //    args.Total = (int)(context.TotalCount ?? 0);
        //}

        //finally
        //{
        //    IsLoading = false;

        //    ResetPagination = false;

        //    IsRebindCalledBySearchSubscriber = false;

        //    StateHasChanged();
        //}

    }

    private async Task GridSelectionModeChanged(object obj)
    {
        SelectedItems = new List<FinancialReportDto>();

        await ReportGridRef.SetState(ReportGridRef.GetState());

        StateHasChanged();
    }

    private void BackButtonHandler()
    {
        if (PageCallback == "allcosts")
        {
            NavigationManager.NavigateTo(PageUrls.AllCostsPage);
        }
        else
        {
            NavigationManager.NavigateTo(PageUrls.MyCostsPage);
        }
    }

    private void OpenSaveBankPaymentModal()
    {
        List<int> personnelCodesToPay;

        if (IsMultiActionOnCustomSelection)
        {
            if (SelectedItems.Any() is false)
            {
                NotificationService.Toast(NotificationType.Error, "هیچ هزینه‌ای انتخاب نشده است");

                return;
            }

            personnelCodesToPay = SelectedItems.ToList().Select(x => x.PersonnelCode).ToList();
        }
        else
        {
            personnelCodesToPay = FinancialReportRawList.Select(x => x.PersonnelCode).ToList();
        }

        SaveBankPaymentCommand.PersonnelCodes = personnelCodesToPay;

        IsVisibleSaveBankPaymentDataModal = true;
    }
    
    private async Task PrintDoc()
    {
        await JSRuntime.Print();
    }

    private async Task OnSaveBankPaymentSubmit()
    {
        if (string.IsNullOrWhiteSpace(SaveBankPaymentCommand.PayedAtJalali))
        {
            NotificationService.Toast(NotificationType.Error, "تاریخ پرداخت را انتخاب نکرده‌اید");
            return;
        }

        if (string.IsNullOrWhiteSpace(SaveBankPaymentCommand.BankTrackingNo))
        {
            NotificationService.Toast(NotificationType.Error, "کد رهگیری بانک را وارد نکرده‌اید");
            return;
        }

        if (SaveBankPaymentCommand.PersonnelCodes.Count == 0)
        {
            NotificationService.Toast(NotificationType.Error, "هیچ هزینه‌ای برای ثبت پرداخت وجود ندارد");
            return;
        }

        IsLoading = true;

        try
        {
            await CoreAPIs.Insurance_SaveBankPayment(SaveBankPaymentCommand);

            NotificationService.Toast(NotificationType.Success, "ذخیره اطلاعات با موفقیت انجام شد");

            BankPayments = new();

            BankPayments = await CoreAPIs.Insurance_GetAllBankPayments();

            await GetReport(null);

            SelectedItems = new List<FinancialReportDto>();

            MultiActionModeSelectedValue = GridSelectionMode.SelectAll.ToString("D");

            await ReportGridRef.SetState(ReportGridRef.GetState());
        }
        catch
        {
            // Ignore
        }
        finally
        {
            IsLoading = false;

            IsVisibleSaveBankPaymentDataModal = false;

            StateHasChanged();
        }
    }

    private async Task<DownloadFileResult> ExcelExport()
    {
        DownloadFileResult downloadFileResult = null;

        try
        {
            IsLoading = true;

            StateHasChanged();

            if (FinancialReportRawList is null || FinancialReportRawList.Count == 0)
            {
                NotificationService.Toast(NotificationType.Error, "هیچ رکوردی برای خروجی اکسل یافت نشد");
            }
            else
            {
                if(IsMultiActionOnCustomSelection && SelectedItems.Any() is false)
                {
                    NotificationService.Toast(NotificationType.Error, "هیچ رکوردی برای خروجی اکسل انتخاب نشده است");
                }
                else
                {
                    //FinancialReportRawList = FinancialReportRawList.OrderBy(wh => wh.UserEmployeePersonnelCode!.Value).ToList();

                    List<FinancialReportDto> report = new List<FinancialReportDto>();

                    if (IsMultiActionOnCustomSelection)
                        report.AddRange(SelectedItems.ToList());
                    else
                        report.AddRange(FinancialReportRawList);

                    IExcelBuilder excelBuilder = ExcelBuilder
                        .SetGeneratedFileName($"لیست پرداخت – {PageActiveBankPaymentNo}")
                        .CreateGridLayoutExcel()
                        .WithOneSheetUsingModelBinding(report)
                        .Build();

                    downloadFileResult = await ExcelWizardService.GenerateAndDownloadExcelInBlazor(excelBuilder);
                }
            }
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

