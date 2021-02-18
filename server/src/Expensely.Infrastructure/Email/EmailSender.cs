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
        private readonly EmailSettings _emailSettings;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmailSender"/> class.
        /// </summary>
        /// <param name="emailSettingsOptions">The email settings options.</param>
        public EmailSender(IOptions<EmailSettings> emailSettingsOptions) => _emailSettings = emailSettingsOptions.Value;

        /// <inheritdoc />
        public async Task SendAsync(MailRequest mailRequest, CancellationToken cancellationToken = default)
        {
            using var smtpClient = new SmtpClient(_emailSettings.Host, _emailSettings.Port)
            {
                EnableSsl = _emailSettings.EnableSsl,
                Credentials = new NetworkCredential(_emailSettings.SenderEmail, _emailSettings.Password)
            };

            using var message = new MailMessage
            {
                From = new MailAddress(_emailSettings.SenderEmail, _emailSettings.DisplayName),
                Subject = mailRequest.Subject,
                Body = mailRequest.Body,
                IsBodyHtml = mailRequest.IsBodyHtml
            };

            message.To.Add(mailRequest.RecipientEmail);

            await smtpClient.SendMailAsync(message, cancellationToken);
        }
    }
}
