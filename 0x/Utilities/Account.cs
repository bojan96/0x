using Nethereum.Web3;

namespace ZeroX.Utilities
{
    // TODO: Add argument validation
    public class Account
    {
        public Account(string privateKey)
        {
            PrivateKey = privateKey;
            Address = (EthereumAddress)Web3.GetAddressFromPrivateKey(privateKey);
        }

        public string PrivateKey { get; set; }
        public EthereumAddress Address { get; set; }
    }
}
