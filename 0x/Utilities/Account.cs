namespace ZeroX.Utilities
{
    public class Account
    {
        public Account(EthereumAddress address, string privateKey)
        {
            Address = address;
            PrivateKey = privateKey;
        }

        public string PrivateKey { get; set; }
        public EthereumAddress Address { get; set; }
    }
}
