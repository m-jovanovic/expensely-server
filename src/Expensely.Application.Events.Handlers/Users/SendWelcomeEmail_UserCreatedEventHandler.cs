using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Abstractions.Notification;
using Expensely.Application.Contracts.Notification;
using Expensely.Common.Primitives.Maybe;
using Expensely.Domain.Abstractions;
using Expensely.Domain.Modules.Users;
using Expensely.Domain.Modules.Users.Events;

namespace Expensely.Application.Events.Handlers.Users
{
    /// <summary>
    /// Represents the <see cref="UserCreatedEvent"/> handler.
    /// </summary>
    public sealed class SendWelcomeEmail_UserCreatedEventHandler : EventHandler<UserCreatedEvent>
    {
        private readonly IUserRepository _userRepository;
        private readonly IEmailSender _emailSender;

        /// <summary>
        /// Initializes a new instance of the <see cref="SendWelcomeEmail_UserCreatedEventHandler"/> class.
        /// </summary>
        /// <param name="userRepository">The user repository.</param>
        /// <param name="emailSender">The email sender.</param>
        public SendWelcomeEmail_UserCreatedEventHandler(IUserRepository userRepository, IEmailSender emailSender)
        {
            _userRepository = userRepository;
            _emailSender = emailSender;
        }

        /// <inheritdoc />
        public override async Task Handle(UserCreatedEvent @event, CancellationToken cancellationToken = default)
        {
            Maybe<User> maybeUser = await _userRepository.GetByIdAsync(@event.UserId, cancellationToken);

            if (maybeUser.HasNoValue)
            {
                return;
            }

            User user = maybeUser.Value;

            var mailRequest = new MailRequest
            {
                RecipientEmail = user.Email,
                Subject = "Welcome to Expensely! 🎉",
                Body = CreateEmailBody(user)
            };

            await _emailSender.SendAsync(mailRequest, cancellationToken);
        }

        private static string CreateEmailBody(User user)
        {
            var stringBuilder = new StringBuilder();

            stringBuilder.AppendLine($"Hello {user.GetFullName()} and welcome to Expensely!");
            stringBuilder.AppendLine();
            stringBuilder.AppendLine("We honestly hope that you will enjoy everything that we have to offer.");
            stringBuilder.AppendLine();
            stringBuilder.AppendLine("Sincerely,");
            stringBuilder.AppendLine("The Expensely team");

            return stringBuilder.ToString();
        }
    }
}
