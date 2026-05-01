using EduTrackApi.Application.Branches.Models;
using EduTrackApi.Application.Common.Interfaces;
using EduTrackApi.Application.Common.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduTrackApi.Api.Controllers;

[Authorize]
[ApiController]
[Route("v1")]
public sealed class BranchesController : ControllerBase
{
    private readonly IEduTrackDbContext _db;

    public BranchesController(IEduTrackDbContext db)
    {
        _db = db;
    }

    [HttpGet("branches")]
    public async Task<IActionResult> GetBranches()
    {
        var branches = await _db.Branches
            .Select(b => new BranchDto { Id = b.Id, Name = b.Name })
            .ToListAsync();
        return Ok(ApiResponse<List<BranchDto>>.Ok(branches));
    }

    [HttpGet("categories")]
    public async Task<IActionResult> GetCategories()
    {
        var categories = await _db.Categories
            .Select(c => new CategoryDto { Id = c.Id, Name = c.Name })
            .ToListAsync();
        return Ok(ApiResponse<List<CategoryDto>>.Ok(categories));
    }
}

