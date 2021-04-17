using System;
using Expensely.Domain.Modules.Users;

namespace Expensely.Domain.Modules.Budgets.Contracts
{
    /// <summary>
    /// Represents the request for creating a new budget.
    /// </summary>
    public sealed record CreateBudgetRequest(
        User User,
        string Name,
        int[] Categories,
        decimal Amount,
        int Currency,
        DateTime StartDate,
        DateTime EndDate);
}
