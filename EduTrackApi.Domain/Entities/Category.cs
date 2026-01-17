using System.Collections.Generic;

namespace EduTrackApi.Domain.Entities;

public class Category
{
    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;

    public ICollection<Course> Courses { get; set; } = new List<Course>();
}