using ATA.HR.Client.Web.APIs.Enums;
using ATABit.Helper.Extensions;

namespace ATA.HR.Client.Web.APIs.Models.Response;

public class ClassRoomDto
{
    public long Id { get; set; }
    public string Title { get; set; }
    public long? ParentId { get; set; }
    public bool HasChildren { get; set; }

    public ClassType ClassType { get; set; }

    public List<long> Descendants { get; set; } = new();
    public List<long> DescendantsIncludingItself
    {
        get
        {
            var descendants = Descendants.SerializeToJson()?.DeserializeToModel<List<long>>();

            descendants?.Add(Id);

            return Descendants;
        }
    }

    public long? ChildCount { get; set; }

    public string Hierarchy { get; set; }
    public List<long> TreeHierarchy => Hierarchy!.Split(",").Select(x => long.Parse(x)).ToList();

    public bool IsSelected { get; set; }

}