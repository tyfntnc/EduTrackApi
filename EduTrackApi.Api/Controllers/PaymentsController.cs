using EduTrackApi.Application.Common.Interfaces;
using EduTrackApi.Application.Common.Models;
using EduTrackApi.Application.Payments.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace EduTrackApi.Api.Controllers;

[Authorize]
[ApiController]
[Route("v1/payments")]
public sealed class PaymentsController : ControllerBase
{
    private readonly IEduTrackDbContext _db;

    public PaymentsController(IEduTrackDbContext db)
    {
        _db = db;
    }

    private string GetCurrentUserId() =>
        User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub") ?? "";

    [HttpGet("my")]
    public async Task<IActionResult> GetMy([FromQuery] string? status)
    {
        var userId = GetCurrentUserId();
        var query = _db.Payments
            .Include(p => p.Student).Include(p => p.Course).Include(p => p.Status)
            .Where(p => p.StudentId == userId);

        if (!string.IsNullOrEmpty(status))
            query = query.Where(p => p.Status!.Code == status || p.Status.Name == status);

        var payments = await query.Select(p => MapToDto(p)).ToListAsync();
        return Ok(ApiResponse<List<PaymentDto>>.Ok(payments));
    }

    [HttpGet("school/{schoolId}")]
    public async Task<IActionResult> GetBySchool(string schoolId, [FromQuery] string? status, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var query = _db.Payments
            .Include(p => p.Student).Include(p => p.Course).Include(p => p.Status)
            .Where(p => p.Course!.SchoolId == schoolId);

        if (!string.IsNullOrEmpty(status))
            query = query.Where(p => p.Status!.Code == status || p.Status!.Name == status);

        var total = await query.CountAsync();
        var payments = await query
            .Skip((page - 1) * pageSize).Take(pageSize)
            .Select(p => MapToDto(p)).ToListAsync();

        return Ok(ApiResponse<List<PaymentDto>>.Ok(payments, new MetaData { Page = page, PageSize = pageSize, Total = total }));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreatePaymentRequest request)
    {
        var pendingStatus = await _db.PaymentStatuses.FirstAsync(s => s.Code == "PENDING");
        var payment = new Domain.Entities.Payment
        {
            Id = Guid.NewGuid().ToString("N")[..12],
            StudentId = request.StudentId,
            CourseId = request.CourseId,
            Amount = request.Amount,
            DueDate = DateTime.TryParse(request.DueDate, out var dd)
                ? DateTime.SpecifyKind(dd, DateTimeKind.Utc)
                : DateTime.UtcNow,
            StatusId = pendingStatus.Id
        };
        _db.Payments.Add(payment);
        await _db.SaveChangesAsync();

        return StatusCode(201, ApiResponse<PaymentDto>.Ok(new PaymentDto
        {
            Id = payment.Id, StudentId = payment.StudentId, CourseId = payment.CourseId,
            Amount = payment.Amount, DueDate = payment.DueDate.ToString("yyyy-MM-dd"),
            Status = "Pending", CreatedAt = DateTime.UtcNow.ToString("o")
        }));
    }

    [HttpPatch("{paymentId}/status")]
    public async Task<IActionResult> UpdateStatus(string paymentId, [FromBody] UpdatePaymentStatusRequest request)
    {
        var payment = await _db.Payments.FindAsync(paymentId);
        if (payment is null) return NotFound(ApiResponse.Fail("NOT_FOUND", "Payment not found."));

        var newStatus = await _db.PaymentStatuses.FirstOrDefaultAsync(s => s.Code == request.Status || s.Name == request.Status);
        if (newStatus is not null) payment.StatusId = newStatus.Id;
        payment.Method = request.Method;
        if (!string.IsNullOrEmpty(request.PaidAt) && DateTime.TryParse(request.PaidAt, out var paidAt))
            payment.PaidAt = DateTime.SpecifyKind(paidAt, DateTimeKind.Utc);

        await _db.SaveChangesAsync();
        return Ok(ApiResponse.Ok(new { id = paymentId, status = request.Status, method = request.Method, paidAt = request.PaidAt }));
    }

    [HttpPost("{paymentId}/pay")]
    public async Task<IActionResult> PayOnline(string paymentId, [FromBody] PayOnlineRequest request)
    {
        var payment = await _db.Payments.FindAsync(paymentId);
        if (payment is null) return NotFound(ApiResponse.Fail("NOT_FOUND", "Payment not found."));

        var paidStatus = await _db.PaymentStatuses.FirstAsync(s => s.Code == "PAID");
        payment.StatusId = paidStatus.Id;
        payment.Method = request.Method ?? "Online";
        payment.PaidAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();

        return Ok(ApiResponse.Ok(new { paymentId, transactionId = $"txn-{Guid.NewGuid():N}"[..16], status = "Paid", paidAt = payment.PaidAt?.ToString("o") }));
    }

    [HttpPost("remind-overdue")]
    public IActionResult RemindOverdue([FromBody] RemindOverdueRequest request)
    {
        // TODO: Implement actual notification sending
        return Ok(ApiResponse.Ok(new { message = "Payment reminder sent." }));
    }

    private static PaymentDto MapToDto(Domain.Entities.Payment p) => new()
    {
        Id = p.Id,
        StudentId = p.StudentId ?? "",
        StudentName = p.Student?.Name ?? "",
        CourseId = p.CourseId ?? "",
        CourseTitle = p.Course?.Title ?? "",
        Amount = p.Amount,
        DueDate = p.DueDate.ToString("yyyy-MM-dd"),
        Status = p.Status?.Name ?? "",
        Method = p.Method,
        PaidAt = p.PaidAt?.ToString("yyyy-MM-dd")
    };
}

