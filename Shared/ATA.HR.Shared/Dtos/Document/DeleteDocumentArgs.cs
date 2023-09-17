using System.ComponentModel.DataAnnotations.Schema;

namespace ATA.HR.Shared.Dtos.Document;

[ComplexType]
public class DeleteDocumentArgs
{
    public int DocId { get; set; }
}