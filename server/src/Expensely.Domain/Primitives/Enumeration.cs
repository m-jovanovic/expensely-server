﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Expensely.Common.Primitives.Maybe;

namespace Expensely.Domain.Primitives
{
    /// <summary>
    /// Represents an enumeration of objects with a unique numeric value and a name.
    /// </summary>
    /// <typeparam name="TEnum">The type of the enumeration.</typeparam>
    public abstract class Enumeration<TEnum> : IEquatable<Enumeration<TEnum>>, IComparable<Enumeration<TEnum>>
        where TEnum : Enumeration<TEnum>
    {
        private static readonly Lazy<Dictionary<int, TEnum>> EnumerationsDictionary = new(
            () => GetAllEnumerationOptions(typeof(TEnum)).ToDictionary(enumeration => enumeration.Value));

        /// <summary>
        /// Initializes a new instance of the <see cref="Enumeration{TEnum}"/> class.
        /// </summary>
        /// <param name="value">The enumeration value.</param>
        /// <param name="name">The enumeration name.</param>
        protected Enumeration(int value, string name)
        {
            Value = value;
            Name = name;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Enumeration{TEnum}"/> class.
        /// </summary>
        /// <remarks>
        /// Required for deserialization.
        /// </remarks>
        protected Enumeration()
        {
        }

        /// <summary>
        /// Gets the enumeration values.
        /// </summary>
        /// <returns>The read-only collection of enumeration values.</returns>
        public static IReadOnlyCollection<TEnum> List => EnumerationsDictionary.Value.Values.ToList();

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        public int Value { get; protected set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; protected set; }

        public static bool operator ==(Enumeration<TEnum> a, Enumeration<TEnum> b)
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

        public static bool operator !=(Enumeration<TEnum> a, Enumeration<TEnum> b) => !(a == b);

        /// <summary>
        /// Creates an enumeration of the specified type based on the specified value.
        /// </summary>
        /// <param name="value">The enumeration value.</param>
        /// <returns>The enumeration instance that matches the specified value.</returns>
        public static Maybe<TEnum> FromValue(int value) =>
            EnumerationsDictionary.Value.TryGetValue(value, out TEnum enumeration) ? enumeration : Maybe<TEnum>.None;

        /// <summary>
        /// Checks if the enumeration with the specified value exists.
        /// </summary>
        /// <param name="value">The enumeration value.</param>
        /// <returns>True if an enumeration with the specified value exists, otherwise false.</returns>
        public static bool ContainsValue(int value) => EnumerationsDictionary.Value.ContainsKey(value);

        /// <inheritdoc />
        public bool Equals(Enumeration<TEnum> other)
        {
            if (other is null)
            {
                return false;
            }

            return GetType() == other.GetType() && other.Value.Equals(Value);
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

            if (obj is not Enumeration<TEnum> otherValue)
            {
                return false;
            }

            return otherValue.Value.Equals(Value);
        }

        /// <inheritdoc />
        public int CompareTo(Enumeration<TEnum> other) => other is null ? 1 : Value.CompareTo(other.Value);

        /// <inheritdoc />
        public override int GetHashCode() => Value.GetHashCode();

        private static IEnumerable<TEnum> GetAllEnumerationOptions(Type enumType) =>
            Assembly
                .GetAssembly(enumType)?
                .GetTypes()
                .Where(enumType.IsAssignableFrom)
                .SelectMany(GetFieldsOfType<TEnum>);

        private static List<TFieldType> GetFieldsOfType<TFieldType>(Type type) =>
            type.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
                .Where(fieldInfo => type.IsAssignableFrom(fieldInfo.FieldType))
                .Select(fieldInfo => (TFieldType)fieldInfo.GetValue(null))
                .ToList();
    }
}
