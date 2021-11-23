using System;
using System.Security.Cryptography;
using Expensely.Common.Primitives.ServiceLifetimes;
using Expensely.Domain.Modules.Users;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace Expensely.Infrastructure.Cryptography
{
    /// <summary>
    /// Represents the password hasher.
    /// </summary>
    public sealed class PasswordHasher : IPasswordHasher, IDisposable, ITransient
    {
        private const KeyDerivationPrf Prf = KeyDerivationPrf.HMACSHA256;
        private const int IterationCount = 10000;
        private const int NumberOfBytesRequested = 256 / 8;
        private const int SaltSize = 128 / 8;
        private readonly RandomNumberGenerator _rng;

        /// <summary>
        /// Initializes a new instance of the <see cref="PasswordHasher"/> class.
        /// </summary>
        public PasswordHasher() => _rng = RandomNumberGenerator.Create(nameof(RandomNumberGenerator));

        /// <inheritdoc />
        public string Hash(Password password)
        {
            if (password is null)
            {
                throw new ArgumentNullException(nameof(password));
            }

            string hashedPassword = Convert.ToBase64String(HashPasswordInternal(password));

            return hashedPassword;
        }

        /// <inheritdoc />
        public bool HashesMatch(Password password, string passwordHash)
        {
            if (password is null)
            {
                throw new ArgumentNullException(nameof(password));
            }

            if (passwordHash is null)
            {
                throw new ArgumentNullException(nameof(passwordHash));
            }

            byte[] decodedHashedPassword = Convert.FromBase64String(passwordHash);

            if (decodedHashedPassword.Length == 0)
            {
                return false;
            }

            bool verified = VerifyPasswordHashInternal(decodedHashedPassword, password);

            return verified;
        }

        /// <inheritdoc />
        public void Dispose() => _rng?.Dispose();

        private static bool VerifyPasswordHashInternal(byte[] hashedPassword, string password)
        {
            try
            {
                byte[] salt = new byte[SaltSize];

                Buffer.BlockCopy(hashedPassword, 0, salt, 0, salt.Length);

                int subKeyLength = hashedPassword.Length - salt.Length;

                if (subKeyLength < SaltSize)
                {
                    return false;
                }

                byte[] expectedSubKey = new byte[subKeyLength];

                Buffer.BlockCopy(hashedPassword, salt.Length, expectedSubKey, 0, expectedSubKey.Length);

                byte[] actualSubKey = KeyDerivation.Pbkdf2(password, salt, Prf, IterationCount, subKeyLength);

                return ByteArraysEqual(actualSubKey, expectedSubKey);
            }
            catch
            {
                return false;
            }
        }

        private static bool ByteArraysEqual(byte[] a, byte[] b)
        {
            if (a == null && b == null)
            {
                return true;
            }

            if (a == null || b == null || a.Length != b.Length)
            {
                return false;
            }

            bool areSame = true;

            for (int i = 0; i < a.Length; i++)
            {
                areSame &= a[i] == b[i];
            }

            return areSame;
        }

        private byte[] HashPasswordInternal(string password)
        {
            byte[] salt = GetRandomSalt();

            byte[] subKey = KeyDerivation.Pbkdf2(password, salt, Prf, IterationCount, NumberOfBytesRequested);

            byte[] outputBytes = new byte[salt.Length + subKey.Length];

            Buffer.BlockCopy(salt, 0, outputBytes, 0, salt.Length);

            Buffer.BlockCopy(subKey, 0, outputBytes, salt.Length, subKey.Length);

            return outputBytes;
        }

        private byte[] GetRandomSalt()
        {
            byte[] salt = new byte[SaltSize];

            _rng.GetBytes(salt);

            return salt;
        }
    }
}
