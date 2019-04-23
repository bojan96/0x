
using Nethereum.Contracts;
using Nethereum.Hex.HexTypes;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Web3;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Numerics;
using System.Reflection;
using System.Threading.Tasks;
using ZeroX.Enums;
using ZeroX.Orders;
using ZeroX.Utilities;

namespace ZeroX.Contracts
{
    public class Exchange
    {
        // TODO: Add argument validation, add docs

        static Exchange()
        {
            Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("ZeroX.ABI.Exchange.json");
            Debug.Assert(stream != null);

            using (stream)
            using (StreamReader reader = new StreamReader(stream))
            {
                _abi = reader.ReadToEnd();
            }

        }

        private static readonly Dictionary<Network, EthereumAddress> _contractAddressses 
            = new Dictionary<Network, EthereumAddress>
            {
                { Network.Main,    (EthereumAddress)"0x4f833a24e1f95d70f028921e27040ca56e09ab0b"},
                { Network.Ropsten, (EthereumAddress)"0x4530c0483a1633c7a1c97d2c53721caff2caaaaf"},
                { Network.Rinkeby, (EthereumAddress)"0xbce0b5f6eb618c565c3e5f5cd69652bbc279f44e"},
                { Network.Kovan,   (EthereumAddress)"0x35dd2932454449b14cee11a94d3674a936d5d7b2"}
            };

        private static readonly string _abi;
        private readonly Web3 _web3;

        public EthereumAddress ContractAddress { get; }
        public Account CallerAccount { get; set; }

        public Exchange(string rpcUrl, EthereumAddress contractAddress, Account callerAccount)
        {
            ContractAddress = contractAddress;
            CallerAccount = callerAccount;
            _web3 = new Web3(rpcUrl);
        }

        public Exchange(string rpcUrl, Network network, Account callerAccount)
            : this(rpcUrl, _contractAddressses[network], callerAccount)
        { }

        public async Task<string> FillOrderAsync(Order order, BigInteger takerAssetAmount, byte[] signature, TxParameters txParams = null)
        {
            CallData callData = FillOrderCallData(order, takerAssetAmount, signature, ContractAddress, _web3);

            TransactionInput tx = new TransactionInput
            {
                From = CallerAccount.Address,
                To = ContractAddress,
                Data = callData.Function.GetData(callData.Parameters)
            };

            if(txParams == null || txParams.GasLimit < 0)
            {
                tx.Gas = await _web3.Eth.Transactions.EstimateGas.SendRequestAsync(tx);
            }
            else
            {
                tx.Gas = new HexBigInteger(txParams.GasLimit);
            }

            tx.GasPrice = new HexBigInteger( txParams == null || txParams.GasPrice < 0 ? 
                Web3.Convert.ToWei(1, Nethereum.Util.UnitConversion.EthUnit.Gwei) : txParams.GasPrice);

            tx.Nonce = txParams == null || txParams.Nonce < 0 ? await GetNonce() : new HexBigInteger(txParams.Nonce);

            return await callData.Function.SendTransactionAsync(tx, callData.Parameters);          
        }

        /// <summary>
        /// Get next nonce from node
        /// </summary>
        /// <returns>Next nonce for caller</returns>
        private async Task<HexBigInteger> GetNonce()
            => await _web3.Eth.Transactions.GetTransactionCount.SendRequestAsync(CallerAccount.Address);
        
        public static CallData FillOrderCallData(Order order, BigInteger takerAssetAmount, byte[] signature, EthereumAddress exchangeAddress, Web3 web3)
        {

            Function fillOrderFunction = web3.Eth.GetContract(_abi, exchangeAddress).GetFunction("fillOrder");
            object[] parameters = new object[]
            {
                order.EIP712Order,
                takerAssetAmount,
                signature
            };

            return new CallData(fillOrderFunction, parameters);
        }

        public static CallData FillOrderCallData(Order order, BigInteger takerAssetAmount, byte[] signature, Network network, Web3 web3)
            => FillOrderCallData(order, takerAssetAmount, signature, _contractAddressses[network], web3);
    }
}
