﻿using System;
using Expensely.Domain.Modules.Common;
using Expensely.Domain.Modules.Transactions;
using Expensely.Domain.Modules.Users;
using Xunit;

namespace Expensely.Domain.UnitTests.TestData.Transactions
{
    public class CreateTransactionArgumentNullExceptionArguments : TheoryData<User, ITransactionDetails, string>
    {
        public CreateTransactionArgumentNullExceptionArguments()
        {
            Add(null, new TransactionDetails(), "user");

            User user = UserTestData.ValidUser;

            Add(
                user,
                new TransactionDetails
                {
                    Description = null
                },
                "description");

            Add(
                user,
                new TransactionDetails
                {
                    Description = DescriptionTestData.EmptyDescription,
                    Category = null
                },
                "category");

            Add(
                user,
                new TransactionDetails
                {
                    Description = DescriptionTestData.EmptyDescription,
                    Category = Category.None,
                    Money = null
                },
                "money");

            Add(
                user,
                new TransactionDetails
                {
                    Description = DescriptionTestData.EmptyDescription,
                    Category = Category.None,
                    Money = new Money(default, CurrencyTestData.DefaultCurrency),
                    OccurredOn = DateTime.UtcNow,
                    TransactionType = null
                },
                "transactionType");
        }
    }
}
