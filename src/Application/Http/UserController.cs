using System;
using System.Threading.Tasks;
using Application.CQS.User.Output;
using Application.CQS.User.Query;
using Common.Util;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Application.Http
{
    [ApiController]
    [Route("users")]
    [AuthorizeRoles(UserRole.Manager)]
    public class UserController : Controller
    {
        [HttpGet]
        public PaginatedData<UserOutput> GetAllUsers(
            [FromServices] GetAllUsersQuery query,
            [FromQuery] Pagination pagination
        )
        {
            return query.Execute(pagination);
        }

        [HttpGet]
        [Route("{userId}")]
        public async Task<UserOutput> GetUser([FromServices] GetUserQuery query, [FromRoute] Guid userId)
        {
            return await query.ExecuteAsync(userId);
        }
    }
}
