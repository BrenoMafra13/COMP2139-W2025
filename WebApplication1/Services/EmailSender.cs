using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace WebApplication1.Services;

public class EmailSender : IEmailSender
{
    private readonly string _sendGridApiKey;

    public EmailSender(IConfiguration configuration)
    {
        _sendGridApiKey = configuration["SendGrid:ApiKey"] 
                          ?? throw new ArgumentNullException("SendGrid Key is missing");
    }

    public async Task SendEmailAsync(string email, string subject, string message)
    {
        try
        {
            var client = new SendGridClient(_sendGridApiKey);
            var from = new EmailAddress("brenolopesmafra@gmail.com", "Pm Tool Default Sender");
            var to = new EmailAddress(email);
            var msg = MailHelper
                .CreateSingleEmail(from, to, subject, "Welcome to PM Tool Inc", message);
            var response = await client.SendEmailAsync(msg);

            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = await response.Body.ReadAsStreamAsync();
                Console.WriteLine($"An error occurred while sending email: {errorMessage}");
            }
        }
        catch(Exception ex)
        {
            Console.WriteLine($"An error occurred while sending email: {ex.Message}");
            throw;
        }
    }
}