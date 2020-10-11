﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Expensely.Domain.Primitives
{
    /// <summary>
    /// Represents the base class all value objects derive from.
    /// </summary>
    public abstract class ValueObject : IEquatable<ValueObject>
    {
        /// <summary>
        /// Determines whether the specified value objects are equal.
        /// </summary>
        /// <param name="a">The first value object.</param>
        /// <param name="b">The second value object.</param>
        /// <returns>True if the value objects are equal, otherwise false.</returns>
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

        /// <summary>
        /// Determines whether the specified value objects are not equal.
        /// </summary>
        /// <param name="a">The first value object.</param>
        /// <param name="b">The second value object.</param>
        /// <returns>True if the value objects are not equal, otherwise false.</returns>
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
            if (obj == null)
            {
                return false;
            }

            if (GetType() != obj.GetType())
            {
                return false;
            }

            if (!(obj is ValueObject valueObject))
            {
                return false;
            }

            return GetAtomicValues().SequenceEqual(valueObject.GetAtomicValues());
        }

        /// <inheritdoc />
        public override int GetHashCode() =>
            GetAtomicValues()
                .Aggregate(default(HashCode), (hashCode, obj) =>
                {
                    hashCode.Add(obj.GetHashCode());

                    return hashCode;
                }).ToHashCode();

        /// <summary>
        /// Gets the atomic values of the value object.
        /// </summary>
        /// <returns>The collection of objects representing the value object values.</returns>
        protected abstract IEnumerable<object> GetAtomicValues();
    }
}
