using EduTrackApi.Application.Common.Interfaces;
using EduTrackApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace EduTrackApi.Infrastructure.Persistence;

public class EduTrackDbContext : DbContext, IEduTrackDbContext
{
    public EduTrackDbContext(DbContextOptions<EduTrackDbContext> options) : base(options)
    {
    }

    public DbSet<UserRole> UserRoles => Set<UserRole>();
    public DbSet<NotificationType> NotificationTypes => Set<NotificationType>();
    public DbSet<PaymentStatus> PaymentStatuses => Set<PaymentStatus>();
    public DbSet<School> Schools => Set<School>();
    public DbSet<Branch> Branches => Set<Branch>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<User> Users => Set<User>();
    public DbSet<ParentChild> ParentChildren => Set<ParentChild>();
    public DbSet<Badge> Badges => Set<Badge>();
    public DbSet<UserBadge> UserBadges => Set<UserBadge>();
    public DbSet<Course> Courses => Set<Course>();
    public DbSet<CourseSchedule> CourseSchedules => Set<CourseSchedule>();
    public DbSet<CourseStudent> CourseStudents => Set<CourseStudent>();
    public DbSet<Payment> Payments => Set<Payment>();
    public DbSet<Notification> Notifications => Set<Notification>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("edutrack");

