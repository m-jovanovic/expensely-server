using System;
using System.Collections.Generic;
using System.Linq;
using Expensely.Domain.Modules.Budgets.Contracts;
using Expensely.Domain.Modules.Budgets.Exceptions;
using Expensely.Domain.Modules.Common;
using Expensely.Domain.Modules.Users;
using Expensely.Domain.Primitives;
using Expensely.Domain.Utility;

namespace Expensely.Domain.Modules.Budgets
{
    /// <summary>
    /// Represents the budget.
    /// </summary>
    public sealed class Budget : Entity, IAuditableEntity
    {
        private readonly HashSet<Category> _categories = new();

        /// <summary>
        /// Initializes a new instance of the <see cref="Budget"/> class.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="name">The name of the budget.</param>
        /// <param name="money">The monetary amount of the budget.</param>
        /// <param name="categories">The categories for the budget.</param>
        /// <param name="startDate">The start date of the budget.</param>
        /// <param name="endDate">The end date of the budget.</param>
        private Budget(User user, Name name, Money money, IReadOnlyCollection<Category> categories, DateTime startDate, DateTime endDate)
            : base(Ulid.NewUlid())
        {
            EnsureValidDetails(user, name, money, categories, startDate, endDate);

            UserId = Ulid.Parse(user.Id);
            Name = name;
            Money = money;
            StartDate = startDate.Date;
            EndDate = endDate.Date;
            ChangeCategories(categories);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Budget"/> class.
        /// </summary>
        /// <remarks>
        /// Required for deserialization.
        /// </remarks>
        private Budget()
        {
        }

        /// <summary>
        /// Gets the user identifier.
        /// </summary>
        public Ulid UserId { get; private set; }

        /// <summary>
        /// Gets the name.
        /// </summary>
        public Name Name { get; private set; }

        /// <summary>
        /// Gets the money.
        /// </summary>
        public Money Money { get; private set; }

        /// <summary>
        /// Gets the categories.
        /// </summary>
        public IReadOnlyCollection<Category> Categories => _categories.ToList();

        /// <summary>
        /// Gets the start date.
        /// </summary>
        public DateTime StartDate { get; private set; }

        /// <summary>
        /// Gets the end date.
        /// </summary>
        public DateTime EndDate { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the budget has expired.
        /// </summary>
        public bool Expired { get; private set; }

        /// <inheritdoc />
        public DateTime CreatedOnUtc { get; private set; }

        /// <inheritdoc />
        public DateTime? ModifiedOnUtc { get; private set; }

        /// <summary>
        /// Creates a new budget for the specified user and budget details.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="budgetDetails">The budget details.</param>
        /// <returns>The newly created budget.</returns>
        internal static Budget Create(User user, IBudgetDetails budgetDetails)
        {
            var budget = new Budget(
                user,
                budgetDetails.Name,
                budgetDetails.Money,
                budgetDetails.Categories,
                budgetDetails.StartDate,
                budgetDetails.EndDate);

            return budget;
        }

        /// <summary>
        /// Updates the budget with the specified budget details.
        /// </summary>
        /// <param name="budgetDetails">The budget details.</param>
        public void ChangeDetails(IBudgetDetails budgetDetails)
        {
            EnsureBudgetDetailsValid(
                budgetDetails.Name,
                budgetDetails.Money,
                budgetDetails.Categories,
                budgetDetails.StartDate,
                budgetDetails.EndDate);

            Name = budgetDetails.Name;
            StartDate = budgetDetails.StartDate;
            EndDate = budgetDetails.EndDate;
            ChangeCategories(budgetDetails.Categories);
        }

        private static void EnsureValidDetails(
            User user,
            Name name,
            Money money,
            IReadOnlyCollection<Category> categories,
            DateTime startDate,
            DateTime endDate)
        {
            Ensure.NotNull(user, "The user is required.", nameof(user));

            EnsureBudgetDetailsValid(name, money, categories, startDate, endDate);
        }

        private static void EnsureBudgetDetailsValid(
            Name name,
            Money money,
            IReadOnlyCollection<Category> categories,
            DateTime startDate,
            DateTime endDate)
        {
            Ensure.NotEmpty(name, "The name is required.", nameof(name));
            Ensure.NotNull(money, "The monetary amount is required.", nameof(money));
            Ensure.NotNull(categories, "The categories are required", nameof(categories));
            Ensure.NotEmpty(startDate, "The start date is required.", nameof(startDate));
            Ensure.NotEmpty(endDate, "The end date is required.", nameof(endDate));
            EnsureMoneyIsGreaterThanZero(money);
            EnsureStartDatePrecedesEndDate(startDate, endDate);
        }

        private static void EnsureStartDatePrecedesEndDate(DateTime startDate, DateTime endDate)
        {
            if (endDate < startDate)
            {
                throw new BudgetEndDatePrecedesStartDateDomainException(startDate, endDate);
            }
        }

        private static void EnsureMoneyIsGreaterThanZero(Money money) =>
            Ensure.NotLessThanOrEqualToZero(money.Amount, "The monetary amount must be greater than zero", nameof(money));

        /// <summary>
        /// Changes the categories.
        /// </summary>
        /// <param name="categories">The categories.</param>
        private void ChangeCategories(IEnumerable<Category> categories)
        {
            _categories.Clear();

            foreach (Category category in categories)
            {
                _categories.Add(category);
            }
        }
    }
}
