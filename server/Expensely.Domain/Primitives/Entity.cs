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
        /// <param name="id">The identifier.</param>
        protected Entity(int id) => Id = id;

        /// <summary>
        /// Initializes a new instance of the <see cref="Entity"/> class.
        /// </summary>
        /// <remarks>
        /// Required by EF Core.
        /// </remarks>
        protected Entity()
        {
        }

        /// <summary>
        /// Gets the entity identifier.
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// Determines whether the specified entities are equal.
        /// </summary>
        /// <param name="a">The first entity.</param>
        /// <param name="b">The second entity.</param>
        /// <returns>True if the entities are equal, otherwise false.</returns>
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

        /// <summary>
        /// Determines whether the specified entities are not equal.
        /// </summary>
        /// <param name="a">The first entity.</param>
        /// <param name="b">The second entity.</param>
        /// <returns>True if the entities are not equal, otherwise false.</returns>
        public static bool operator !=(Entity a, Entity b) => !(a == b);

        /// <inheritdoc />
        public bool Equals(Entity other)
        {
            if (other is null)
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

            if (obj.GetType() != GetType())
            {
                return false;
            }

            if (!(obj is Entity other))
            {
                return false;
            }

            if (Id == default || other.Id == default)
            {
                return false;
            }

            return Id == other.Id;
        }

        /// <inheritdoc />
        public override int GetHashCode() => Id.GetHashCode() * 41;
    }
}
