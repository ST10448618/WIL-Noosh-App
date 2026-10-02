using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using NooshApp.Api.Models;
using NooshApp.Api.Services.Interfaces;

namespace NooshApp.Api.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IConfiguration configuration, ILogger<EmailService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }
// Asynchronously builds and sends an email notification with file attachments for a new job application
        public async Task SendCareerApplicationNotificationAsync(JobApplication application, List<string> attachmentPaths)
        {
            // Asynchronously builds and sends an email notification with file attachments for a new job application

            var smtpHost = _configuration["Smtp:Host"];
            var smtpPort = int.Parse(_configuration["Smtp:Port"] ?? "587");
            var smtpUser = _configuration["Smtp:Username"];
            var smtpPass = _configuration["Smtp:Password"];
            var fromEmail = _configuration["Smtp:FromEmail"];
            var fromName = _configuration["Smtp:FromName"] ?? "Noosh Website";
            var ownerEmail = _configuration["BusinessContact:CareersEmail"];

            if (string.IsNullOrEmpty(smtpHost) || string.IsNullOrEmpty(ownerEmail))
            {
                
                _logger.LogWarning("SMTP not configured — skipping career application email.");
                return;
            }

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(fromName, fromEmail));
            message.To.Add(MailboxAddress.Parse(ownerEmail));
            message.Subject = $"New Job Application: {application.FullName} — {application.DesiredPosition}";
            // Validate and attach files (CV and supporting docs) to the email
            var scoreLabel = application.Status == ApplicationStatus.Shortlisted ? "SHORTLISTED" : "Not Shortlisted";

            try
            {
                var bodyBuilder = new BodyBuilder
                {
                    HtmlBody = $@"
                        <h2>New Career Application</h2>
                        <p><strong>Name:</strong> {application.FullName}</p>
                        <p><strong>Phone:</strong> {application.PhoneNumber}</p>
                        <p><strong>Email:</strong> {application.Email}</p>
                        <p><strong>Desired Position:</strong> {application.DesiredPosition}</p>
                        <p><strong>Keyword Score:</strong> {application.KeywordScore} — <strong>{scoreLabel}</strong></p>
                        <p><strong>Cover Letter:</strong><br/>{application.CoverLetter ?? "(none provided)"}</p>
                        <p><strong>Submitted:</strong> {application.SubmittedAt:dd MMM yyyy, HH:mm} UTC</p>
                        <p>{attachmentPaths.Count} file(s) attached — CV plus any supporting documents.</p>"
                };
                //Validate and attach files (CV and supporting docs) to the email
                foreach (var path in attachmentPaths)
                {
                    if (File.Exists(path))
                    {
                        await bodyBuilder.Attachments.AddAsync(path);
                    }
                    else
                    {
                        _logger.LogWarning("Attachment path not found, skipping: {Path}", path);
                    }
                }

                message.Body = bodyBuilder.ToMessageBody();
                //Set up a cancellation token to enforce a 15-second timeout on the SMTP operation
                using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(15));
                using var client = new SmtpClient();

                // Port 465 = implicit SSL from the start of the connection.
                // Port 587 = plain connection that upgrades via STARTTLS.
                // Using the wrong option for the configured port causes an
                // immediate handshake failure.
                var secureOption = smtpPort == 465
                    ? SecureSocketOptions.SslOnConnect
                    : SecureSocketOptions.StartTls;

                await client.ConnectAsync(smtpHost, smtpPort, secureOption, cts.Token);
                await client.AuthenticateAsync(smtpUser, smtpPass, cts.Token);
                await client.SendAsync(message, cts.Token);
                await client.DisconnectAsync(true, cts.Token);
            }
            catch (OperationCanceledException)
            {
                _logger.LogError("SMTP send timed out after 15 seconds — check host/port/firewall settings.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send career application email via SMTP.");
            }
        }
    }
}