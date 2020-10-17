using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Abstractions.Authentication;
using Expensely.Application.Abstractions.Data;
using Expensely.Application.Abstractions.Messaging;
using Expensely.Application.Validation;
using Expensely.Domain.Core;
using Expensely.Domain.Primitives.Result;
using Microsoft.EntityFrameworkCore;

namespace Expensely.Application.Users.Commands.CreateUser
{
    /// <summary>
    /// Represents the <see cref="CreateUserCommand"/> handler.
    /// </summary>
    internal sealed class CreateUserCommandHandler : ICommandHandler<CreateUserCommand, Result<string>>
    {
        private readonly IDbContext _dbContext;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IJwtProvider _jwtProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateUserCommandHandler"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <param name="jwtProvider">The JWT provider.</param>
        public CreateUserCommandHandler(IDbContext dbContext, IUnitOfWork unitOfWork, IJwtProvider jwtProvider)
        {
            _dbContext = dbContext;
            _unitOfWork = unitOfWork;
            _jwtProvider = jwtProvider;
        }

        /// <inheritdoc />
        public async Task<Result<string>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            Result<Email> emailResult = Email.Create(request.Email);

            if (emailResult.IsFailure)
            {
                return Result.Failure<string>(emailResult.Error);
            }

            Email email = emailResult.Value;

            bool emailAlreadyExists = await _dbContext.Set<User>().AnyAsync(u => u.Email.Value == email, cancellationToken);

            if (emailAlreadyExists)
            {
                return Result.Failure<string>(Errors.User.EmailAlreadyInUse);
            }

            var user = new User(email);

            _dbContext.Insert(user);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            string token = _jwtProvider.CreateToken(user);

            return token;
        }
    }
}
