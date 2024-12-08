using Microsoft.AspNetCore.Identity.UI.Services;

namespace ProjectWeb.Utility
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            //Implementation email services
            return Task.CompletedTask;
        }
    }
}
