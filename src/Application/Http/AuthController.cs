using System.Threading.Tasks;
using Application.CQS.Auth.Command;
using Application.CQS.Auth.Input;
using Application.CQS.Auth.Output;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Application.Http
{
    [AllowAnonymous]
    [Route("auth")]
    public class AuthController : Controller
    {
        [HttpPost]
        [Route("sign-in")]
        public async Task<SignInOutput> SignIn([FromServices] SignInCommand command, [FromBody] SignInInput input)
        {
            return await command.Execute(input);
        }

        [HttpPost]
        [Route("sign-up")]
        public async Task<SignInOutput> SignUp([FromServices] SignUpCommand command, [FromBody] SignUpInput input)
        {
            return await command.ExecuteAsync(input);
        }
    }
}