namespace Expensely.Domain.Modules.Users
{
    /// <summary>
    /// Represents the password hasher interface.
    /// </summary>
    public interface IPasswordHasher
    {
        /// <summary>
        /// Hashes the specified password and returns the hash.
        /// </summary>
        /// <param name="password">The password to be hashed.</param>
        /// <returns>The password hash.</returns>
        string Hash(Password password);

        /// <summary>
        /// Checks if the specified password's hash matches the specified password hash.
        /// </summary>
        /// <param name="password">The password to check.</param>
        /// <param name="passwordHash">The password hash.</param>
        /// <returns>True if the password's hash matches the provided password hash, otherwise false.</returns>
        bool HashesMatch(Password password, string passwordHash);
    }
}
