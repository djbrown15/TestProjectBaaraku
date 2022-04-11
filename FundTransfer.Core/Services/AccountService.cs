using TestService.Core.DTOs;
using TestService.Core.Entities;
using TestService.Core.Interfaces;
using TestService.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestService.Core.Services
{
    public class AccountService : IAccountService
    {
        private readonly Context _context;

        public AccountService(Context context)
        {
            _context = context;
        }

        public async Task<Response<string>> TopUpAccount(AccountDto model)
        {
            try
            {
                if (model == null)
                {
                    return new Response<string> { HasError = true, Message = "Request cannot be empty" };
                }
                model = Helper.TrimStringProps(model);

                if (model.Amount < 1)
                {
                    return new Response<string> { HasError = true, Message = "Amount must be greater than 0" };
                }

                if (string.IsNullOrEmpty(model.AccountNumber))
                    return new Response<string> { HasError = true, Message = "Customer's account number is required" };

                var account = _context.Accounts.FirstOrDefault(x => x.AccountNumber == model.AccountNumber);
                if (account == null)
                {
                    return new Response<string> { HasError = true, Message = "Customer's account not found" };
                }
                else
                {
                    account.AccountBalance += model.Amount;
                    var trans = new Transaction
                    {
                        TranDate = DateTime.Now,
                        TranAmount = model.Amount,
                        TranType = "CR",
                        AcctId = account.AcctId
                    };
                    _context.Transactions.Add(trans);
                }

                await _context.SaveChangesAsync();

                return new Response<string>
                {
                    HasError = false,
                    Message = $"Your account has been successfully toped up with {model.Amount}"
                };
            }
            catch (Exception ex)
            {
                return new Response<string> { HasError = true, Message = $"{ex.Message}" };
            }
        }
    }
}
