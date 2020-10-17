using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Abstractions.Authentication;
using Expensely.Application.Abstractions.Data;
using Expensely.Application.Abstractions.Messaging;
using Expensely.Application.Validation;
using Expensely.Domain.Core;
using Expensely.Domain.Primitives.Maybe;
using Expensely.Domain.Primitives.Result;
using Microsoft.EntityFrameworkCore;

namespace Expensely.Application.Users.Commands.CreateTokenForUser
{
    /// <summary>
    /// Represents the <see cref="CreateTokenForUserCommand"/> handler.
    /// </summary>
    internal sealed class CreateTokenForUserCommandHandler : ICommandHandler<CreateTokenForUserCommand, Result<string>>
    {
        private readonly IDbContext _dbContext;
        private readonly IJwtProvider _jwtProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateTokenForUserCommandHandler"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        /// <param name="jwtProvider">The JWT provider.</param>
        public CreateTokenForUserCommandHandler(IDbContext dbContext, IJwtProvider jwtProvider)
        {
            _dbContext = dbContext;
            _jwtProvider = jwtProvider;
        }

        /// <inheritdoc />
        public async Task<Result<string>> Handle(CreateTokenForUserCommand request, CancellationToken cancellationToken)
        {
            Result<Email> emailResult = Email.Create(request.Email);

            if (emailResult.IsFailure)
            {
                return Result.Failure<string>(emailResult.Error);
            }

            Email email = emailResult.Value;

            Maybe<User> maybeUser = await _dbContext.Set<User>().FirstOrDefaultAsync(u => u.Email.Value == email, cancellationToken);

            if (maybeUser.HasNoValue)
            {
                return Result.Failure<string>(Errors.User.InvalidEmail);
            }

            string token = _jwtProvider.CreateToken(maybeUser.Value);

            return token;
        }
    }
}
