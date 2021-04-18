using Expensely.Common.Primitives.Result;
using Expensely.Domain.Modules.Transactions.Contracts;

namespace Expensely.Domain.Modules.Transactions
{
    /// <summary>
    /// Represents the transaction details validator interface.
    /// </summary>
    public interface ITransactionDetailsValidator
    {
        /// <summary>
        /// Validates the provided transaction information and returns the result of the validation.
        /// </summary>
        /// <param name="validateTransactionDetailsRequest">The validate transaction details request.</param>
        /// <returns>The result of the transaction validation process containing the transaction details or an error.</returns>
        Result<ITransactionDetails> Validate(ValidateTransactionDetailsRequest validateTransactionDetailsRequest);
    }
}
