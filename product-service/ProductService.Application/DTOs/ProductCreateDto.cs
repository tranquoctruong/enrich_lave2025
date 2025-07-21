﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Application.DTOs
{
    public class ProductCreateDto
    {
        public string Name { get; set; } = default!;
        public decimal Price { get; set; }
    }
}
