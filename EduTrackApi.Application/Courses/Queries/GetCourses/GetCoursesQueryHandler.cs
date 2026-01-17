using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EduTrackApi.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EduTrackApi.Application.Courses.Queries.GetCourses;

public sealed class GetCoursesQueryHandler
{
    //private readonly IEduTrackDbContext _context;

    //public GetCoursesQueryHandler(IEduTrackDbContext context)
    //{
    //    _context = context;
    //}

    public async Task<IReadOnlyList<CourseDto>> HandleAsync(
        GetCoursesQuery query,
        CancellationToken cancellationToken = default)
    {
        //var courses = await _context.Courses
        //    .Select(c => new CourseDto
        //    {
        //        Id = c.Id,
        //        Title = c.Title,
        //        SchoolId = c.SchoolId,
        //        BranchId = c.BranchId,
        //        CategoryId = c.CategoryId,
        //        TeacherId = c.TeacherId,
        //        Location = c.Location,
        //        Address = c.Address
        //    })
        //    .AsNoTracking()
        //    .ToListAsync(cancellationToken);

        //return courses; 

        // constants.tsx içindeki MOCK_COURSES verisine denk gelecek şekilde mock course listesi
        var courses = new List<CourseDto>
        {
            new()
            {
                Id = "crs1",
                Title = "U19 Futbol Elit",
                SchoolId = "school-a",
                BranchId = "b1",
                CategoryId = "c1",
                TeacherId = "u1",
                Location = "A Sahası",
                Address = "41.0082, 28.9784"
            },
            new()
            {
                Id = "crs2",
                Title = "Matematik İleri Seviye",
                SchoolId = "school-a",
                BranchId = "b3",
                CategoryId = "c3",
                TeacherId = "u7",
                Location = "Z-12 Laboratuvarı",
                Address = "Ankara, Çankaya"
            }
        };

        // İsteğe göre query ile filtreleme (örnekler, ihtiyaca göre açılabilir)
        // if (!string.IsNullOrEmpty(query.SchoolId))
        //     courses = courses.Where(c => c.SchoolId == query.SchoolId).ToList();
        //
        // if (!string.IsNullOrEmpty(query.TeacherId))
        //     courses = courses.Where(c => c.TeacherId == query.TeacherId).ToList();

        return await Task.FromResult<IReadOnlyList<CourseDto>>(courses);

    }
}