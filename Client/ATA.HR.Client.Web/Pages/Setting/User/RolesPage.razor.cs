using ATA.HR.Client.Web.Contracts;
using ATA.HR.Client.Web.Enums;
using ATA.HR.Shared;
using ATA.HR.Shared.Dtos;
using Bit.Http.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Net.Http;
using System.Threading;
using Telerik.Blazor.Components;
using Telerik.Blazor.Extensions;

namespace ATA.HR.Client.Web.Pages.Setting.User;

[Authorize(Claims.Settings_Permissions_View)]
public partial class RolesPage
{
    // Props
    private bool IsLoading { get; set; } = true;
    public OperationType PageOperationType { get; set; } = OperationType.Filter;
    private List<string> AllClaims { get; set; } = new();
    private IEnumerable<string> _selectedClaims = new string[] { };

    private List<RoleReadDto> RolesData { get; set; } = new();
    private TelerikGrid<RoleReadDto> RoleGridRef { get; set; }

    public bool ResetPagination { get; set; }

    public RoleReadDto Role { get; set; } = new();

    // Injects
    [Inject] private INotificationService NotificationService { get; set; }
    [Inject] private IJSRuntime JsRuntime { get; set; }

    // Life cycles
    protected override async Task OnInitializedAsync(CancellationToken cancellationToken)
    {
        IsLoading = true;

        try
        {
            AllClaims = await HttpClient.Role().GetAllClaims(cancellationToken: cancellationToken);
        }
        finally
        {
            IsLoading = false;

            StateHasChanged();
        }

        await base.OnInitializedAsync(cancellationToken);
    }

    // Methods

    private void ChangeToFilterMode()
    {
        PageOperationType = OperationType.Filter;
    }

    protected async Task OnReadHandler(GridReadEventArgs args)
    {
        await LoadData(args);
    }

    // Make the grid call OnRead to request data again
    // As part of our 3.0.1 release we introduced the Rebind method to the component reference. This would make the rest of the approaches in this article obsolete.
    private Task RebindGrid(bool resetPagination)
    {
        ResetPagination = resetPagination;

        return RoleGridRef.SetState(RoleGridRef.GetState());
    }

    public async Task LoadData(GridReadEventArgs args)
    {
        IsLoading = true;

        try
        {
            if (ResetPagination)
            {
                args.Request.Skip = 0;
                args.Request.Page = 1;
                RoleGridRef.Page = 1;
            }

            var oDataQuery = args.Request.ToODataString();

            var context = new ODataContext { Query = oDataQuery };

            var cancellationToken = CancellationToken.None;

            RolesData = await HttpClient.Role().GetAllRoles(context, cancellationToken);

            args.Data = RolesData;

            args.Total = (int)(context.TotalCount ?? 0);
        }

        finally
        {
            IsLoading = false;

            ResetPagination = false;

            StateHasChanged();
        }
    }

    private void ChangeToManageRolePermissionsMode(string roleName)
    {
        Role = RolesData.Single(u => u.RoleName == roleName);

        _selectedClaims = Role.Claims.ToArray();

        PageOperationType = OperationType.Custom1;
    }

    private async Task OnClaimChangesSubmit()
    {
        IsLoading = true;

        try
        {
            await HttpClient.Role().SetRoleClaims(new SetRoleClaimsArgs
            {
                RoleName = Role.RoleName,
                NewClaims = _selectedClaims.ToList()
            });

            PageOperationType = OperationType.Filter;

            NotificationService.Toast(NotificationType.Success, "با موفقیت ذخیره شد");

            await RebindGrid(false);
        }
        finally
        {
            IsLoading = false;

            StateHasChanged();
        }
    }
}