using Microsoft.AspNetCore.Components;

namespace ATA.HR.Client.Web.Components
{
    public partial class HeaderAddOrEdit
    {
        [Parameter]
        public RenderFragment ChildContent { get; set; }
    }
}