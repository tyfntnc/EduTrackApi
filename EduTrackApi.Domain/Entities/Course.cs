using System.Collections.Generic;

namespace EduTrackApi.Domain.Entities;

public class Course
{
    public string Id { get; set; } = null!;
    public string SchoolId { get; set; } = null!;
    public string BranchId { get; set; } = null!;
    public string CategoryId { get; set; } = null!;
    public string TeacherId { get; set; } = null!;

    public string Title { get; set; } = null!;
    public string? Location { get; set; }
    public string? Address { get; set; }
    public string? InstructorNotes { get; set; }

    public School School { get; set; } = null!;
    public Branch Branch { get; set; } = null!;
    public Category Category { get; set; } = null!;
    public User Teacher { get; set; } = null!;

    public ICollection<CourseSchedule> Schedules { get; set; } = new List<CourseSchedule>();
    public ICollection<CourseStudent> CourseStudents { get; set; } = new List<CourseStudent>();
    public ICollection<Payment> Payments { get; set; } = new List<Payment>();
}