using EduTrackApi.Application.Common.Models;
using EduTrackApi.Application.Payments.Models;
using Microsoft.AspNetCore.Mvc;

namespace EduTrackApi.Api.Controllers;

[ApiController]
[Route("v1/payments")]
public sealed class PaymentsController : ControllerBase
{
    private static readonly List<PaymentDto> MockPayments =
    [
        new() { Id = "pay1", StudentId = "u2", StudentName = "Mehmet Kaya", CourseId = "crs1", CourseTitle = "U19 Football Elite", Amount = 1500, DueDate = "2026-05-01", Status = "Overdue", Method = null, PaidAt = null },
        new() { Id = "pay2", StudentId = "u2", StudentName = "Mehmet Kaya", CourseId = "crs2", CourseTitle = "Mathematics Advanced Level", Amount = 1200, DueDate = "2026-06-15", Status = "Paid", Method = "Credit Card", PaidAt = "2026-06-10" },
        new() { Id = "pay3", StudentId = "u2", StudentName = "Mehmet Kaya", CourseId = "crs1", CourseTitle = "U19 Football Elite", Amount = 1500, DueDate = "2026-07-01", Status = "Pending", Method = "Manual", PaidAt = null },
        new() { Id = "pay4", StudentId = "u9", StudentName = "Ali Vural", CourseId = "crs1", CourseTitle = "U19 Football Elite", Amount = 1500, DueDate = "2026-06-01", Status = "Paid", Method = "Manual", PaidAt = "2026-05-28" }
    ];

    [HttpGet("my")]
    public IActionResult GetMy([FromQuery] string? status)
    {
        var payments = MockPayments.Where(p => p.StudentId == "u2").ToList();
        if (!string.IsNullOrEmpty(status))
            payments = payments.Where(p => p.Status == status).ToList();
        return Ok(ApiResponse<List<PaymentDto>>.Ok(payments));
    }

    [HttpGet("school/{schoolId}")]
    public IActionResult GetBySchool(string schoolId, [FromQuery] string? status, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var payments = MockPayments.ToList();
        if (!string.IsNullOrEmpty(status))
            payments = payments.Where(p => p.Status == status).ToList();
        return Ok(ApiResponse<List<PaymentDto>>.Ok(payments, new MetaData { Page = page, PageSize = pageSize, Total = payments.Count }));
    }

    [HttpPost]
    public IActionResult Create([FromBody] CreatePaymentRequest request)
    {
        var payment = new PaymentDto
        {
            Id = $"pay-{Guid.NewGuid():N}",
            StudentId = request.StudentId,
            CourseId = request.CourseId,
            Amount = request.Amount,
            DueDate = request.DueDate,
            Status = "Pending",
            CreatedAt = DateTime.UtcNow.ToString("o")
        };
        return StatusCode(201, ApiResponse<PaymentDto>.Ok(payment));
    }

    [HttpPatch("{paymentId}/status")]
    public IActionResult UpdateStatus(string paymentId, [FromBody] UpdatePaymentStatusRequest request)
    {
        return Ok(ApiResponse.Ok(new { id = paymentId, status = request.Status, method = request.Method, paidAt = request.PaidAt }));
    }

    [HttpPost("{paymentId}/pay")]
    public IActionResult PayOnline(string paymentId, [FromBody] PayOnlineRequest request)
    {
        return Ok(ApiResponse.Ok(new { paymentId, transactionId = "txn-abc-123", status = "Paid", paidAt = DateTime.UtcNow.ToString("o") }));
    }

    [HttpPost("remind-overdue")]
    public IActionResult RemindOverdue([FromBody] RemindOverdueRequest request)
    {
        return Ok(ApiResponse.Ok(new { notifiedCount = 3, message = "Payment reminder sent to 3 users." }));
    }
}
