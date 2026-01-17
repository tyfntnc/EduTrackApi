using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EduTrackApi.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EduTrackApi.Application.Users.Queries.GetUsers;

public sealed class GetUsersQueryHandler
{
    //private readonly IEduTrackDbContext _context;

    //public GetUsersQueryHandler(IEduTrackDbContext context)
    //{
    //    _context = context;
    //}

    public async Task<IReadOnlyList<UserDto>> HandleAsync(
        GetUsersQuery query,
        CancellationToken cancellationToken = default)
    {
        //var users = await _context.Users
        //    .Include(u => u.Role)
        //    .Select(u => new UserDto
        //    {
        //        Id = u.Id,
        //        Name = u.Name,
        //        Email = u.Email,
        //        SchoolId = u.SchoolId,
        //        RoleCode = u.Role != null ? u.Role.Code : null
        //    })
        //    .AsNoTracking()
        //    .ToListAsync(cancellationToken);

        //return users;



        var users = new List<UserDto>
        {
            new()
            {
                Id = "admin",
                Name = "Zeynep Sistem",
                Email = "admin@edutrack.com",
                SchoolId = null,
                RoleCode = "SYSTEM_ADMIN"
            },
            new()
            {
                Id = "u4",
                Name = "Canan Sert",
                Email = "canan@okul-a.com",
                SchoolId = "school-a",
                RoleCode = "SCHOOL_ADMIN"
            },
            new()
            {
                Id = "u1",
                Name = "Ahmet Yılmaz",
                Email = "ahmet@okul-a.com",
                SchoolId = "school-a",
                RoleCode = "TEACHER"
            },
            new()
            {
                Id = "u3",
                Name = "Ayşe Demir",
                Email = "ayse@veli.com",
                SchoolId = null,
                RoleCode = "PARENT"
            },
            new()
            {
                Id = "u2",
                Name = "Mehmet Kaya",
                Email = "mehmet@okul-a.com",
                SchoolId = "school-a",
                RoleCode = "STUDENT"
            },
            new()
            {
                Id = "u9",
                Name = "Ali Vural",
                Email = "ali@okul-a.com",
                SchoolId = "school-a",
                RoleCode = "STUDENT"
            },
            new()
            {
                Id = "u5",
                Name = "Bülent Arın",
                Email = "bulent@okul-b.com",
                SchoolId = "school-b",
                RoleCode = "SCHOOL_ADMIN"
            },
            new()
            {
                Id = "u7",
                Name = "Fatma Şahin",
                Email = "fatma@okul-a.com",
                SchoolId = "school-a",
                RoleCode = "TEACHER"
            },
            new()
            {
                Id = "u8",
                Name = "Murat Can",
                Email = "murat@okul-a.com",
                SchoolId = "school-a",
                RoleCode = "TEACHER"
            }
        };

        // İstersen burada query'ye göre filtre uygulayabilirsin:
        // ör: okulId veya rol koduna göre
        // if (!string.IsNullOrEmpty(query.SchoolId))
        //     users = users.Where(u => u.SchoolId == query.SchoolId).ToList();

        return await Task.FromResult<IReadOnlyList<UserDto>>(users);

    }
}