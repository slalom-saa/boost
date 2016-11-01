using System;
using System.Threading.Tasks;

namespace Test.Identity.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}
