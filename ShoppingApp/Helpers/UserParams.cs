﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingApp.Helpers
{
    public class UserParams : PaginationParams
    {

        public string Gender { get; set; }
        public string Category{ get; set; }

        public int MinPrice { get; set; } = 1;

        public int MaxPrice { get; set; } = 100000;
    }
}
