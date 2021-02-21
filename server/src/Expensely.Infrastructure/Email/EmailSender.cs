using System.Net;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Abstractions.Email;
using Expensely.Application.Contracts.Email;
using Microsoft.Extensions.Options;

namespace Expensely.Infrastructure.Email
{
    /// <summary>
    /// Represents the email sender.
    /// </summary>
    public sealed class EmailSender : IEmailSender
    {
        private readonly EmailSettings _settings;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmailSender"/> class.
        /// </summary>
        /// <param name="emailSettingsOptions">The email settings options.</param>
        public EmailSender(IOptions<EmailSettings> emailSettingsOptions) => _settings = emailSettingsOptions.Value;

        /// <inheritdoc />
        public async Task SendAsync(MailRequest mailRequest, CancellationToken cancellationToken = default)
        {
            using var smtpClient = new SmtpClient(_settings.Host, _settings.Port)
            {
                EnableSsl = _settings.EnableSsl,
                Credentials = new NetworkCredential(_settings.SenderEmail, _settings.Password)
            };

            using var message = new MailMessage
            {
                From = new MailAddress(_settings.SenderEmail, _settings.DisplayName),
                Subject = mailRequest.Subject,
                Body = mailRequest.Body,
                IsBodyHtml = mailRequest.IsBodyHtml
            };

            message.To.Add(mailRequest.RecipientEmail);

            await smtpClient.SendMailAsync(message, cancellationToken);
        }
    }
}
