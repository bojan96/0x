using Nethereum.Contracts;
using Nethereum.Hex.HexConvertors.Extensions;

namespace ZeroX.Utilities
{
    public class CallData
    {
        internal CallData(Function function, object[] parameters)
        {
            Function = function;
            Parameters = parameters;
            TxData = Function.GetData(Parameters).HexToByteArray();
        }

        public Function Function { get; }
        public object[] Parameters { get; }

        public byte[] TxData { get; }
    }
}
