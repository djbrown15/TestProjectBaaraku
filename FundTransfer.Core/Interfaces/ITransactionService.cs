using TestService.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestService.Core.Interfaces
{
    public interface ITransactionService
    {
        Task<Response<PaystackResponse>> Transfer(FundTransferDto model);
    }
}
