using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingApp.Entities
{
    public class AppUser : IdentityUser<int>
    {
        public string Fullname { get; set; }
        public string UserRole { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string EmailSent{ get; set; }
        public string PhotoUrl{ get; set; }
        public string PublicId{ get; set; }
        public ICollection<AppUserRole> UserRoles { get; set; }
        public int IsActive{ get; set; }
        public string UniqueId{ get; set; }
    }
}
