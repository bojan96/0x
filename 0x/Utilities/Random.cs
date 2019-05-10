using System.Numerics;

namespace ZeroX.Utilities
{
    internal static class Random
    {

        private static System.Random _random = new System.Random();

        /// <summary>
        /// Generates a salt value suitable for use in orders and 0x transactions
        /// </summary>
        /// <returns></returns>
        public static BigInteger GenerateSalt()
        {
            byte[] randomInt = new byte[33];
            randomInt[32] = 0; // Ensure we will not generate negative value
            _random.NextBytes(randomInt);

            return new BigInteger(randomInt);
        }
    }
}
