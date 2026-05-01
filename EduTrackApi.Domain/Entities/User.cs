using System;
using System.Collections.Generic;

namespace EduTrackApi.Domain.Entities;

public class User
{
    public string Id { get; set; } = null!;
    public short RoleId { get; set; }
    public string? SchoolId { get; set; }

    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? PasswordHash { get; set; }
    public string? Avatar { get; set; }
    public string? Bio { get; set; }

    public string? PhoneNumber { get; set; }
    public string? Gender { get; set; }
    public DateTime? BirthDate { get; set; }
    public string? Address { get; set; }

    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiry { get; set; }

    public UserRole Role { get; set; } = null!;
    public School? School { get; set; }

    public ICollection<ParentChild> ParentChildrenAsParent { get; set; } = new List<ParentChild>();
    public ICollection<ParentChild> ParentChildrenAsChild { get; set; } = new List<ParentChild>();

    public ICollection<UserBadge> UserBadges { get; set; } = new List<UserBadge>();

    public ICollection<Course> CoursesAsTeacher { get; set; } = new List<Course>();
    public ICollection<CourseStudent> CourseStudents { get; set; } = new List<CourseStudent>();

    public ICollection<Payment> Payments { get; set; } = new List<Payment>();
}