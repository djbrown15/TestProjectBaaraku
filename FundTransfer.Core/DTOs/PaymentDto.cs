﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestService.Core.DTOs
{
    public class PaymentDto
    {
        public string email { get; set; }

        public string amount { get; set; }

        public string publicKey { get; set; }
    }
}
