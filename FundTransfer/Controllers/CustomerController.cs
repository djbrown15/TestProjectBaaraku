using TestService.Core.DTOs;
using TestService.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestService.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        private readonly IAccountService _accountService;


        public CustomerController(ICustomerService customerService, IAccountService accountService)
        {
            _customerService = customerService;
            _accountService = accountService;

        }

        [HttpPost]
        public async Task<IActionResult> CreateNewCustomer([FromBody] KYCDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var response = await _customerService.CreateNew(model);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> TopUpAccount([FromBody] AccountDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var response = await _accountService.TopUpAccount(model);
            return Ok(response);
        }

    }
}
