using IKE;
using System;
using System.Security.Cryptography;
using System.Text;

namespace MicroCore.Security
{
    public class IKEClient
    {
        public byte[] key;
        public string GenerateResponse(string primeS, string givenS)
        {
            var prime = new BigInteger(primeS, 36);
            var g = (BigInteger)7;
            var mine = BigInteger.GenPseudoPrime(256, 30, new StrongNumberProvider());
            BigInteger given = new BigInteger(givenS, 36);
            BigInteger key = given.ModPow(mine, prime);

            this.key = key.GetBytes();
            BigInteger send = g.ModPow(mine, prime);
            return send.ToString(36);
        }
    }
    public static class MicroCripto
    {

        public static string Encrypt(string message, byte[] key)
        {
            TripleDESCryptoServiceProvider desCryptoProvider = new TripleDESCryptoServiceProvider();
            MD5CryptoServiceProvider hashMD5Provider = new MD5CryptoServiceProvider();

            var byteHash = hashMD5Provider.ComputeHash(key);
            desCryptoProvider.Key = byteHash;
            desCryptoProvider.Mode = CipherMode.ECB; 
            var byteBuff = Encoding.UTF8.GetBytes(message);

            string encoded =
                Convert.ToBase64String(desCryptoProvider.CreateEncryptor().TransformFinalBlock(byteBuff, 0, byteBuff.Length));
            return encoded;
        }

        public static string Decrypt(string encryptedMessage, byte[] key)
        {
            TripleDESCryptoServiceProvider desCryptoProvider = new TripleDESCryptoServiceProvider();
            MD5CryptoServiceProvider hashMD5Provider = new MD5CryptoServiceProvider();

            var byteHash = hashMD5Provider.ComputeHash(key);
            desCryptoProvider.Key = byteHash;
            desCryptoProvider.Mode = CipherMode.ECB; 
            var byteBuff = Convert.FromBase64String(encryptedMessage);

            string plaintext = Encoding.UTF8.GetString(desCryptoProvider.CreateDecryptor().TransformFinalBlock(byteBuff, 0, byteBuff.Length));
            return plaintext;
        }
    }
}
