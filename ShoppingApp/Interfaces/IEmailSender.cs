using ShoppingApp.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingApp.Interfaces
{
    public interface IEmailSender
    {
        public Task SendEmailAsync(Message message, string username);

        public Task SendResetPasswordAsync(Message message, string username);
    }
}
