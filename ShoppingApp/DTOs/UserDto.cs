﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingApp.DTOs
{
    public class UserDto
    {
        public string Username { get; set; }
        public string Token { get; set; }
        public string UserRole { get; set; }
        public string Gender { get; set; }
        public string PhotoUrl{ get; set; }
        public string UniqueId{ get; set; }
    }
}
