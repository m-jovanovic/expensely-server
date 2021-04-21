using System;
using System.Collections.Generic;
using System.Linq;

namespace Expensely.Domain.Primitives
{
    /// <summary>
    /// Represents the base class all value objects derive from.
    /// </summary>
    public abstract class ValueObject : IEquatable<ValueObject>
    {
        public static bool operator ==(ValueObject a, ValueObject b)
        {
            if (a is null && b is null)
            {
                return true;
            }

            if (a is null || b is null)
            {
                return false;
            }

            return a.Equals(b);
        }

        public static bool operator !=(ValueObject a, ValueObject b) => !(a == b);

        /// <inheritdoc />
        public bool Equals(ValueObject other)
        {
            if (other is null)
            {
                return false;
            }

            return GetAtomicValues().SequenceEqual(other.GetAtomicValues());
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (obj is null)
            {
                return false;
            }

            if (GetType() != obj.GetType())
            {
                return false;
            }

            if (obj is not ValueObject valueObject)
            {
                return false;
            }

            return GetAtomicValues().SequenceEqual(valueObject.GetAtomicValues());
        }

        /// <inheritdoc />
        public override int GetHashCode() =>
            GetAtomicValues().Aggregate(default(int), (hashcode, value) => HashCode.Combine(hashcode, value.GetHashCode()));

        /// <summary>
        /// Gets the atomic values of the value object.
        /// </summary>
        /// <returns>The collection of objects representing the value object values.</returns>
        protected abstract IEnumerable<object> GetAtomicValues();
    }
}
