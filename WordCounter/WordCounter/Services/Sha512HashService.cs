using System;
using System.Security.Cryptography;
using System.Text;
using WordCounter.Services.Base;

namespace WordCounter.Services
{
    public class Sha512HashService : IHashService
    {
        #region Public Members

        public string GetHash(string contentString)
        {
            if (string.IsNullOrEmpty(contentString))
            {
                return string.Empty;
            }

            using var sha = SHA512.Create();

            var bytes = Encoding.UTF8.GetBytes(contentString);
            var hash = sha.ComputeHash(bytes);

            return Convert.ToBase64String(hash);
        }

        #endregion
    }
}