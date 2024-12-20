﻿using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SpendingTracker.Application.Common.Dto;
using SpendingTracker.Application.Services.IServices;

namespace SpendingTracker.API.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto model)
        {
            var response = await _authService.Login(model);
            if (response.IsFailure)
            {
                return BadRequest(response.Error);
            }
            return Ok(response);
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto model)
        {
            var response = await _authService.Register(model);
            if (response.IsFailure)
            {
                return BadRequest(response.Error);
            }
            return Ok(response);
        }
    }
}
