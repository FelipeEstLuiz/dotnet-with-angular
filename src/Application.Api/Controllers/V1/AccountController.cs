using Application.Api.Controllers._Shared;
using Application.Api.Util;
using Application.Core.Common.Dispatcher;
using Application.Core.DTO.User;
using Application.Core.Model;
using Application.Core.Model.User;
using Application.Domain.Extensions;
using Application.Domain.Model;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Mime;

namespace Application.Api.Controllers.V1;

[ApiExplorerSettings(GroupName = "Account")]
[Consumes(MediaTypeNames.Application.Json)]
[Produces("application/json")]
public class AccountController(CommunicationProtocol protocol, RequestDispatcher dispatcher)
    : BaseApplicationController(protocol)
{
    [HttpPost]
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(Response<LoginDto>))]
    [ProducesResponseType((int)HttpStatusCode.Created, Type = typeof(Response<LoginDto>))]
    public async Task<IActionResult> InsertUsuarioAsync([FromBody] InsertUserModel request)
    {
        Result<UserLoginDto> response = await dispatcher.Dispatch<InsertUserModel, Result<UserLoginDto>>(request);

        if (response.IsSuccess)
        {
            await SetRefreshToken(response.Data!.RefreshToken);
            return HandlerResponse(HttpStatusCode.OK, Result<LoginDto>.Success(response.Data!.Login));
        }

        return HandlerResponse(HttpStatusCode.OK, response);
    }

    [HttpPost("Login")]
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(Response<LoginDto>))]
    public async Task<IActionResult> LoginAsync([FromBody] LoginModel request)
    {
        Result<UserLoginDto> response = await dispatcher.Dispatch<LoginModel, Result<UserLoginDto>>(request);

        if (response.IsSuccess)
        {
            await SetRefreshToken(response.Data!.RefreshToken);
            return HandlerResponse(HttpStatusCode.OK, Result<LoginDto>.Success(response.Data!.Login));
        }

        return HandlerResponse(HttpStatusCode.OK, response);
    }

    [HttpPost("Logout")]
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(Response<bool>))]
    public async Task<IActionResult> Logout()
    {
        Result<bool> response = await dispatcher.Dispatch<LogoutModel, Result<bool>>(new LogoutModel(User.GetUserId()));

        if (response.IsSuccess)
            Response.Cookies.Delete("refreshToken");

        return HandlerResponse(HttpStatusCode.OK, response);
    }

    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken()
    {
        string? refreshToken = Request.Cookies["refreshToken"];
        Result<UserLoginDto> response = await dispatcher.Dispatch<RefreshTokenModel, Result<UserLoginDto>>(new RefreshTokenModel(refreshToken));

        if (response.IsSuccess)
        {
            if (response.Data is not null)
                await SetRefreshToken(response.Data!.RefreshToken);

#pragma warning disable CS8604
            return HandlerResponse(HttpStatusCode.OK, Result<LoginDto>.Success(response.Data?.Login));
#pragma warning restore CS8604
        }

        return HandlerResponse(HttpStatusCode.OK, response);
    }

    private async Task SetRefreshToken(string refreshToken)
    {
        CookieOptions cookieOptions = new()
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTimeOffset.UtcNow.AddDays(7)
        };

        Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
    }
}
