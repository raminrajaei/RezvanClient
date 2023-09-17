using System.Threading;
using ATA.HR.Client.Web.APIs.Insurance.Models.Responses;
using ATA.HR.Client.Web.Contracts;
using ATA.HR.Client.Web.Enums;
using ATA.HR.Client.Web.Extensions;
using ATA.HR.Client.Web.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;

namespace ATA.HR.Client.Web.Pages.Insurance;

public partial class CostsPrintPage
{
    // Consts
    private const int PageSize = 30;

    // Props
    private bool IsLoading { get; set; } = true;
    private bool ShowResult { get; set; }
    private int CostsSum { get; set; }

    private OperationType PageOperationType { get; set; } = OperationType.Filter;
    public List<List<CostDto>> CostsDataList { get; set; } = new();
    public (string MainInsuredName, string MainInsuredPersonnelCode, string MainInsuredSignatureUrl) PrintInfo { get; set; } = new();

    // Inject
    [Inject] public AppData AppData { get; set; }
    [Inject] public INotificationService NotificationService { get; set; }
    [Inject] public NavigationManager NavigationManager { get; set; }
    [Inject] public IJSRuntime JsRuntime { get; set; }

    [Parameter] public string? PageCallback { get; set; }

    [CascadingParameter]
    private Task<AuthenticationState> AuthenticationStateTask { get; set; }

    // Life Cycles
    protected override async Task OnInitializedAsync(CancellationToken cancellationToken)
    {
        IsLoading = true;

        try
        {
            await GetReport();
        }
        finally
        {
            IsLoading = false;

            StateHasChanged();
        }

        await base.OnInitializedAsync(cancellationToken);
    }

    private async Task GetReport()
    {
        IsLoading = true;

        try
        {
            PrintInfo = AppData.CostsPrintInfo;

            var costsData = AppData.CostsToPrint;

            var i = 1;

            CostsSum = 0;

            costsData.ForEach(c =>
            {
                c.Id = i;
                i++;
                CostsSum += c.PaidAmount;
            });

            CostsDataList = PaginateData(costsData);

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
        }
    }

    private List<List<CostDto>> PaginateData(List<CostDto> data)
    {
        List<List<CostDto>> result = new();

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

    private async Task PrintDoc()
    {
        await JSRuntime.Print();
    }
}

