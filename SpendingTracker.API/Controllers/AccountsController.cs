using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SpendingTracker.Application.Common.Dto;
using SpendingTracker.Application.Services.IServices;

namespace SpendingTracker.API.Controllers
{
    [Route("api/accounts")]
    [ApiController]
    [Authorize]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountsController(IAccountService accountService)
        {
            _accountService = accountService;
        }


        [HttpGet]
        public async Task<IActionResult> GetByUserId()
        {
            var response = await _accountService.GetAccountbyUserId(User);

            return Ok(response);

        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] AccountsDto model) 
        {
            var response = await  _accountService.CreatAccount(model, User);
            if (response.IsFailure)
            {
                return BadRequest(response.Error);
            }

            return Ok(response);
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] AccountsDto model)
        {
            var response = await _accountService.UpdateAccount(model, User);
            if (response.IsFailure)
            {
                return BadRequest(response.Error);
            }

            return Ok(response);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] Guid accountId)
        {
            var response = await _accountService.RemoveAccount(accountId, User);
            if (response.IsFailure)
            {
                return BadRequest(response.Error);
            }

            return Ok(response);
        }
    }
}
