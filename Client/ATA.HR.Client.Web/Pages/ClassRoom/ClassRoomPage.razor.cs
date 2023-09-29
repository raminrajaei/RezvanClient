using ATA.HR.Client.Web.APIs;
using ATA.HR.Client.Web.Contracts;
using ATA.HR.Client.Web.Enums;
using ATA.HR.Client.Web.Models;
using ATA.HR.Shared;
using ATABit.Helper.Extensions;
using Bit.Http.Contracts;
using BlazorDownloadFile;
using ExcelWizard.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;
using ATA.HR.Client.Web.APIs.Enums;
using ATA.HR.Client.Web.APIs.Models.Response;
using Telerik.Blazor.Components;
using Telerik.Blazor.Extensions;
using GridSelectionMode = ATA.HR.Client.Web.Enums.GridSelectionMode;
using ATA.HR.Client.Web.APIs.Models.Request;
using ATABit.Helper.Utils;
using ATABit.Shared;
using ExcelWizard.Models;
using ExcelWizard.Models.EWExcel;
using Radzen;
using Microsoft.JSInterop;

namespace ATA.HR.Client.Web.Pages.ClassRoom;

[Authorize]
public partial class ClassRoomPage
{
    // Props
    private bool IsLoading { get; set; } = true;
    private bool IsSaving { get; set; }
    private OperationType PageOperationType { get; set; } = OperationType.Nothing;
    private int? ClassTypeFilter { get; set; }

    private TelerikTextBox _focusableClassNameRef;
    private List<ClassRoomDto> Classes { get; set; } = new();
    private IEnumerable<object> ExpandedItems { get; set; } = new List<ClassRoomDto>();
    private IEnumerable<object> SelectedItems { get; set; } = new List<object>();
    private List<SelectListItem> ClassTypesSource { get; set; } = EnumMapping.ToSelectListItems<ClassType>();

    private ClassRoomUpsertDto Class { get; set; } = new();

    // Inject
    [Inject] public AppData AppData { get; set; }
    [Inject] public IExcelWizardService ExcelWizardService { get; set; }
    [Inject] public INotificationService NotificationService { get; set; }
    [Inject] public NavigationManager NavigationManager { get; set; }
    [Inject] private IJSRuntime JsRuntime { get; set; }
    [Inject] public IRezvanAPIs APIs { get; set; }

    [CascadingParameter]
    private Task<AuthenticationState> AuthenticationStateTask { get; set; }

    // Life Cycles
    protected override async Task OnInitializedAsync(CancellationToken cancellationToken)
    {
        await LoadClasses(null, cancellationToken);

        await base.OnInitializedAsync(cancellationToken);
    }

    // Methods
    private async Task LoadClasses(int? classType = null, CancellationToken cancellationToken = default)
    {
        IsLoading = true;

        try
        {
            ApiResult<List<ClassRoomDto>> apiResult;

            if (classType.HasValue is false)
                apiResult = await APIs.GetClasses();
            else
                apiResult = await APIs.GetClasses(classType.Value);

            if (apiResult.IsSuccess)
            {
                Classes = apiResult.Data;
                ExpandedItems = Classes.Where(x => x.HasChildren).ToList();
            }
        }
        finally
        {
            IsLoading = false;

            StateHasChanged();
        }
    }

    private Task OnSelectClass(ClassRoomDto @class)
    {
        Classes.ForEach(c => c.IsSelected = c.Id == @class.Id);

        PageOperationType = OperationType.Edit;

        Class = new ClassRoomUpsertDto
        {
            Id = @class.Id,

            Title = @class.Title,

            ClassType = (int)@class.ClassType,

            ParentId = @class.ParentId
        };

        return FocusOnClassNameTextBoxAsync();
    }

    private Task AddRootClass()
    {
        PageOperationType = OperationType.Custom1; //Custom1 = AddingRoot

        Class = new ClassRoomUpsertDto { ParentId = null };

        return FocusOnClassNameTextBoxAsync();
    }

    private async Task OnClassSubmit()
    {
        IsSaving = true;

        try
        {
            await APIs.UpsertClass(Class);

            string message = "";

            if (PageOperationType is OperationType.Add or OperationType.Custom1)
                message = "کلاس با موفقیت ایجاد گردید";
            else if (PageOperationType == OperationType.Edit)
                message = "کلاس با موفقیت ویرایش گردید";

            NotificationService.Toast(NotificationType.Success, message);

            PageOperationType = OperationType.Nothing;

            await LoadClasses();
        }
        finally
        {
            IsSaving = false;

            StateHasChanged();
        }
    }

    private Task OnAddClass(ClassRoomDto parentClass)
    {
        PageOperationType = OperationType.Add;

        Class = new ClassRoomUpsertDto { ParentId = parentClass.Id, ClassType = (int)parentClass.ClassType };

        Console.WriteLine(Class.SerializeToJson());

        return FocusOnClassNameTextBoxAsync();
    }

    private async Task OnDeleteClass(ClassRoomDto @class)
    {
        IsLoading = true;

        try
        {
            await APIs.DeleteClass(@class.Id);

            await LoadClasses();

            NotificationService.Toast(NotificationType.Success, "حذف کلاس با موفقیت انجام شد");
        }
        finally
        {
            IsLoading = false;

            PageOperationType = OperationType.Nothing;

            StateHasChanged();
        }
    }

    private async Task FocusOnClassNameTextBoxAsync()
    {
        //workaround
        await Task.Delay(200);

        await _focusableClassNameRef.FocusAsync();
    }

    private async Task ApplyFilters()
    {
        await LoadClasses(ClassTypeFilter);
    }
}