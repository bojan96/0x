﻿using Nethereum.Contracts;

namespace ZeroX.Contracts
{
    public class CallData
    {
        internal CallData(Function function, object[] parameters)
        {
            Function = function;
            Parameters = parameters;
        }

        public Function Function { get; }
        public object[] Parameters { get; }
    }
}
