using ClosedXML.Report.Utils;
using System.ComponentModel.DataAnnotations.Schema;

namespace ATA.HR.Shared.Dtos;

[ComplexType]
public class PersonnelToManageDocumentFilterArgs
{
    public string? SearchTerm { get; set; }

    public string? Unit { get; set; }

    public string? WorkLocation { get; set; }

    public string? UserStatusSelectedValue { get; set; }
    public bool? IsUserDismissed => UserStatusSelectedValue.IsNullOrWhiteSpace() ? null : UserStatusSelectedValue == "dismissed";
}