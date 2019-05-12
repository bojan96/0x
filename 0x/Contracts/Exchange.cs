
using Nethereum.Contracts;
using Nethereum.Hex.HexTypes;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Web3;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;
using ZeroX.Enums;
using ZeroX.Orders;
using ZeroX.Utilities;
using Transaction = ZeroX.Transactions.Transaction;
using Nethereum.Hex.HexConvertors.Extensions;

namespace ZeroX.Contracts
{
    public class Exchange : SmartContract
    {
        // TODO: Add argument validation, add docs

        static Exchange()
        {
            _abi = LoadABI(nameof(Exchange));
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

        public Exchange(string rpcUrl, EthereumAddress contractAddress, Account callerAccount)
            : base(rpcUrl, contractAddress, callerAccount)
        {
        }

        public Exchange(string rpcUrl, Network network, Account callerAccount)
            : this(rpcUrl, _contractAddressses[network], callerAccount)
        { }

        public async Task<string> FillOrderExecTxAsync(Order order, BigInteger takerAssetFillAmount, 
            byte[] makerSignature, byte[] takerSignature, BigInteger txSalt, TxParameters txParams = null)
        {
            // TODO: More checks ?
            if (order.SenderAddress != CallerAccount.Address)
                throw new ArgumentException($"{nameof(order)}.{nameof(Order.SenderAddress)} " +
                    $"must be equal to caller account address", nameof(order));

            CallData callData = FillOrderExecTxCallData(order, takerAssetFillAmount, 
                makerSignature, takerSignature, txSalt, ContractAddress, _web3);
            
            return await SendTx(callData, txParams);
        }

        public async Task<string> FillOrderAsync(Order order, BigInteger takerAssetFillAmount, byte[] signature, TxParameters txParams = null)
        {
            // TODO: More validation ?
            if (order.TakerAddress != CallerAccount.Address)
                throw new ArgumentException($"{nameof(order)}.{nameof(Order.TakerAddress)} must be equal to caller account address", nameof(order));

            CallData callData = FillOrderCallData(order, takerAssetFillAmount, signature, ContractAddress, _web3);

            return await SendTx(callData, txParams);
        }

        public static CallData FillOrderExecTxCallData(Order order, BigInteger takerAssetFillAmount,
            byte[] makerSignature, byte[] takerSignature, BigInteger txSalt, EthereumAddress exchangeAddress, Web3 web3)
        {
            Contract exchangeContract = web3.Eth.GetContract(_abi, exchangeAddress);
            Function executeTxFunction = exchangeContract.GetFunction("executeTransaction");
            string fillOrderTxData = GetTxData(_abi, "fillOrder", 
                new object[] 
                {
                    order.EIP712Order,
                    takerAssetFillAmount,
                    makerSignature
                });

            object[] parameters = new object[]
            {
                txSalt,
                order.TakerAddress.ToString(),
                fillOrderTxData.HexToByteArray(),
                takerSignature
            };

            return new CallData(executeTxFunction, parameters);
        }

        public static CallData FillOrderExecTxCallData(Order order, BigInteger takerAssetFillAmount,
            byte[] makerSignature, byte[] takerSignature, BigInteger txSalt, Network network, Web3 web3)
            => FillOrderExecTxCallData(order, takerAssetFillAmount, makerSignature, takerSignature, txSalt, _contractAddressses[network], web3);

        public static Transaction FillOrderGet0xTx(Order order, BigInteger takerAssetFillAmount, byte[] makerSignature)
        {
            string txData = GetTxData(_abi, "fillOrder", 
                new object[] 
                {
                    order.EIP712Order,
                    takerAssetFillAmount,
                    makerSignature
                });

            return new Transaction(order.TakerAddress, txData);
        }

        public static CallData FillOrderCallData(Order order, BigInteger takerAssetFillAmount, byte[] signature, EthereumAddress exchangeAddress, Web3 web3)
        {
            Function fillOrderFunction = web3.Eth.GetContract(_abi, exchangeAddress).GetFunction("fillOrder");
            object[] parameters = new object[]
            {
                order.EIP712Order,
                takerAssetFillAmount,
                signature
            };

            return new CallData(fillOrderFunction, parameters);
        }

        public static CallData FillOrderCallData(Order order, BigInteger takerAssetAmount, byte[] signature, Network network, Web3 web3)
            => FillOrderCallData(order, takerAssetAmount, signature, _contractAddressses[network], web3);
    }
}
