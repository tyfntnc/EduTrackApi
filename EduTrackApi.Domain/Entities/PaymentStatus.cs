using System.Collections.Generic;

namespace EduTrackApi.Domain.Entities;

public class PaymentStatus
{
    public short Id { get; set; }
    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;

    public ICollection<Payment> Payments { get; set; } = new List<Payment>();
}