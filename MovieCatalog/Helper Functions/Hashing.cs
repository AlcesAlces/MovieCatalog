using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MovieCatalog.Helper_Functions
{
    static class Hashing
    {

        public static string stringTohash(string toHash)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in getHash(toHash))
            {
                sb.Append(b.ToString("X2"));
            }

            return sb.ToString();
        }

        private static byte[] getHash(string toHash)
        {
            HashAlgorithm algo = MD5.Create();
            return algo.ComputeHash(Encoding.UTF8.GetBytes(toHash));
        }

    }
}
