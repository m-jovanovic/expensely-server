using System;
using System.Collections.Generic;
using System.Linq;
using Expensely.Common.Primitives.Result;
using Expensely.Domain.Errors;
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
        public Budget(User user, Name name, Money money, IReadOnlyCollection<Category> categories, DateTime startDate, DateTime endDate)
            : base(Ulid.NewUlid())
        {
            Ensure.NotNull(user, "The user is required.", nameof(user));
            Ensure.NotEmpty(name, "The name is required.", nameof(name));
            Ensure.NotNull(money, "The monetary amount is required.", nameof(money));
            EnsureMoneyIsGreaterThanZero(money);
            Ensure.NotNull(categories, "The categories are required", nameof(categories));
            Ensure.NotEmpty(startDate, "The start date is required.", nameof(startDate));
            Ensure.NotEmpty(endDate, "The end date is required.", nameof(endDate));
            EnsureStartDatePrecedesEndDate(startDate, endDate);

            UserId = Ulid.Parse(user.Id);
            Name = name;
            Money = money;
            StartDate = startDate.Date;
            EndDate = endDate.Date;

            foreach (Category category in categories)
            {
                AddCategory(category);
            }
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
        /// Changes the name of the budget.
        /// </summary>
        /// <param name="name">The new name.</param>
        public void ChangeName(Name name)
        {
            if (name == Name)
            {
                return;
            }

            Name = name;
        }

        /// <summary>
        /// Changes the monetary amount of the budget.
        /// </summary>
        /// <param name="money">The new money amount.</param>
        public void ChangeMoney(Money money)
        {
            EnsureMoneyIsGreaterThanZero(money);

            if (Money == money)
            {
                return;
            }

            Money = money;
        }

        /// <summary>
        /// Changes the dates of the budget.
        /// </summary>
        /// <param name="startDate">The new start date.</param>
        /// <param name="endDate">The new end date.</param>
        public void ChangeDates(DateTime startDate, DateTime endDate)
        {
            EnsureStartDatePrecedesEndDate(startDate, endDate);

            if (startDate == StartDate && endDate == EndDate)
            {
                return;
            }

            StartDate = startDate;
            EndDate = endDate;
        }

        /// <summary>
        /// Marks the budget as expired.
        /// </summary>
        /// <exception cref="BudgetAlreadyExpiredDomainException"> when the budget is already expired.</exception>
        public void MarkAsExpired()
        {
            if (Expired)
            {
                throw new BudgetAlreadyExpiredDomainException();
            }

            Expired = true;
        }

        /// <summary>
        /// Adds the specified category to the budget.
        /// </summary>
        /// <param name="category">The category to be added.</param>
        /// <returns>The success result if the category was added, otherwise an error result.</returns>
        public Result AddCategory(Category category) =>
            _categories.Add(category) ? Result.Success() : Result.Failure(DomainErrors.Budget.CategoryAlreadyExists);

        /// <summary>
        /// Removes the specified category from the budget.
        /// </summary>
        /// <param name="category">The category to be removed.</param>
        /// <returns>The success result if the category was removed, otherwise an error result.</returns>
        public Result RemoveCategory(Category category) =>
            _categories.Remove(category) ? Result.Success() : Result.Failure(DomainErrors.Budget.CategoryDoesNotExist);

        /// <summary>
        /// Ensures that the specified start date precedes the specified end date, otherwise throws an exception.
        /// </summary>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <exception cref="BudgetEndDatePrecedesStartDateDomainException"> when the end date precedes the start date.</exception>
        private static void EnsureStartDatePrecedesEndDate(DateTime startDate, DateTime endDate)
        {
            if (endDate < startDate)
            {
                throw new BudgetEndDatePrecedesStartDateDomainException(startDate, endDate);
            }
        }

        /// <summary>
        /// Ensures that the specified money amount is greater than zero.
        /// </summary>
        /// <param name="money">The monetary amount.</param>
        /// <exception cref="ArgumentException"> if the monetary amount is less than or equal to zero.</exception>
        private static void EnsureMoneyIsGreaterThanZero(Money money) =>
            Ensure.NotLessThanOrEqualToZero(money.Amount, "The monetary amount must be greater than zero", nameof(money));
    }
}
