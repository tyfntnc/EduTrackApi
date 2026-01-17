namespace EduTrackApi.Domain.Entities;

public class ParentChild
{
    public string ParentId { get; set; } = null!;
    public string ChildId { get; set; } = null!;

    public User Parent { get; set; } = null!;
    public User Child { get; set; } = null!;
}