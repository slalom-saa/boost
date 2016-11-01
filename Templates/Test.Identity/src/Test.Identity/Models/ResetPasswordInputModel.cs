namespace Test.Identity.Models
{
    public class ResetPasswordInputModel
    {
        public string UserId { get; set; }

        public string Token { get; set; }

        public string Password { get; set; }
    }
}