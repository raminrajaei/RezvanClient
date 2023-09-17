using ATA.HR.Client.Web.APIs;
using ATA.HR.Client.Web.APIs.Enums;
using ATA.HR.Client.Web.APIs.HRInsuranceModels;
using ATA.HR.Client.Web.APIs.Insurance.Models.Requests;
using ATA.HR.Client.Web.APIs.Insurance.Models.Responses;
using ATA.HR.Client.Web.Contracts;
using ATA.HR.Client.Web.Enums;
using ATA.HR.Client.Web.Extensions;
using ATA.HR.Client.Web.Models;
using ATA.HR.Shared;
using ATA.HR.Shared.Dtos.Document;
using ATABit.Helper.Extensions;
using ATABit.Shared;
using Bit.Core.Exceptions;
using Bit.Http.Contracts;
using BlazorDownloadFile;
using DNTPersianUtils.Core;
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
using ATA.HR.Shared.Dtos.PaySlip;
using Telerik.Blazor;
using Telerik.Blazor.Components;
using Telerik.Blazor.Extensions;
using GridSelectionMode = ATA.HR.Client.Web.Enums.GridSelectionMode;

namespace ATA.HR.Client.Web.Pages.PaySlip;

[Authorize]
public partial class PaySlipPage
{
    // Props
    public List<PaySlipReadDto> PaySlipData { get; set; } = new();
    private bool IsLoading { get; set; } = true;
    private OperationType PageOperationType { get; set; } = OperationType.Filter;

    private List<SelectListItem> YearsSource { get; set; } = new();
    private List<SelectListItem> MonthsSource { get; set; } = new();

    // Parameter
    //[Parameter] public int? FromAddNewCostPage { get; set; }

    // Inject
    [Inject] public AppData AppData { get; set; }
    [Inject] public IExcelWizardService ExcelWizardService { get; set; }
    [Inject] public INotificationService NotificationService { get; set; }
    [Inject] public NavigationManager NavigationManager { get; set; }
    [Inject] public ICoreAPIs CoreAPIs { get; set; }

    [CascadingParameter]
    private Task<AuthenticationState> AuthenticationStateTask { get; set; }

    // Life Cycles
    protected override async Task OnInitializedAsync(CancellationToken cancellationToken)
    {
        IsLoading = true;

        try
        {
            PaySlipData = await HttpClient.PaySlip().GetCurrentUserPaySlipData(1402, "مرداد", cancellationToken: cancellationToken);
        }
        finally
        {
            IsLoading = false;
        }

        await base.OnInitializedAsync(cancellationToken);
    }

    protected override Task OnParametersSetAsync()
    {
        return base.OnParametersSetAsync();
    }


    // Methods

}