        // UserRole
        modelBuilder.Entity<UserRole>(entity =>
        {
            entity.ToTable("user_roles");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Code).HasColumnName("code").HasMaxLength(50).IsRequired();
            entity.Property(e => e.Name).HasColumnName("name").HasMaxLength(100).IsRequired();
        });

        // NotificationType
        modelBuilder.Entity<NotificationType>(entity =>
        {
            entity.ToTable("notification_types");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Code).HasColumnName("code").HasMaxLength(50).IsRequired();
            entity.Property(e => e.Name).HasColumnName("name").HasMaxLength(100).IsRequired();
        });

        // PaymentStatus
        modelBuilder.Entity<PaymentStatus>(entity =>
        {
            entity.ToTable("payment_statuses");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Code).HasColumnName("code").HasMaxLength(50).IsRequired();
            entity.Property(e => e.Name).HasColumnName("name").HasMaxLength(100).IsRequired();
        });

        // School
        modelBuilder.Entity<School>(entity =>
        {
            entity.ToTable("schools");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id").HasMaxLength(50);
            entity.Property(e => e.Name).HasColumnName("name").HasMaxLength(255).IsRequired();
        });

        // Branch
        modelBuilder.Entity<Branch>(entity =>
        {
            entity.ToTable("branches");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id").HasMaxLength(50);
            entity.Property(e => e.Name).HasColumnName("name").HasMaxLength(255).IsRequired();
        });

        // Category
        modelBuilder.Entity<Category>(entity =>
        {
            entity.ToTable("categories");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id").HasMaxLength(50);
            entity.Property(e => e.Name).HasColumnName("name").HasMaxLength(255).IsRequired();
        });

        // User
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("users");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id").HasMaxLength(50);
            entity.Property(e => e.Name).HasColumnName("name").HasMaxLength(255).IsRequired();
            entity.Property(e => e.Email).HasColumnName("email").HasMaxLength(255).IsRequired();
            entity.HasIndex(e => e.Email).IsUnique();

            entity.Property(e => e.Avatar).HasColumnName("avatar");
            entity.Property(e => e.Bio).HasColumnName("bio");
            entity.Property(e => e.PhoneNumber).HasColumnName("phone_number").HasMaxLength(50);
            entity.Property(e => e.Gender).HasColumnName("gender").HasMaxLength(20);
            entity.Property(e => e.BirthDate).HasColumnName("birth_date");
            entity.Property(e => e.Address).HasColumnName("address");

            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.SchoolId).HasColumnName("school_id").HasMaxLength(50);

            entity.HasOne(e => e.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(e => e.RoleId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.School)
                .WithMany(s => s.Users)
                .HasForeignKey(e => e.SchoolId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        // ParentChild
        modelBuilder.Entity<ParentChild>(entity =>
        {
            entity.ToTable("parent_children");
            entity.HasKey(e => new { e.ParentId, e.ChildId });

            entity.Property(e => e.ParentId).HasColumnName("parent_id").HasMaxLength(50);
            entity.Property(e => e.ChildId).HasColumnName("child_id").HasMaxLength(50);

            entity.HasOne(e => e.Parent)
                .WithMany(u => u.ParentChildrenAsParent)
                .HasForeignKey(e => e.ParentId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Child)
                .WithMany(u => u.ParentChildrenAsChild)
                .HasForeignKey(e => e.ChildId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Badge
        modelBuilder.Entity<Badge>(entity =>
        {
            entity.ToTable("badges");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id").HasMaxLength(50);
            entity.Property(e => e.Name).HasColumnName("name").HasMaxLength(255).IsRequired();
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Icon).HasColumnName("icon").HasMaxLength(20).IsRequired();
            entity.Property(e => e.Color).HasColumnName("color").HasMaxLength(100).IsRequired();
        });

        // UserBadge
        modelBuilder.Entity<UserBadge>(entity =>
        {
            entity.ToTable("user_badges");
            entity.HasKey(e => new { e.UserId, e.BadgeId });

            entity.Property(e => e.UserId).HasColumnName("user_id").HasMaxLength(50);
            entity.Property(e => e.BadgeId).HasColumnName("badge_id").HasMaxLength(50);
            entity.Property(e => e.AwardedAt).HasColumnName("awarded_at");

            entity.HasOne(e => e.User)
                .WithMany(u => u.UserBadges)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Badge)
                .WithMany(b => b.UserBadges)
                .HasForeignKey(e => e.BadgeId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Course
        modelBuilder.Entity<Course>(entity =>
        {
            entity.ToTable("courses");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id").HasMaxLength(50);

            entity.Property(e => e.SchoolId).HasColumnName("school_id").HasMaxLength(50);
            entity.Property(e => e.BranchId).HasColumnName("branch_id").HasMaxLength(50);
            entity.Property(e => e.CategoryId).HasColumnName("category_id").HasMaxLength(50);
            entity.Property(e => e.TeacherId).HasColumnName("teacher_id").HasMaxLength(50);

            entity.Property(e => e.Title).HasColumnName("title").HasMaxLength(255).IsRequired();
            entity.Property(e => e.Location).HasColumnName("location").HasMaxLength(255);
            entity.Property(e => e.Address).HasColumnName("address");
            entity.Property(e => e.InstructorNotes).HasColumnName("instructor_notes");

            entity.HasOne(e => e.School)
                .WithMany(s => s.Courses)
                .HasForeignKey(e => e.SchoolId);

            entity.HasOne(e => e.Branch)
                .WithMany(b => b.Courses)
                .HasForeignKey(e => e.BranchId);

            entity.HasOne(e => e.Category)
                .WithMany(c => c.Courses)
                .HasForeignKey(e => e.CategoryId);

            entity.HasOne(e => e.Teacher)
                .WithMany(u => u.CoursesAsTeacher)
                .HasForeignKey(e => e.TeacherId);
        });

        // CourseSchedule
        modelBuilder.Entity<CourseSchedule>(entity =>
        {
            entity.ToTable("course_schedules");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CourseId).HasColumnName("course_id").HasMaxLength(50);
            entity.Property(e => e.DayOfWeek).HasColumnName("day_of_week");
            entity.Property(e => e.StartTime).HasColumnName("start_time");
            entity.Property(e => e.EndTime).HasColumnName("end_time");

            entity.HasOne(e => e.Course)
                .WithMany(c => c.Schedules)
                .HasForeignKey(e => e.CourseId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // CourseStudent
        modelBuilder.Entity<CourseStudent>(entity =>
        {
            entity.ToTable("course_students");
            entity.HasKey(e => new { e.CourseId, e.StudentId });

            entity.Property(e => e.CourseId).HasColumnName("course_id").HasMaxLength(50);
            entity.Property(e => e.StudentId).HasColumnName("student_id").HasMaxLength(50);

            entity.HasOne(e => e.Course)
                .WithMany(c => c.CourseStudents)
                .HasForeignKey(e => e.CourseId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Student)
                .WithMany(u => u.CourseStudents)
                .HasForeignKey(e => e.StudentId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Payment
        modelBuilder.Entity<Payment>(entity =>
        {
            entity.ToTable("payments");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id).HasColumnName("id").HasMaxLength(50);
            entity.Property(e => e.StudentId).HasColumnName("student_id").HasMaxLength(50);
            entity.Property(e => e.CourseId).HasColumnName("course_id").HasMaxLength(50);
            entity.Property(e => e.Amount).HasColumnName("amount").HasColumnType("numeric(12,2)");
            entity.Property(e => e.DueDate).HasColumnName("due_date");
            entity.Property(e => e.StatusId).HasColumnName("status_id");
            entity.Property(e => e.PaidAt).HasColumnName("paid_at");
            entity.Property(e => e.Method).HasColumnName("method").HasMaxLength(100);

            entity.HasOne(e => e.Student)
                .WithMany(u => u.Payments)
                .HasForeignKey(e => e.StudentId);

            entity.HasOne(e => e.Course)
                .WithMany(c => c.Payments)
                .HasForeignKey(e => e.CourseId);

            entity.HasOne(e => e.Status)
                .WithMany(s => s.Payments)
                .HasForeignKey(e => e.StatusId);

            entity.HasIndex(e => e.StudentId).HasDatabaseName("ix_payments_student");
            entity.HasIndex(e => e.CourseId).HasDatabaseName("ix_payments_course");
        });

        // Notification
        modelBuilder.Entity<Notification>(entity =>
        {
            entity.ToTable("notifications");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id).HasColumnName("id").HasMaxLength(50);
            entity.Property(e => e.TypeId).HasColumnName("type_id");
            entity.Property(e => e.Title).HasColumnName("title").HasMaxLength(255).IsRequired();
            entity.Property(e => e.Message).HasColumnName("message");
            entity.Property(e => e.Timestamp).HasColumnName("timestamp");
            entity.Property(e => e.IsRead).HasColumnName("is_read");
            entity.Property(e => e.SenderRoleId).HasColumnName("sender_role_id");

            entity.HasOne(e => e.Type)
                .WithMany(t => t.Notifications)
                .HasForeignKey(e => e.TypeId);

            entity.HasOne(e => e.SenderRole)
                .WithMany(r => r.NotificationsAsSenderRole)
                .HasForeignKey(e => e.SenderRoleId);

            entity.HasIndex(e => e.TypeId).HasDatabaseName("ix_notifications_type");
        });

        // -----------------------
        // SEED DATA (HasData)
        // -----------------------

        // UserRoles
        modelBuilder.Entity<UserRole>().HasData(
            new UserRole { Id = 1, Code = "SYSTEM_ADMIN", Name = "Sistem Yöneticisi" },
            new UserRole { Id = 2, Code = "SCHOOL_ADMIN", Name = "Okul Yöneticisi" },
            new UserRole { Id = 3, Code = "TEACHER", Name = "Eğitmen" },
            new UserRole { Id = 4, Code = "PARENT", Name = "Veli" },
            new UserRole { Id = 5, Code = "STUDENT", Name = "Öğrenci" }
        );

        // NotificationTypes
        modelBuilder.Entity<NotificationType>().HasData(
            new NotificationType { Id = 1, Code = "ANNOUNCEMENT", Name = "Duyuru" },
            new NotificationType { Id = 2, Code = "UPCOMING_CLASS", Name = "Yaklaşan Ders" },
            new NotificationType { Id = 3, Code = "ATTENDANCE_UPDATE", Name = "Yoklama Güncellemesi" }
        );

        // PaymentStatuses
        modelBuilder.Entity<PaymentStatus>().HasData(
            new PaymentStatus { Id = 1, Code = "PENDING", Name = "Beklemede" },
            new PaymentStatus { Id = 2, Code = "PAID", Name = "Ödendi" },
            new PaymentStatus { Id = 3, Code = "OVERDUE", Name = "Gecikmiş" }
        );

        // Branches (INITIAL_BRANCHES)
        modelBuilder.Entity<Branch>().HasData(
            new Branch { Id = "b1", Name = "Futbol" },
            new Branch { Id = "b2", Name = "Basketbol" },
            new Branch { Id = "b3", Name = "Matematik" },
            new Branch { Id = "b4", Name = "Voleybol" },
            new Branch { Id = "b5", Name = "Yüzme" }
        );

        // Categories (INITIAL_CATEGORIES)
        modelBuilder.Entity<Category>().HasData(
            new Category { Id = "c1", Name = "U19" },
            new Category { Id = "c2", Name = "U15" },
            new Category { Id = "c3", Name = "Özel Ders" },
            new Category { Id = "c4", Name = "Grup" }
        );

        // Schools (constants.tsx'de sadece id kullanılıyor, basic seed)
        modelBuilder.Entity<School>().HasData(
            new School { Id = "school-a", Name = "Okul A" },
            new School { Id = "school-b", Name = "Okul B" }
        );

        // Badges (SYSTEM_BADGES)
        modelBuilder.Entity<Badge>().HasData(
            new Badge { Id = "bg1", Name = "7 Günlük Seri", Description = "Bir hafta boyunca her gün antrenmana katıldın. Muhteşem süreklilik!", Icon = "🔥", Color = "from-orange-400 to-rose-500" },
            new Badge { Id = "bg2", Name = "Erken Kuş", Description = "Sabah 09:00 öncesindeki antrenmanların yıldızı sensin.", Icon = "🌅", Color = "from-amber-300 to-orange-500" },
            new Badge { Id = "bg3", Name = "Çift Mesai", Description = "Aynı gün içerisinde iki farklı branşta eğitim alarak sınırları zorladın.", Icon = "⚡", Color = "from-indigo-400 to-purple-600" },
            new Badge { Id = "bg4", Name = "Sadık Sporcu", Description = "Bu ay toplamda 20 saati aşkın antrenman süresine ulaştın.", Icon = "🎯", Color = "from-emerald-400 to-teal-600" },
            new Badge { Id = "bg5", Name = "Gelişim Öncüsü", Description = "Eğitmen notlarında üst üste 3 kez \"Üstün Başarı\" yorumu aldın.", Icon = "🚀", Color = "from-blue-400 to-indigo-600" },
            new Badge { Id = "bg6", Name = "Sosyal Kelebek", Description = "Grup çalışmalarında en çok yardımlaşan öğrenci seçildin.", Icon = "🤝", Color = "from-pink-400 to-rose-500" }
        );

        // Users (MOCK_USERS)
        modelBuilder.Entity<User>().HasData(
            new User
            {
                Id = "admin",
                Name = "Zeynep Sistem",
                Email = "admin@edutrack.com",
                Avatar = "https://picsum.photos/seed/admin/200",
                PhoneNumber = "0555 111 22 33",
                Gender = "Kadın",
                BirthDate = new DateTime(1990, 5, 15, 0, 0, 0, DateTimeKind.Utc),
                Address = "İstanbul, Türkiye",
                RoleId = 1
            },
            new User
            {
                Id = "u4",
                Name = "Canan Sert",
                Email = "canan@okul-a.com",
                SchoolId = "school-a",
                Avatar = "https://picsum.photos/seed/u4/200",
                PhoneNumber = "0555 444 55 66",
                RoleId = 2
            },
            new User
            {
                Id = "u1",
                Name = "Ahmet Yılmaz",
                Email = "ahmet@okul-a.com",
                SchoolId = "school-a",
                Avatar = "https://picsum.photos/seed/u1/200",
                Bio = "15 yıllık profesyonel futbol antrenörlüğü tecrübesi.",
                PhoneNumber = "0532 123 45 67",
                RoleId = 3
            },
            new User
            {
                Id = "u3",
                Name = "Ayşe Demir",
                Email = "ayse@veli.com",
                Avatar = "https://picsum.photos/seed/u3/200",
                RoleId = 4
            },
            new User
            {
                Id = "u2",
                Name = "Mehmet Kaya",
                Email = "mehmet@okul-a.com",
                SchoolId = "school-a",
                Avatar = "https://picsum.photos/seed/u2/200",
                PhoneNumber = "0505 987 65 43",
                BirthDate = new DateTime(2008, 10, 12, 0, 0, 0, DateTimeKind.Utc),
                Gender = "Erkek",
                Address = "Ankara, Türkiye",
                RoleId = 5
            },
            new User
            {
                Id = "u9",
                Name = "Ali Vural",
                Email = "ali@okul-a.com",
                SchoolId = "school-a",
                Avatar = "https://picsum.photos/seed/u9/200",
                RoleId = 5
            },
            new User
            {
                Id = "u5",
                Name = "Bülent Arın",
                Email = "bulent@okul-b.com",
                SchoolId = "school-b",
                Avatar = "https://picsum.photos/seed/u5/200",
                RoleId = 2
            },
            new User
            {
                Id = "u7",
                Name = "Fatma Şahin",
                Email = "fatma@okul-a.com",
                SchoolId = "school-a",
                Avatar = "https://picsum.photos/seed/u7/200",
                Bio = "Matematik Olimpiyatları koordinatörü.",
                RoleId = 3
            },
            new User
            {
                Id = "u8",
                Name = "Murat Can",
                Email = "murat@okul-a.com",
                SchoolId = "school-a",
                Avatar = "https://picsum.photos/seed/u8/200",
                RoleId = 3
            }
        );

        // ParentChild (MOCK_USERS childIds/parentIds)
        modelBuilder.Entity<ParentChild>().HasData(
            new ParentChild { ParentId = "u3", ChildId = "u2" },
            new ParentChild { ParentId = "u3", ChildId = "u9" }
        );

        // UserBadges (MOCK_USERS badges)
        var seedDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        modelBuilder.Entity<UserBadge>().HasData(
            new UserBadge { UserId = "u2", BadgeId = "bg1", AwardedAt = seedDate },
            new UserBadge { UserId = "u2", BadgeId = "bg2", AwardedAt = seedDate },
            new UserBadge { UserId = "u2", BadgeId = "bg3", AwardedAt = seedDate },
            new UserBadge { UserId = "u2", BadgeId = "bg4", AwardedAt = seedDate },
            new UserBadge { UserId = "u2", BadgeId = "bg5", AwardedAt = seedDate },
            new UserBadge { UserId = "u9", BadgeId = "bg1", AwardedAt = seedDate }
        );

        // Courses (MOCK_COURSES) – schedule ve students ayrı tablolarda
        modelBuilder.Entity<Course>().HasData(
            new Course
            {
                Id = "crs1",
                SchoolId = "school-a",
                BranchId = "b1",
                CategoryId = "c1",
                TeacherId = "u1",
                Title = "U19 Futbol Elit",
                Location = "A Sahası",
                Address = "41.0082, 28.9784",
                InstructorNotes = "Lütfen antrenmana 15 dakika erken gelerek ısınma hareketlerine başlayın. Krampon kontrolü yapılacak."
            },
            new Course
            {
                Id = "crs2",
                SchoolId = "school-a",
                BranchId = "b3",
                CategoryId = "c3",
                TeacherId = "u7",
                Title = "Matematik İleri Seviye",
                Location = "Z-12 Laboratuvarı",
                Address = "Ankara, Çankaya",
                InstructorNotes = "Geçen haftaki problem setini yanınızda getirmeyi unutmayın. Türev konusuna giriş yapacağız."
            }
        );

        // CourseSchedules (MOCK_COURSES schedule)
        // EF seed için sabit gün kullanıyoruz (örnek: 1 = Pazartesi, 2 = Salı ...)
        modelBuilder.Entity<CourseSchedule>().HasData(
            new CourseSchedule { Id = 1, CourseId = "crs1", DayOfWeek = 1, StartTime = new TimeSpan(16, 0, 0), EndTime = new TimeSpan(18, 0, 0) },
            new CourseSchedule { Id = 2, CourseId = "crs1", DayOfWeek = 4, StartTime = new TimeSpan(16, 0, 0), EndTime = new TimeSpan(18, 0, 0) },
            new CourseSchedule { Id = 3, CourseId = "crs2", DayOfWeek = 1, StartTime = new TimeSpan(18, 30, 0), EndTime = new TimeSpan(20, 0, 0) },
            new CourseSchedule { Id = 4, CourseId = "crs2", DayOfWeek = 3, StartTime = new TimeSpan(18, 30, 0), EndTime = new TimeSpan(20, 0, 0) }
        );

        // CourseStudents (MOCK_COURSES studentIds)
        modelBuilder.Entity<CourseStudent>().HasData(
            new CourseStudent { CourseId = "crs1", StudentId = "u2" },
            new CourseStudent { CourseId = "crs1", StudentId = "u9" },
            new CourseStudent { CourseId = "crs2", StudentId = "u2" },
            new CourseStudent { CourseId = "crs2", StudentId = "u9" }
        );

        // Payments (MOCK_PAYMENTS)
        modelBuilder.Entity<Payment>().HasData(
            new Payment
            {
                Id = "pay1",
                StudentId = "u2",
                CourseId = "crs1",
                Amount = 1500m,
                DueDate = new DateTime(2024, 5, 1, 0, 0, 0, DateTimeKind.Utc),
                StatusId = 3 // OVERDUE
            },
            new Payment
            {
                Id = "pay2",
                StudentId = "u2",
                CourseId = "crs2",
                Amount = 1200m,
                DueDate = new DateTime(2024, 6, 15, 0, 0, 0, DateTimeKind.Utc),
                StatusId = 2, // PAID
                PaidAt = new DateTime(2024, 6, 10, 0, 0, 0, DateTimeKind.Utc),
                Method = "Credit Card"
            },
            new Payment
            {
                Id = "pay3",
                StudentId = "u2",
                CourseId = "crs1",
                Amount = 1500m,
                DueDate = new DateTime(2024, 7, 1, 0, 0, 0, DateTimeKind.Utc),
                StatusId = 1, // PENDING
                Method = "Manual"
            },
            new Payment
            {
                Id = "pay4",
                StudentId = "u9",
                CourseId = "crs1",
                Amount = 1500m,
                DueDate = new DateTime(2024, 6, 1, 0, 0, 0, DateTimeKind.Utc),
                StatusId = 2, // PAID
                PaidAt = new DateTime(2024, 5, 28, 0, 0, 0, DateTimeKind.Utc),
                Method = "Manual"
            }
        );

        // Notifications (MOCK_NOTIFICATIONS) – zamanları sabit
        modelBuilder.Entity<Notification>().HasData(
            new Notification
            {
                Id = "n3",
                TypeId = 1, // ANNOUNCEMENT
                Title = "Haftalık Program Güncellemesi",
                Message = "Antrenman saatlerinde düzenleme yapılmıştır.",
                Timestamp = new DateTime(2024, 4, 1, 10, 0, 0, DateTimeKind.Utc),
                IsRead = false,
                SenderRoleId = 2 // SCHOOL_ADMIN
            },
            new Notification
            {
                Id = "n1",
                TypeId = 2, // UPCOMING_CLASS
                Title = "Ders Başlıyor",
                Message = "U19 Futbol Elit Grubu dersi 15 dakika içinde başlayacak.",
                Timestamp = new DateTime(2024, 4, 1, 11, 30, 0, DateTimeKind.Utc),
                IsRead = false
            },
            new Notification
            {
                Id = "n2",
                TypeId = 3, // ATTENDANCE_UPDATE
                Title = "Yoklama Alındı",
                Message = "Mehmet Kaya bugünkü matematik dersinde \"Var\" olarak işaretlendi.",
                Timestamp = new DateTime(2024, 4, 1, 12, 0, 0, DateTimeKind.Utc),
                IsRead = true
            }
        );
    }
}