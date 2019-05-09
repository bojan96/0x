
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

        public async Task<string> FillOrderViaExecuteTx(Order order, BigInteger takerAssetFillAmount, 
            byte[] makerSignature, byte[] takerSignature, TxParameters txParams = null)
        {

            // TODO: More checks
            if (order.SenderAddress != CallerAccount.Address)
                throw new ArgumentException($"{nameof(order)}.{nameof(Order.SenderAddress)} " +
                    $"must be equal to caller account address", nameof(order));


            CallData callData = FillOrderViaExecuteTxCallData(order, takerAssetFillAmount, 
                makerSignature, takerSignature, 0, ContractAddress, _web3);
            
            return await SendTx(callData, txParams);
        }

        public async Task<string> FillOrderAsync(Order order, BigInteger takerAssetFillAmount, byte[] signature, TxParameters txParams = null)
        {
            // TODO: Check whether order.takerAddress == CallerAccount.Address
            CallData callData = FillOrderCallData(order, takerAssetFillAmount, signature, ContractAddress, _web3);

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

        public static CallData FillOrderViaExecuteTxCallData(Order order, BigInteger takerAssetFillAmount,
            byte[] makerSignature, byte[] takerSignature, BigInteger txSalt, EthereumAddress exchangeAddress, Web3 web3)
        {
            Contract exchangeContract = web3.Eth.GetContract(_abi, exchangeAddress);
            Function executeTxFunction = exchangeContract.GetFunction("executeTransaction");
            Function fillOrderFunction = exchangeContract.GetFunction("fillOrder");
            string fillOrderTxData = fillOrderFunction.GetData(
                new object[] { order.EIP712Order, takerAssetFillAmount, makerSignature });

            object[] parameters = new object[]
            {
                txSalt,
                order.TakerAddress.ToString(),
                fillOrderTxData.HexToByteArray(),
                takerSignature
            };

            return new CallData(executeTxFunction, parameters);
        }

        public static Transaction FillOrderGet0xTx(Order order, BigInteger takerAssetFillAmount, byte[] makerSignature, EthereumAddress exchangeAddress, Web3 web3)
        {
            string txData = GetTxData("fillOrder", 
                new object[] 
                {
                    order.EIP712Order,
                    takerAssetFillAmount,
                    makerSignature
                }, 
                exchangeAddress, 
                web3);

            return new Transaction(order.TakerAddress, txData);
        }


        private static string GetTxData(string functionName, object[] functionParams, EthereumAddress exchangeAddress, Web3 web3)
            => web3.Eth.GetContract(_abi, exchangeAddress).GetFunction(functionName).GetData(functionParams);
        

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
