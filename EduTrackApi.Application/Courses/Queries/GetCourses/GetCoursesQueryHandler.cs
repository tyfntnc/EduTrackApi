using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EduTrackApi.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EduTrackApi.Application.Courses.Queries.GetCourses;

public sealed class GetCoursesQueryHandler
{
    private readonly IEduTrackDbContext _context;

    public GetCoursesQueryHandler(IEduTrackDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<CourseDto>> HandleAsync(
        GetCoursesQuery query,
        CancellationToken cancellationToken = default)
    {
        var courses = await _context.Courses
            .Select(c => new CourseDto
            {
                Id = c.Id,
                Title = c.Title,
                SchoolId = c.SchoolId,
                BranchId = c.BranchId,
                CategoryId = c.CategoryId,
                TeacherId = c.TeacherId,
                Location = c.Location,
                Address = c.Address
            })
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return courses;
    }
}