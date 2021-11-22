using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingApp.DTOs
{
    public class PasswordChangeDto
    {
        public string UserName{ get; set; }
        public string Email{ get; set; }
        public string ClientUrl { get; set; }
    }
}
