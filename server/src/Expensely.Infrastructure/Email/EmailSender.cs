using System.Net;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Abstractions.Email;
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
        public async Task SendAsync(
            string recipient,
            string subject,
            string body,
            bool isBodyHtml = false,
            CancellationToken cancellationToken = default)
        {
            using var smtpClient = new SmtpClient(_emailSettings.Host, _emailSettings.Port)
            {
                EnableSsl = _emailSettings.EnableSsl,
                Credentials = new NetworkCredential(_emailSettings.Email, _emailSettings.Password)
            };

            using var message = new MailMessage
            {
                From = new MailAddress(_emailSettings.Email, _emailSettings.DisplayName),
                Subject = subject,
                Body = body,
                IsBodyHtml = isBodyHtml
            };

            message.To.Add(recipient);

            await smtpClient.SendMailAsync(message, cancellationToken);
        }
    }
}
