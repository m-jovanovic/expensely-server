using System;

namespace Expensely.Domain.Primitives
{
    /// <summary>
    /// Represents the base class that all entities derive from.
    /// </summary>
    public abstract class Entity : IEquatable<Entity>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Entity"/> class.
        /// </summary>
        /// <param name="id">The entity identifier.</param>
        protected Entity(Ulid id)
            : this()
        {
            if (id == Ulid.Empty)
            {
                throw new ArgumentException("The entity identifier is required.", nameof(id));
            }

            Id = id.ToString();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Entity"/> class.
        /// </summary>
        /// <remarks>
        /// Required for deserialization.
        /// </remarks>
        protected Entity()
        {
        }

        /// <summary>
        /// Gets the entity identifier.
        /// </summary>
        public string Id { get; private set; }

        public static bool operator ==(Entity a, Entity b)
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

        public static bool operator !=(Entity a, Entity b) => !(a == b);

        /// <inheritdoc />
        public bool Equals(Entity other)
        {
            if (other is null)
            {
                return false;
            }

            if (other.GetType() != GetType())
            {
                return false;
            }

            return ReferenceEquals(this, other) || Id == other.Id;
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (obj is null)
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj is not Entity other)
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(Id) || string.IsNullOrWhiteSpace(other.Id))
            {
                return false;
            }

            return Id == other.Id;
        }

        /// <inheritdoc />
        public override int GetHashCode() => Id.GetHashCode(StringComparison.InvariantCulture) * 41;
    }
}
