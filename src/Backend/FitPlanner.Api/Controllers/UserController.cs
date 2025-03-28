using FitPlanner.Application.UseCases.User.Register;
using FitPlanner.Communication.Requests;
using FitPlanner.Communication.Responses;
using Microsoft.AspNetCore.Mvc;

namespace FitPlanner.Api.Controllers;
[Route("[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(ResponseRegisteredUserJson), StatusCodes.Status201Created)]
    public async Task<IActionResult> Execute(
        [FromServices] IRegisterUserUseCase useCase,
        [FromBody] RequestRegisterUserJson request)
    {
        var result = await useCase.Execute(request);
        
        return Created(string.Empty, result);
    }
}