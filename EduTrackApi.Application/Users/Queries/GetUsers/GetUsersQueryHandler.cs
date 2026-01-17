using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EduTrackApi.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EduTrackApi.Application.Users.Queries.GetUsers;

public sealed class GetUsersQueryHandler
{
    private readonly IEduTrackDbContext _context;

    public GetUsersQueryHandler(IEduTrackDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<UserDto>> HandleAsync(
        GetUsersQuery query,
        CancellationToken cancellationToken = default)
    {
        var users = await _context.Users
            .Include(u => u.Role)
            .Select(u => new UserDto
            {
                Id = u.Id,
                Name = u.Name,
                Email = u.Email,
                SchoolId = u.SchoolId,
                RoleCode = u.Role != null ? u.Role.Code : null
            })
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return users;
    }
}