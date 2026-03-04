using System;
using System.Security.Cryptography;

namespace CaseManagement.Common.Helpers
{
    /// <summary>
    /// 密碼產生器
    /// </summary>
    public static class PasswordGenerator
    {
        /// <summary>
        /// 產生6位數字密碼
        /// </summary>
        /// <returns>6位數字密碼字串</returns>
        public static string Generate()
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                var bytes = new byte[4];
                rng.GetBytes(bytes);
                var randomNumber = BitConverter.ToUInt32(bytes, 0);
                var password = (randomNumber % 900000) + 100000;
                return password.ToString();
            }
        }
    }
}
