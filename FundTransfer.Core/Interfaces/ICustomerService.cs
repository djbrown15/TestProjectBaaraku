using TestService.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestService.Core.Interfaces
{
    public interface ICustomerService
    {
        Task<Response<string>> CreateNew(KYCDto model);
    }
}
