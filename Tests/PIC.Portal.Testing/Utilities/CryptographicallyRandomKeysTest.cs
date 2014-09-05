using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security;
using System.Security.Cryptography;
using NUnit.Framework;

namespace PIC.Portal.Testing.Utilities
{
    [TestFixture]
    public class CryptographicallyRandomKeysTest
    {
        [Test]
        public void GenerateTest()
        {
            int len = 48;

            byte[] buff = new byte[len / 2];
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            rng.GetBytes(buff);

            StringBuilder sb = new StringBuilder(len);

            for (int i = 0; i < buff.Length; i++)
            {
                sb.Append(string.Format("{0:X2}", buff[i]));
            }

            Console.WriteLine(sb);

        }
    }
}
