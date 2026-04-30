namespace EduTrackApi.Application.Notifications.Models;

public sealed class NotificationDto
{
    public string Id { get; init; } = default!;
    public string Type { get; init; } = default!;
    public string Title { get; init; } = default!;
    public string Message { get; init; } = default!;
    public string Timestamp { get; init; } = default!;
    public bool IsRead { get; init; }
    public string? SenderRole { get; init; }
}

public sealed class SendNotificationRequest
{
    public string Type { get; init; } = default!;
    public string Title { get; init; } = default!;
    public string Message { get; init; } = default!;
    public TargetAudienceDto TargetAudience { get; init; } = default!;
}

public sealed class TargetAudienceDto
{
    public string Scope { get; init; } = default!;
    public string? SchoolId { get; init; }
    public string? CourseId { get; init; }
    public List<string>? UserIds { get; init; }
    public List<string>? Roles { get; init; }
}

public sealed class PaymentReminderRequest
{
    public string PaymentId { get; init; } = default!;
    public string StudentId { get; init; } = default!;
}
