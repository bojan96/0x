using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.Hex.HexTypes;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Web3;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using ZeroX.Utilities;

namespace ZeroX.Contracts
{
    public abstract class SmartContract
    {
        protected readonly Web3 _web3;

        /// <summary>
        /// Smart contract address
        /// </summary>
        public EthereumAddress ContractAddress { get; }

        /// <summary>
        /// Caller account
        /// </summary>
        public Account CallerAccount { get; set; }

        protected SmartContract(string rpcUrl, EthereumAddress contractAddress, Account callerAccount)
        {
            rpcUrl = rpcUrl ?? throw new ArgumentNullException(nameof(rpcUrl));
            ContractAddress = contractAddress ?? throw new ArgumentNullException(nameof(contractAddress));
            CallerAccount = callerAccount ?? throw new ArgumentNullException(nameof(callerAccount));
            _web3 = new Web3(new Nethereum.Web3.Accounts.Account(CallerAccount.PrivateKey), rpcUrl);
        }

        /// <summary>
        /// Get next nonce from node
        /// </summary>
        /// <returns>Next nonce for caller</returns>
        private async Task<HexBigInteger> GetNonce()
            => await _web3.Eth.Transactions.GetTransactionCount.SendRequestAsync(CallerAccount.Address);

        protected async Task<string> SendTx(CallData callData, TxParameters txParams)
        {
            TransactionInput tx = new TransactionInput
            {
                From = CallerAccount.Address,
                To = ContractAddress,
                Data = callData.TxData.ToHex(true)
            };

            tx.Gas = txParams == null || !txParams.GasLimit.HasValue ?
                await _web3.Eth.Transactions.EstimateGas.SendRequestAsync(tx) : new HexBigInteger(txParams.GasLimit.Value);
            
            tx.GasPrice = new HexBigInteger(txParams == null || !txParams.GasPrice.HasValue ?
                Web3.Convert.ToWei(1, Nethereum.Util.UnitConversion.EthUnit.Gwei) : txParams.GasPrice.Value);

            tx.Nonce = txParams == null || !txParams.Nonce.HasValue ? await GetNonce() : new HexBigInteger(txParams.Nonce.Value);

            return await _web3.TransactionManager.SendTransactionAsync(tx);
        }

        protected static string GetTxData(string abi, string functionName, object[] functionParams)
            => new Web3().Eth.GetContract(abi, EthereumAddress.ZeroAddress).GetFunction(functionName).GetData(functionParams);

        protected static string LoadABI(string contractName)
        {
            Stream stream = typeof(SmartContract).GetTypeInfo()
                .Assembly.GetManifestResourceStream($"ZeroX.ABI.{contractName}.json");
            Debug.Assert(stream != null);

            using (stream)
            using (StreamReader reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }
    }
}
