using System;
using System.Threading.Tasks;

namespace Test.Identity.Services
{
    public interface ISmsSender
    {
        Task SendSmsAsync(string number, string message);
    }
}
