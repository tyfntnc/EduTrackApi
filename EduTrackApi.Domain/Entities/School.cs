using System.Collections.Generic;

namespace EduTrackApi.Domain.Entities;

public class School
{
    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;

    public ICollection<User> Users { get; set; } = new List<User>();
    public ICollection<Course> Courses { get; set; } = new List<Course>();
}