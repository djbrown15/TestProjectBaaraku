using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestService.Core.DTOs
{
    public class Response<T>
    {
        public bool HasError { get; set; }
        public string Message { get; set; }
        public T Result { get; set; }
    }
}
