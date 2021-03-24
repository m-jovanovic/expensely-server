using System;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Expensely.App.ModelBinders.Ulid
{
    /// <summary>
    /// Represents the <see cref="UlidModelBinder"/> provider.
    /// </summary>
    public sealed class UlidModelBinderProvider : IModelBinderProvider
    {
        /// <inheritdoc />
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (context.Metadata.ModelType == typeof(System.Ulid))
            {
                return new UlidModelBinder();
            }

            return null;
        }
    }
}
