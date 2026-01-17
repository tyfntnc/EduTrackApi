using System;

namespace EduTrackApi.Domain.Entities;

public class Payment
{
    public string Id { get; set; } = null!;
    public string StudentId { get; set; } = null!;
    public string CourseId { get; set; } = null!;

    public decimal Amount { get; set; }
    public DateTime DueDate { get; set; }

    public short StatusId { get; set; }
    public DateTime? PaidAt { get; set; }
    public string? Method { get; set; }

    public User Student { get; set; } = null!;
    public Course Course { get; set; } = null!;
    public PaymentStatus Status { get; set; } = null!;
}