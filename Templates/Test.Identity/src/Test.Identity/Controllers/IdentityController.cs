using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Test.Identity.Data;
using Test.Identity.Models;
using Test.Identity.Services;

namespace Test.Identity.Controllers
{
    [Authorize]
    public class IdentityController : ControllerBase
    {
        private readonly IEmailSender _emailSender;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ISmsSender _smsSender;
        private readonly UserManager<ApplicationUser> _userManager;

        public IdentityController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IEmailSender emailSender, ISmsSender smsSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _smsSender = smsSender;
        }

        [Route("identity/actions/register"), HttpPost]
        public async Task Register([FromBody] RegisterInputModel request)
        {
            if (!this.ModelState.IsValid)
            {
                throw new InvalidOperationException();
            }

            await _userManager.CreateAsync(new ApplicationUser { Email = request.Email, UserName = request.Email }, request.Password);
        }

        [Route("identity/actions/confirm-email"), HttpPost]
        public async Task ConfirmEmail(string userName, string code)
        {
            var user = await _userManager.FindByIdAsync(userName);

            await _userManager.ConfirmEmailAsync(user, code);
        }

        [Route("identity/actions/request-reset"), HttpPost]
        public async Task<string> RequestReset(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user.EmailConfirmed)
            {
                return await _userManager.GeneratePasswordResetTokenAsync(user);
            }
            throw new InvalidOperationException("The specified user has not confirmed email.");
        }

        [Route("identity/actions/reset-password"), HttpPost]
        public async Task ResetPassword([FromBody] ResetPasswordInputModel request)
        {
            if (!this.ModelState.IsValid)
            {
                throw new InvalidOperationException();
            }

            var user = await _userManager.FindByIdAsync(request.UserId);
            await _userManager.ResetPasswordAsync(user, request.Token, request.Password);
        }

        //[Route("identity/actions/send-code"), HttpPost]
        //public async Task SendCode([FromBody] SendCodeInputModel request)
        //{
        //    var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();

        //    var code = await _userManager.GenerateTwoFactorTokenAsync(user, request.Provider);

        //    var message = "Your security code is: " + code;
        //    if (request.Provider == "Email")
        //    {
        //        await _emailSender.SendEmailAsync(await _userManager.GetEmailAsync(user), "Security Code", message);
        //    }
        //    else if (request.Provider == "Phone")
        //    {
        //        await _smsSender.SendSmsAsync(await _userManager.GetPhoneNumberAsync(user), message);
        //    }
        //}

        //[Route("identity/actions/verify-code"), HttpPost]
        //public async Task<VerifyCodeStatus> VerifyCode([FromBody] VerifyCodeInputModel request)
        //{
        //    if (!this.ModelState.IsValid)
        //    {
        //        throw new InvalidOperationException();
        //    }

        //    var result = await _signInManager.TwoFactorSignInAsync(request.Provider, request.Code, request.RememberMe, request.RememberBrowser);
        //    if (result.Succeeded)
        //    {
        //        return VerifyCodeStatus.Success;
        //    }
        //    if (result.IsLockedOut)
        //    {
        //        return VerifyCodeStatus.LockedOut;
        //    }
        //    return VerifyCodeStatus.Invalid;
        //}
    }
}