using ATA.HR.Client.Shared;
using ATA.HR.Client.Web.Extensions;
using ATA.HR.Client.Web.Pages;
using ATA.HR.Shared;
using ATABit.Helper.Extensions;
using Bit.Core.Exceptions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.ComponentModel.DataAnnotations;

namespace ATA.HR.Client.Web.Shared;

public partial class SideBar
{
    public List<string> ByPassUnitsForViewMenuItems = new() { "فناوری اطلاعات و ارتباطات", "منابع انسانی" };

    // Props
    public List<MenuItem> MenuItems { get; set; } = new()
    {
        // Rezvan: Child Menu
        new()
        {
            Title = "کودک",
            URL = PageUrls.ChildRootPath,
            ActiveIconUrl = IconUrls.Child,
            InactiveIconUrl = IconUrls.Child,
            SubMenuItems = new()
            {
                new(PageUrls.ChildrenPage, "لیست کودکان"),
                new(PageUrls.AddChildFormPage(), "ثبت نام کودک جدید")
            }
        },
        
        // Rezvan: Student Menu
        new()
        {
            Title = "بزرگسال",
            URL = PageUrls.AdultRootPath,
            ActiveIconUrl = IconUrls.Adult,
            InactiveIconUrl = IconUrls.Adult,
            SubMenuItems = new()
            {
                new(PageUrls.AdultsPage, "لیست بزرگسالان"),
                new(PageUrls.AdultsPage, "ثبت نام بزرگسال جدید")
            }
        },
        
        // Rezvan: Teacher Menu
        new()
        {
            Title = "مدرس",
            URL = PageUrls.TeachersPage,
            ActiveIconUrl = IconUrls.Teacher,
            InactiveIconUrl = IconUrls.Teacher
        },
        
        // Rezvan: Class Menu
        new()
        {
            Title = "کلاس",
            URL = PageUrls.TeachersPage,
            ActiveIconUrl = IconUrls.Class,
            InactiveIconUrl = IconUrls.Class
        },

        // Settings
        new()
        {
            Title = "تنظیمات",
            URL = PageUrls.SettingsRootPath,
            ActiveIconUrl = IconUrls.Settings,
            InactiveIconUrl = IconUrls.Settings,
            SubMenuItems = new()
            {
                new("", "مدیریت کاربران", true, Claims.Settings_Permissions_View),
                new(PageUrls.UsersPage, "کاربران دارای نقش", false, Claims.Settings_Permissions_View),
                new(PageUrls.RolesPage, "نقش‌های سامانه", false, Claims.Settings_Permissions_View),
                new("", "مدیریت امضا", true),
                new(PageUrls.SignaturesPage, "امضای پرسنل", policy: Claims.GatherSignaturesPermission),
                new("", "گردش کار", true),
                new(PageUrls.ContractsDirectManagersPage, "مدیران تایید کننده قراردادها", false, Claims.DirectManagersListView),
                new(PageUrls.WorkHoursDirectManagersPage, "مدیران تایید کننده کارکردها", false, Claims.DirectManagersListView),
                new("", "پیامک", true, Claims.AppSMSes_ViewAll),
                new(PageUrls.AllAppSMSesPage, "همه‌ی پیامک‌های ارسالی", false, Claims.AppSMSes_ViewAll),
                new(PageUrls.NotifyManagersWorkHourJobSettingsPage, "تنظیم پیامک هوشمند مدیران تایید کارکردها", false, Claims.AppSMSes_ViewAll)
            },
            Policies = new List<string>
            {
                Claims.Settings_Permissions_View, 
                Claims.DirectManagersListView, 
                Claims.AppSMSes_ViewAll,
                Claims.GatherSignaturesPermission
            }
        }
    };
    public List<SubMenuItem> SubMenuItems { get; set; } = new();
    public bool IsVisibleSubMenuPanel { get; set; }
    public List<string> UserClaims { get; set; } = new();
    public string? UserUnit { get; set; }

    [CascadingParameter]
    private Task<AuthenticationState> AuthenticationStateTask { get; set; }

    // Life-cycles
    protected override async Task OnInitializedAsync()
    {
        var authState = await AuthenticationStateTask;

        if (authState.User.Identity is null)
            return;

        if (authState.User.Identity.IsAuthenticated)
        {
            UserUnit = await AuthenticationStateTask.GetUserUnit();

            Console.WriteLine($"User Unit: {UserUnit}");

            UserClaims = await AuthenticationStateTask.GetUserClaims();
        }

        IsVisibleSubMenuPanel = false;

        await base.OnInitializedAsync();
    }

    protected override void OnParametersSet()
    {
        foreach (var menuItem in MenuItems)
        {
            if (menuItem.HasSubMenu)
            {
                if (menuItem.SubMenuItems.Any(subMenuItem => subMenuItem.IsGroupHeader is false && subMenuItem.URL!.Contains(menuItem.URL!) is false))
                    throw new DomainLogicException($"{menuItem.Title} menu has children which their url don't contain the father one!. Please fix it");
            }
        }
    }

    private void OpenSubMenu(MenuItem menu)
    {
        if (menu.HasSubMenu)
        {
            SubMenuItems = menu.SubMenuItems;

            IsVisibleSubMenuPanel = true;
        }
    }

    private void CloseSubMenu()
    {
        IsVisibleSubMenuPanel = false;
    }
}

public class MenuItem
{
    public bool IsHome { get; set; }

    [Required]
    public string? URL { get; set; }
    public string? Href => HasSubMenu ? "javascript:function() { return false; }" : URL;

    [Required]
    public string? Title { get; set; }

    public string? ActiveIconUrl { get; set; }

    public string? InactiveIconUrl { get; set; }

    public bool HasSubMenu => SubMenuItems.Count > 0;

    public List<SubMenuItem> SubMenuItems { get; set; } = new();

    public List<string> Policies { get; set; } = new();

    public List<string> AllowedUnits { get; set; } = new();

    public bool HasAuthorizeView => Policies.Count > 0;
}

public class SubMenuItem
{
    public SubMenuItem(string? url, string? title, bool isGroupHeader = false, string? policy = null)
    {
        URL = url;
        Title = title;
        IsGroupHeader = isGroupHeader;
        Policy = policy;
    }

    public string? URL { get; set; }

    public string? Title { get; set; }

    public bool IsGroupHeader { get; set; }

    public string? Policy { get; set; }

    public bool HasAuthorizeView => Policy.IsNotNullOrEmpty();
}