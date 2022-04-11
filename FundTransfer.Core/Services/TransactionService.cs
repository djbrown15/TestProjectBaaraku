using TestService.Core.DTOs;
using TestService.Core.Entities;
using TestService.Core.Interfaces;
using TestService.Core.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestService.Core.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly Context _context;
        private readonly IPaymentService _paymentService;
        private IConfiguration _config;

        public TransactionService(Context context, IPaymentService paymentService, IConfiguration _configuration)
        {
            _config = _configuration;
            _context = context;
            _paymentService = paymentService;
        }

        public async Task<Response<PaystackResponse>> Transfer(FundTransferDto model)
        {
            try
            {
                if (model == null)
                {
                    return new Response<PaystackResponse> { HasError = true, Message = "Request cannot be empty" };
                }
                model = Helper.TrimStringProps(model);

                if (model.Amount < 1)
                {
                    return new Response<PaystackResponse> { HasError = true, Message = "Amount must be greater than 0" };
                }

                if (string.IsNullOrEmpty(model.AccountNumber))
                    return new Response<PaystackResponse> { HasError = true, Message = "Account number is required" };

                var account = _context.Accounts.Where(x => x.AccountNumber == model.AccountNumber).Include(x => x.Customer).FirstOrDefault();
                if (account == null)
                {
                    return new Response<PaystackResponse> { HasError = true, Message = "Customer's account not found" };
                }

                if (model.TranType == "CR")
                {
                    account.AccountBalance += model.Amount;
                }
                else if (model.TranType == "DR")
                {
                    if (account.AccountBalance < model.Amount)
                    {
                        return new Response<PaystackResponse> { HasError = true, Message = "Insufficient funds" };
                    }

                    // call payment gateway
                    PaymentDto dto = new()
                    {
                        email = $"{_config.GetSection("ApiSettings")["BusinessEmail"]}",
                        amount = (Convert.ToDouble(model.Amount) * 100).ToString()
                    };

                    PaystackResponse response = await _paymentService.PostRequest<PaystackResponse, PaymentDto>(dto, $"{_config.GetSection("ApiSettings")["InitializeUri"]}");
                    if (response.status)
                    {
                        // redirect to checkout page
                        // and verify transaction
                        // but I would proceed to balance the books from here

                        //debit customer's account
                        account.AccountBalance -= model.Amount;

                        var trans = new Transaction
                        {
                            TranDate = DateTime.Now,
                            TranAmount = model.Amount,
                            TranType = model.TranType,
                            AcctId = account.AcctId
                        };
                        await _context.Transactions.AddAsync(trans);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        return new Response<PaystackResponse>
                        {
                            HasError = true,
                            Message = "Transfer could not be initialized"
                        };
                    }
                }
                else
                {
                    return new Response<PaystackResponse> { HasError = true, Message = "Invalid transaction type" };
                }



                return new Response<PaystackResponse>
                {
                    HasError = false,
                    Message = $"{model.TranType}: Transfer of {model.Amount} was successful"
                };
            }
            catch (Exception ex)
            {
                return new Response<PaystackResponse> { HasError = true, Message = $"{ex.Message}" };
            }
        }


        //    verify trx from paystack and save transaction details
    }
}
