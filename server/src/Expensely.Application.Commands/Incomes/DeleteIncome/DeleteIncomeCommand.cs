using System;
using Expensely.Common.Abstractions.Messaging;
using Expensely.Domain.Abstractions.Result;

namespace Expensely.Application.Commands.Incomes.DeleteIncome
{
    /// <summary>
    /// Represents the command for deleting an income.
    /// </summary>
    public sealed class DeleteIncomeCommand : ICommand<Result>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteIncomeCommand"/> class.
        /// </summary>
        /// <param name="incomeId">The income identifier.</param>
        public DeleteIncomeCommand(Guid incomeId) => IncomeId = incomeId;

        /// <summary>
        /// Gets the income identifier.
        /// </summary>
        public Guid IncomeId { get; }
    }
}
