using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace ZeroX.Contracts
{
    public class TxParameters
    {
        public BigInteger GasPrice { get; set; }
        public int GasLimit { get; set; }
    }
}
