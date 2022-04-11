using TestService.Core.DTOs;
using TestService.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace TestService.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _transactionService;
        private readonly IPaymentService _paymentService;

        public TransactionController(ITransactionService transactionService, IPaymentService paymentService)
        {
            _transactionService = transactionService;
            _paymentService = paymentService;
        }

        [HttpPost]
        public async Task<IActionResult> FundTransfer([FromBody] FundTransferDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var response = await _transactionService.Transfer(model);
            return Ok(response);
        }

    }
}
