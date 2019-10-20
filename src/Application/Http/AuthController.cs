using app.Application.CQS.Auth.Command;
using app.Application.CQS.Auth.Input;
using app.Application.CQS.Auth.Output;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace app.Application.Http
{
    [AllowAnonymous]
    [Route("auth")]
    public class AuthController : Controller
    {
        [HttpPost]
        [Route("sign-in")]
        public SignInOutput SignIn([FromServices] SignInCommand command, [FromBody] SignInInput input)
        {
            return command.Execute(input);
        }

        [HttpPost]
        [Route("sing-up")]
        public SignInOutput SignUp([FromServices] SignUpCommand command, [FromBody] SignUpInput input)
        {
            return command.Execute(input);
        }
    }
}