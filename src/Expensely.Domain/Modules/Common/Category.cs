using Expensely.Domain.Primitives;

namespace Expensely.Domain.Modules.Common
{
    /// <summary>
    /// Represents the category enumeration.
    /// </summary>
    public class Category : Enumeration<Category>
    {
        /// <summary>
        /// The none category.
        /// </summary>
        public static readonly Category None = new(1, "None");

        /// <summary>
        /// The shopping expense category.
        /// </summary>
        public static readonly Category Shopping = new ExpenseCategory(2, "Shopping");

        /// <summary>
        /// The groceries expense category.
        /// </summary>
        public static readonly Category Groceries = new ExpenseCategory(3, "Groceries");

        /// <summary>
        /// The food expense category.
        /// </summary>
        public static readonly Category Food = new ExpenseCategory(4, "Food");

        /// <summary>
        /// The drinks expense category.
        /// </summary>
        public static readonly Category Drinks = new ExpenseCategory(5, "Drinks");

        /// <summary>
        /// The clothing expense category.
        /// </summary>
        public static readonly Category Clothing = new ExpenseCategory(6, "Clothing");

        /// <summary>
        /// The travel expense category.
        /// </summary>
        public static readonly Category Travel = new ExpenseCategory(7, "Travel");

        /// <summary>
        /// The bills expense category.
        /// </summary>
        public static readonly Category Bills = new ExpenseCategory(8, "Bills");

        /// <summary>
        /// The cash income category.
        /// </summary>
        public static readonly Category Cash = new IncomeCategory(100, "Cash");

        /// <summary>
        /// The salary income category.
        /// </summary>
        public static readonly Category Salary = new IncomeCategory(101, "Salary");

        /// <summary>
        /// Initializes a new instance of the <see cref="Category"/> class.
        /// </summary>
        /// <param name="value">The currency value.</param>
        /// <param name="name">The currency name.</param>
        protected Category(int value, string name)
            : base(value, name.ToLower()) =>
            IsDefault = true;

        /// <summary>
        /// Initializes a new instance of the <see cref="Category"/> class.
        /// </summary>
        protected Category()
        {
        }

        /// <summary>
        /// Gets a value indicating whether or not the category is the default category.
        /// </summary>
        public bool IsDefault { get; private init; }

        /// <summary>
        /// Gets a value indicating whether or not the category is an expense.
        /// </summary>
        public bool IsExpense { get; private init; }

        private sealed class ExpenseCategory : Category
        {
            public ExpenseCategory(int value, string name)
                : base(value, name)
            {
                IsDefault = false;

                IsExpense = true;
            }
        }

        private sealed class IncomeCategory : Category
        {
            public IncomeCategory(int value, string name)
                : base(value, name)
            {
                IsDefault = false;

                IsExpense = false;
            }
        }
    }
}
