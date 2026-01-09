using FitPlanner.Api.Attributes;
using FitPlanner.Application.UseCases.Login.DoLogin;
using FitPlanner.Application.UseCases.Login.Logout;
using FitPlanner.Application.UseCases.Login.RefreshToken;
using FitPlanner.Communication.Requests;
using FitPlanner.Communication.Responses;
using Microsoft.AspNetCore.Mvc;

namespace FitPlanner.Api.Controllers;

public class LoginController : FitPlannerBaseController
{
    [HttpPost]
    [ProducesResponseType(typeof(ResponseRegisteredUserJson), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromServices] IDoLoginUseCase useCase, [FromBody] RequestLoginJson request)
    {
        var response = await useCase.Execute(request);

        return Ok(response);
    }

    [HttpPost("refresh-token")]
    [ProducesResponseType(typeof(ResponseTokensJson), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> RefreshToken([FromServices] IRefreshTokenUseCase useCase, [FromBody] RequestRefreshTokenJson request)
    {
        var response = await useCase.Execute(request);

        return Ok(response);
    }

    [HttpPost("logout")]
    [AuthenticatedUser]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Logout([FromServices] ILogoutUseCase useCase)
    {
        await useCase.Execute();

        return NoContent();
    }
}