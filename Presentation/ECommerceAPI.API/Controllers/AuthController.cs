﻿using ECommerceAPI.Application.Features.Commands.AppUser.GoogleLogin;
using ECommerceAPI.Application.Features.Commands.AppUser.LoginUser;
using ECommerceAPI.Application.Features.Commands.AppUser.PasswordReset;
using ECommerceAPI.Application.Features.Commands.AppUser.RefreshTokenLogin;
using ECommerceAPI.Application.Features.Commands.AppUser.VerifyResfreshToken;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost("[action]")]
        public async Task<IActionResult> Login(LoginUserCommandRequest loginUserCommandRequest)
        {
            LoginUserCommandResponse loginUserCommandResponse = await _mediator.Send(loginUserCommandRequest);
            return Ok(loginUserCommandResponse);
        }
        [HttpPost("[action]")]
        public async Task<IActionResult> RefreshTokenLogin([FromForm] RefreshTokenLoginCommandRequest refreshTokenLoginCommandRequest)
        {
            RefreshTokenLoginCommandResponse refreshTokenLoginCommandResponse= await _mediator.Send(refreshTokenLoginCommandRequest);
            return Ok(refreshTokenLoginCommandResponse);
        }

        [HttpPost("google-login")]
        public async Task<IActionResult> GoogleLogin(GoogleLoginCommandRequest googleLoginCommandRequest)
        {
            GoogleLoginCommandResponse googleLoginCommandResponse = await _mediator.Send(googleLoginCommandRequest);
            //Gelen istek mediator ile handler a gonderildi. response elde edildi.

            return Ok(googleLoginCommandResponse);

        }
        [HttpPost("password-reset")]
        public async Task<IActionResult> PasswordReset([FromBody] PasswordResetCommandRequest passwordResetCommandRequest)
        {
           PasswordResetCommandResponse passwordResetCommandResponse = await _mediator.Send(passwordResetCommandRequest);
            return Ok(passwordResetCommandResponse);
        }
        
        
        [HttpPost("verify-reset-token")]
        public async Task<IActionResult> VerifyResetToken([FromBody] VerifyRefreshTokenCommandRequest verifyRefreshTokenCommandRequest)
        {
            VerifyRefreshTokenCommandResponse verifyRefreshTokenCommandResponse  = await _mediator.Send(verifyRefreshTokenCommandRequest);
            return Ok(verifyRefreshTokenCommandResponse);
        }
    }
}
