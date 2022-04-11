using TestService.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestService.Core.Interfaces
{
    public interface IPaymentService
    {
        Task<K> PostRequest<K, T>(T model, string url);
        Task<T> GetRequest<T>(string url);
    }
}
