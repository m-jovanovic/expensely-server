using System.Net;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Abstractions.Notification;
using Expensely.Application.Contracts.Notification;
using Expensely.Common.Primitives.ServiceLifetimes;
using Microsoft.Extensions.Options;

namespace Expensely.Notification.Email
{
    /// <summary>
    /// Represents the email sender.
    /// </summary>
    public sealed class EmailSender : IEmailSender, ITransient
    {
        private readonly EmailOptions _options;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmailSender"/> class.
        /// </summary>
        /// <param name="options">The email options.</param>
        public EmailSender(IOptions<EmailOptions> options) => _options = options.Value;

        /// <inheritdoc />
        public async Task SendAsync(MailRequest mailRequest, CancellationToken cancellationToken = default)
        {
            using var smtpClient = new SmtpClient(_options.Host, _options.Port)
            {
                EnableSsl = _options.EnableSsl,
                Credentials = new NetworkCredential(_options.SenderEmail, _options.Password)
            };

            using var message = new MailMessage
            {
                From = new MailAddress(_options.SenderEmail, _options.DisplayName),
                Subject = mailRequest.Subject,
                Body = mailRequest.Body,
                IsBodyHtml = mailRequest.IsBodyHtml
            };

            message.To.Add(mailRequest.RecipientEmail);

            await smtpClient.SendMailAsync(message, cancellationToken);
        }
    }
}
