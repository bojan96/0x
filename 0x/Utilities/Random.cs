using System.Numerics;

namespace ZeroX.Utilities
{
    public static class Random
    {

        private static System.Random _random = new System.Random();
        private static object _randLock = new object();

        /// <summary>
        /// Generates a salt value suitable for use in orders and 0x transactions
        /// </summary>
        /// <returns></returns>
        public static BigInteger GenerateSalt()
        {
            byte[] randomInt = new byte[33];
            lock(_randLock)
                _random.NextBytes(randomInt);
            randomInt[32] = 0; // Ensure we will not generate negative value

            return new BigInteger(randomInt);
        }
    }
}
