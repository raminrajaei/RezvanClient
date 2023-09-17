using System.ComponentModel.DataAnnotations.Schema;

namespace ATA.HR.Shared.Dtos.Document;

[ComplexType]
public class PersonnelDocumentsStatistics
{
    public int TotalUploadedDocumentsNo { get; set; }

    public int TotalUsersHaveDocs { get; set; }

    public int TotalDocsUploadedToday { get; set; }

    public int TotalDocsUploadedLast7Days { get; set; }
}