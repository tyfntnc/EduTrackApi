using EduTrackApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace EduTrackApi.Application.Common.Interfaces;

public interface IEduTrackDbContext
{
    DbSet<UserRole> UserRoles { get; }
    DbSet<NotificationType> NotificationTypes { get; }
    DbSet<PaymentStatus> PaymentStatuses { get; }
    DbSet<School> Schools { get; }
    DbSet<Branch> Branches { get; }
    DbSet<Category> Categories { get; }
    DbSet<User> Users { get; }
    DbSet<ParentChild> ParentChildren { get; }
    DbSet<Badge> Badges { get; }
    DbSet<UserBadge> UserBadges { get; }
    DbSet<Course> Courses { get; }
    DbSet<CourseSchedule> CourseSchedules { get; }
    DbSet<CourseStudent> CourseStudents { get; }
    DbSet<Payment> Payments { get; }
    DbSet<Notification> Notifications { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}