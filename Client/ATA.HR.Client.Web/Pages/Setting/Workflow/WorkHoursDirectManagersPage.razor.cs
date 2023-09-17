using ATA.HR.Client.Web.Enums;
using ATA.HR.Shared;
using ATA.HR.Shared.Dtos;
using Bit.Http.Contracts;
using Microsoft.AspNetCore.Authorization;
using System.Net.Http;
using System.Threading;
using Telerik.Blazor.Components;
using Telerik.Blazor.Extensions;

namespace ATA.HR.Client.Web.Pages.Setting.Workflow;

[Authorize(Claims.DirectManagersListView)]
public partial class WorkHoursDirectManagersPage
{
    // Props
    private bool IsLoading { get; set; } = true;
    private OperationType PageOperationType { get; set; } = OperationType.Filter;
    public List<DirectManagerReadDto> DirectManagers { get; set; } = new();

    private TelerikGrid<DirectManagerReadDto> DirectManagersGridRef { get; set; }

    // Life Cycles

    protected override async Task OnInitializedAsync(CancellationToken cancellationToken)
    {
        IsLoading = true;

        try
        {
            DirectManagers = await HttpClient.User().WorkHoursGetDirectManagers(cancellationToken: cancellationToken);
        }
        finally
        {
            IsLoading = false;

            StateHasChanged();
        }

        await base.OnInitializedAsync(cancellationToken);
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
        return DirectManagersGridRef.SetState(DirectManagersGridRef.GetState());
    }

    public async Task LoadData(GridReadEventArgs args)
    {
        IsLoading = true;

        try
        {
            var oDataQuery = args.Request.ToODataString();

            var context = new ODataContext { Query = oDataQuery };

            args.Data = await HttpClient.User().WorkHoursGetDirectManagers(context);

            args.Total = (int)(context.TotalCount ?? 0);
        }

        finally
        {
            IsLoading = false;

            StateHasChanged();
        }
    }
}