using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.Hex.HexTypes;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Web3;
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
        public EthereumAddress ContractAddress { get; }
        public Account CallerAccount { get; set; }

        protected SmartContract(string rpcUrl, EthereumAddress contractAddress, Account callerAccount)
        {
            ContractAddress = contractAddress;
            CallerAccount = callerAccount;
            _web3 = new Web3(new Nethereum.Web3.Accounts.Account(CallerAccount.PrivateKey), rpcUrl);
        }

        /// <summary>
        /// Get next nonce from node
        /// </summary>
        /// <returns>Next nonce for caller</returns>
        protected async Task<HexBigInteger> GetNonce()
            => await _web3.Eth.Transactions.GetTransactionCount.SendRequestAsync(CallerAccount.Address);

        protected async Task<string> SendTx(CallData callData, TxParameters txParams)
        {
            TransactionInput tx = new TransactionInput
            {
                From = CallerAccount.Address,
                To = ContractAddress,
                Data = callData.TxData.ToHex(true)
            };

            if (txParams == null || txParams.GasLimit < 0)
            {
                tx.Gas = await _web3.Eth.Transactions.EstimateGas.SendRequestAsync(tx);
            }
            else
            {
                tx.Gas = new HexBigInteger(txParams.GasLimit);
            }

            tx.GasPrice = new HexBigInteger(txParams == null || txParams.GasPrice < 0 ?
                Web3.Convert.ToWei(1, Nethereum.Util.UnitConversion.EthUnit.Gwei) : txParams.GasPrice);

            tx.Nonce = txParams == null || txParams.Nonce < 0 ? await GetNonce() : new HexBigInteger(txParams.Nonce);

            return await _web3.TransactionManager.SendTransactionAsync(tx);
        }

        protected static string LoadABI(string contractName)
        {
            Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream($"ZeroX.ABI.{contractName}.json");
            Debug.Assert(stream != null);

            using (stream)
            using (StreamReader reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }
    }
}
