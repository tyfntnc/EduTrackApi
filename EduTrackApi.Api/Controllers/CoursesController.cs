using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EduTrackApi.Application.Courses.Queries.GetCourses;
using Microsoft.AspNetCore.Mvc;

namespace EduTrackApi.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class CoursesController : ControllerBase
{
    private readonly GetCoursesQueryHandler _getCoursesHandler;

    public CoursesController(GetCoursesQueryHandler getCoursesHandler)
    {
        _getCoursesHandler = getCoursesHandler;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<CourseDto>>> Get(CancellationToken cancellationToken)
    {
        var result = await _getCoursesHandler.HandleAsync(new GetCoursesQuery(), cancellationToken);
        return Ok(result);
    }
}