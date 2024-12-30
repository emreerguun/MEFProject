using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;

public class EmailService
{
    private readonly string _sendGridApiKey;

    //TO-DO:burada configuration bilgileri appsettingsten alınacak.
    public EmailService(/*IConfiguration configuration*/)
    {
        //_sendGridApiKey = configuration["SendGrid:ApiKey"];
    }

    public async Task SendEmailAsync(string toEmail, string subject, string message)
    {
        var client = new SendGridClient(_sendGridApiKey);
        var msg = new SendGridMessage
        {
            From = new EmailAddress("your-email@example.com", "Your App Name"),
            Subject = subject,
            PlainTextContent = message,
            HtmlContent = message
        };
        msg.AddTo(new EmailAddress(toEmail));

        await client.SendEmailAsync(msg);
    }
}
