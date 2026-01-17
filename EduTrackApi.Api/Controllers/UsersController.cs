using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EduTrackApi.Application.Users.Queries.GetUsers;
using Microsoft.AspNetCore.Mvc;

namespace EduTrackApi.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class UsersController : ControllerBase
{
    private readonly GetUsersQueryHandler _getUsersHandler;

    public UsersController(GetUsersQueryHandler getUsersHandler)
    {
        _getUsersHandler = getUsersHandler;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<UserDto>>> Get(CancellationToken cancellationToken)
    {
        var result = await _getUsersHandler.HandleAsync(new GetUsersQuery(), cancellationToken);
        return Ok(result);
    }
}