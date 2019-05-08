using Nethereum.Contracts;

namespace ZeroX.Utilities
{
    public class CallData
    {
        internal CallData(Function function, object[] parameters)
        {
            Function = function;
            Parameters = parameters;
            TxData = Function.GetData(Parameters);
        }

        public Function Function { get; }
        public object[] Parameters { get; }

        public string TxData { get; }
    }
}
