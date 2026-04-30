using EduTrackApi.Application.Branches.Models;
using EduTrackApi.Application.Common.Models;
using Microsoft.AspNetCore.Mvc;

namespace EduTrackApi.Api.Controllers;

[ApiController]
[Route("v1")]
public sealed class BranchesController : ControllerBase
{
    [HttpGet("branches")]
    public IActionResult GetBranches()
    {
        var branches = new List<BranchDto>
        {
            new() { Id = "b1", Name = "Football" },
            new() { Id = "b2", Name = "Basketball" },
            new() { Id = "b3", Name = "Mathematics" },
            new() { Id = "b4", Name = "Volleyball" },
            new() { Id = "b5", Name = "Swimming" }
        };
        return Ok(ApiResponse<List<BranchDto>>.Ok(branches));
    }

    [HttpGet("categories")]
    public IActionResult GetCategories()
    {
        var categories = new List<CategoryDto>
        {
            new() { Id = "c1", Name = "U19" },
            new() { Id = "c2", Name = "U15" },
            new() { Id = "c3", Name = "Private Lesson" },
            new() { Id = "c4", Name = "Group" }
        };
        return Ok(ApiResponse<List<CategoryDto>>.Ok(categories));
    }
}
