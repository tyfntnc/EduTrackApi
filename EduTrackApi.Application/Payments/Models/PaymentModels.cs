namespace EduTrackApi.Application.Payments.Models;

public sealed class PaymentDto
{
    public string Id { get; init; } = default!;
    public string StudentId { get; init; } = default!;
    public string? StudentName { get; init; }
    public string? CourseId { get; init; }
    public string? CourseTitle { get; init; }
    public decimal Amount { get; init; }
    public string DueDate { get; init; } = default!;
    public string Status { get; init; } = default!;
    public string? Method { get; init; }
    public string? PaidAt { get; init; }
    public string? TransactionId { get; init; }
    public string? CreatedAt { get; init; }
}

public sealed class CreatePaymentRequest
{
    public string StudentId { get; init; } = default!;
    public string CourseId { get; init; } = default!;
    public decimal Amount { get; init; }
    public string DueDate { get; init; } = default!;
}

public sealed class UpdatePaymentStatusRequest
{
    public string Status { get; init; } = default!;
    public string? Method { get; init; }
    public string? PaidAt { get; init; }
}

public sealed class PayOnlineRequest
{
    public string Method { get; init; } = default!;
    public string CardToken { get; init; } = default!;
}

public sealed class RemindOverdueRequest
{
    public string SchoolId { get; init; } = default!;
}
