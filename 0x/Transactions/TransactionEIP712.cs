using EIP712.Attributes;
using System;
using System.Numerics;

namespace ZeroX.Transactions
{
    [StructName("ZeroExTransaction")]
    internal class TransactionEIP712
    {
        [Member("uint256", 1)]
        public BigInteger Salt { get; set; }

        [Member("address", 2)]
        public string SignerAddress { get; set; }

        [Member("bytes", 3)]
        public byte[] Data { get; set; }
    }
}
