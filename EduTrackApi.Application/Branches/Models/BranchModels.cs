namespace EduTrackApi.Application.Branches.Models;

public sealed class BranchDto
{
    public string Id { get; init; } = default!;
    public string Name { get; init; } = default!;
}

public sealed class CategoryDto
{
    public string Id { get; init; } = default!;
    public string Name { get; init; } = default!;
}
