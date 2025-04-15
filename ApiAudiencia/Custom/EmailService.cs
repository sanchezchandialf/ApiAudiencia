using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Options;
public class SmtpSettings
{
    public required string Server { get; set; }
    public int Port { get; set; }
    public required string SenderName { get; set; }
    public required string SenderEmail { get; set; }
    public required string Username { get; set; }
    public required string Password { get; set; }
    public bool EnableSsl { get; set; }
    public bool UseDefaultCredentials { get; set; }
}

public interface IEmailService
{
    Task SendEmailAsync(string toEmail, string subject, string htmlContent);
}

public class EmailService : IEmailService
{
    private readonly SmtpSettings _settings;
    private readonly ILogger<EmailService> _logger;

    public EmailService(IOptions<SmtpSettings> settings, ILogger<EmailService> logger)
    {
        _settings = settings.Value;
        _logger = logger;
    }

    public async Task SendEmailAsync(string toEmail, string subject, string htmlContent)
    {
        using var client = new SmtpClient(_settings.Server, _settings.Port)
        {
            EnableSsl = _settings.EnableSsl,
            UseDefaultCredentials = _settings.UseDefaultCredentials,
            Credentials = new NetworkCredential(_settings.Username, _settings.Password),
            DeliveryMethod = SmtpDeliveryMethod.Network,
            Timeout = 10000 // 10 segundos de espera
        };

        var mailMessage = new MailMessage
        {
            From = new MailAddress(_settings.SenderEmail, _settings.SenderName),
            Subject = subject,
            Body = htmlContent,
            IsBodyHtml = true,
            Priority = MailPriority.Normal
        };
        mailMessage.To.Add(toEmail);

        try
        {
            client.TargetName = "SMTPSVC/";
            client.EnableSsl = true;
            await client.SendMailAsync(mailMessage);
            _logger.LogInformation($"Email enviado a {toEmail}");
        }
        catch (SmtpException ex)
        {
            _logger.LogError(ex, $"Error SMTP al enviar a {toEmail}. Status: {ex.StatusCode}");
            throw new Exception($"Error SMTP: {ex.Message}");
        }
    }
}