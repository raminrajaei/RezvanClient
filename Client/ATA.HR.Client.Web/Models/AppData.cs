using ATA.HR.Client.Web.APIs.Insurance.Models.Responses;
using ATA.HR.Shared.Dtos.Workflow;

namespace ATA.HR.Client.Web.Models
{
    public class AppData
    {
        public List<CostDto> CostsToPrint { get; set; }
        public (string MainInsuredName, string MainInsuredPersonnelCode, string MainInsuredSignatureUrl) CostsPrintInfo { get; set; }

        public string? SearchTermAllCostsQueryAllCostsPage { get; set; }

        public DashboardFilterArgs? DashboardFilterArgs { get; set; }

        //private List<LocationReadDto>? _locations = new();
        //public List<LocationReadDto> Locations
        //{
        //    get => _locations ?? new List<LocationReadDto>();
        //    set
        //    {
        //        _locations = value;
        //        NotifyDataChanged();
        //    }
        //}

        public string? UserProfileImageUrl { get; set; }


        public event Action? OnChange;
        private void NotifyDataChanged() => OnChange?.Invoke();
    }
}