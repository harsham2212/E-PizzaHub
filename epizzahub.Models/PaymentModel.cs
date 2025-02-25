﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace epizzahub.Models
{
    public class PaymentModel
    {
        public string Name { get; set; }
        public string RazorpayKey { get; set; }
        public decimal GrandTotal { get; set; }
        public string Description { get; set; }
        public string Currency { get; set; }
        public string OrderId { get; set; }
        public CartModel Cart { get; set; }
        public string Receipt { get; set; }
    }
}
