namespace EduTrackApi.Domain.Entities;

public class CourseStudent
{
    public string CourseId { get; set; } = null!;
    public string StudentId { get; set; } = null!;

    public Course Course { get; set; } = null!;
    public User Student { get; set; } = null!;
}