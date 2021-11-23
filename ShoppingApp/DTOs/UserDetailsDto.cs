using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingApp.DTOs
{
    public class UserDetailsDto
    {
        public string Fullname{ get; set; }
        public string UserName{ get; set; }
        public string Email{ get; set; }
        public string PhoneNumber{ get; set; }
        public string City{ get; set; }
        public string State{ get; set; }
        public string Country{ get; set; }
        public string UserRole{ get; set; }
        public string PhotoUrl{ get; set; }
        public string PublicId{ get; set; }
    }
}
