using System;

namespace EduTrackApi.Domain.Entities;

public class CourseSchedule
{
    public long Id { get; set; }
    public string CourseId { get; set; } = null!;
    public short DayOfWeek { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }

    public Course Course { get; set; } = null!;
}