﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TakeCommand.Data.Models
{
    public class OrderDto
    {
        public int Id { get; set; }
        public string Address { get; set; }

        public string Email { get; set; }

        public int Total { get; set; }
        
        public bool IsReturned { get; set; }

    }
}