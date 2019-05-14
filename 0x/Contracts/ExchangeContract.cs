﻿
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
    public class ExchangeContract : SmartContract
    {
        // TODO: Add argument validation, add docs
        private const string ABIName = "Exchange";

        static ExchangeContract()
        {
            _abi = LoadABI(ABIName);
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

        /// <summary>
        /// Constructs exchange contract instance
        /// </summary>
        /// <param name="rpcUrl">Ethereum RPC URL</param>
        /// <param name="contractAddress">Address of exchange contract</param>
        /// <param name="callerAccount">Account which performs calls on blockchain (i.e. msg.sender)</param>
        public ExchangeContract(string rpcUrl, EthereumAddress contractAddress, Account callerAccount)
            : base(rpcUrl, contractAddress, callerAccount)
        {
        }

        /// <summary>
        /// Constructs exchange contract instance
        /// </summary>
        /// <param name="rpcUrl">Ethereum RPC UR</param>
        /// <param name="network">Ethereum network</param>
        /// <param name="callerAccount">Account which performs calls on blockchain (i.e. msg.sender)</param>
        public ExchangeContract(string rpcUrl, Network network, Account callerAccount)
            : this(rpcUrl, _contractAddressses[network], callerAccount)
        { }

        /// <summary>
        /// Fills order, exchanges tokens between Maker and Taker
        /// </summary>
        /// <param name="order">0x order</param>
        /// <param name="takerAssetFillAmount">Amount to fill</param>
        /// <param name="makerSignature">Maker signature (i.e. signature of the order)</param>
        /// <param name="takerSignature">Taker signature (i.e. signature of 0x <see cref="Transaction"/></param>
        /// <param name="txSalt">0x <see cref="Transaction"/> salt</param>
        /// <param name="txParams">Ethereum transaction parameters</param>
        /// <returns>Task which resolves into tx hash</returns>
        /// <exception cref="ArgumentNullException">Thrown when any of the arguments is null</exception>
        /// <exception cref="ArgumentException">Thrown when order.SenderAddress not equal to this contract instance caller address</exception>
        public async Task<string> FillOrderAsync(Order order, BigInteger takerAssetFillAmount, 
            byte[] makerSignature, byte[] takerSignature, BigInteger txSalt, TxParameters txParams = null)
        {
            order = order ?? throw new ArgumentNullException(nameof(order));
            makerSignature = makerSignature ?? throw new ArgumentNullException(nameof(makerSignature));
            takerSignature = takerSignature ?? throw new ArgumentNullException(nameof(takerSignature));

            if (order.SenderAddress != CallerAccount.Address)
                throw new ArgumentException($"{nameof(order)}.{nameof(Order.SenderAddress)} " +
                    $"must be equal to caller account address", nameof(order));

            CallData callData = FillOrderCallData(order, takerAssetFillAmount, 
                makerSignature, takerSignature, txSalt, ContractAddress, _web3);
            
            return await SendTx(callData, txParams);
        }

        /// <summary>
        /// Fills order, exchanges tokens between Maker and this contract caller
        /// </summary>
        /// <param name="order">0x order</param>
        /// <param name="takerAssetFillAmount">Amount to fill</param>
        /// <param name="signature">Maker signature (i.e. signature of the order)</param>
        /// <param name="txParams">Ethereum transaction parameters</param>
        /// <returns>Task which resolves into tx hash</returns>
        /// <exception cref="ArgumentNullException">Thrown when any of the arguments is null</exception>
        /// <exception cref="ArgumentException">Thrown when order.TakerAddress not equal to this contract caller address</exception>
        public async Task<string> FillOrderAsync(Order order, BigInteger takerAssetFillAmount, byte[] signature, TxParameters txParams = null)
        {
            order = order ?? throw new ArgumentNullException(nameof(order));
            signature = signature ?? throw new ArgumentNullException(nameof(signature));

            if (order.TakerAddress != CallerAccount.Address)
                throw new ArgumentException($"{nameof(order)}.{nameof(Order.TakerAddress)} " +
                    $"must be equal to caller account address", nameof(order));

            CallData callData = FillOrderCallData(order, takerAssetFillAmount, signature, ContractAddress, _web3);

            return await SendTx(callData, txParams);
        }

        /// <summary>
        /// Constructs <see cref="CallData"/> object for FillOrder call.
        /// Use this overload when you want third party to fill order
        /// </summary>
        /// <param name="order">0x order</param>
        /// <param name="takerAssetFillAmount">Amount to fill</param>
        /// <param name="makerSignature">Maker signature (i.e. signature of the order)</param>
        /// <param name="takerSignature">Taker signature (i.e. signature of 0x <see cref="Transaction"/></param>
        /// <param name="txSalt">0x <see cref="Transaction"/> salt</param>
        /// <param name="exchangeAddress">Exchange contract address</param>
        /// <param name="web3">Nethereum <see cref="Web3"/> object</param>
        /// <returns><see cref="CallData"/> instance</returns>
        /// <exception cref="ArgumentNullException">Thrown when any of the arguments is null</exception>
        public static CallData FillOrderCallData(Order order, BigInteger takerAssetFillAmount,
            byte[] makerSignature, byte[] takerSignature, BigInteger txSalt, EthereumAddress exchangeAddress, Web3 web3)
        {
            order = order ?? throw new ArgumentNullException(nameof(order));
            makerSignature = makerSignature ?? throw new ArgumentNullException(nameof(makerSignature));
            takerSignature = takerSignature ?? throw new ArgumentNullException(nameof(takerSignature));
            exchangeAddress = exchangeAddress ?? throw new ArgumentNullException(nameof(exchangeAddress));
            web3 = web3 ?? throw new ArgumentNullException(nameof(web3));
                
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

        /// <summary>
        /// Constructs <see cref="CallData"/> object for FillOrder call
        /// Use this overload when you want third party to fill order
        /// </summary>
        /// <param name="order">0x order</param>
        /// <param name="takerAssetFillAmount">Amount to fill</param>
        /// <param name="makerSignature">Maker signature (i.e. signature of the order)</param>
        /// <param name="takerSignature">Taker signature (i.e. signature of 0x <see cref="Transaction"/></param>
        /// <param name="txSalt">0x <see cref="Transaction"/> salt</param>
        /// <param name="network">Ethereum network</param>
        /// <param name="web3">Nethereum <see cref="Web3"/> object</param>
        /// <returns><see cref="CallData"/> instance</returns>
        /// <exception cref="ArgumentNullException">Thrown when any of the arguments is null</exception>
        public static CallData FillOrderCallData(Order order, BigInteger takerAssetFillAmount,
            byte[] makerSignature, byte[] takerSignature, BigInteger txSalt, Network network, Web3 web3)
            => FillOrderCallData(order, takerAssetFillAmount, makerSignature, takerSignature, txSalt, _contractAddressses[network], web3);

        public static Transaction FillOrderGet0xTx(Order order, BigInteger takerAssetFillAmount, byte[] makerSignature)
        {
            order = order ?? throw new ArgumentNullException(nameof(order));
            makerSignature = makerSignature ?? throw new ArgumentNullException(nameof(makerSignature));

            string txData = GetTxData(_abi, "fillOrder", 
                new object[] 
                {
                    order.EIP712Order,
                    takerAssetFillAmount,
                    makerSignature
                });

            return new Transaction(order.TakerAddress, txData);
        }

        /// <summary>
        /// Constructs <see cref="CallData"/> object for FillOrder call.
        /// Use this overload when Sender of the transaction is Taker
        /// </summary>
        /// <param name="order">0x order</param>
        /// <param name="takerAssetFillAmount">Amount to fill</param>
        /// <param name="signature">Maker signature (i.e. signature of the order)</param>
        /// <param name="exchangeAddress">Exchange contract address</param>
        /// <param name="web3">Nethereum <see cref="Web3"/> object</param>
        /// <returns><see cref="CallData"/> instance</returns>
        /// <exception cref="ArgumentNullException">Thrown when any of the arguments is null</exception>
        public static CallData FillOrderCallData(Order order, BigInteger takerAssetFillAmount, byte[] signature, 
            EthereumAddress exchangeAddress, Web3 web3)
        {

            order = order ?? throw new ArgumentNullException(nameof(order));
            signature = signature ?? throw new ArgumentNullException(nameof(signature));
            exchangeAddress = exchangeAddress ?? throw new ArgumentNullException(nameof(exchangeAddress));
            web3 = web3 ?? throw new ArgumentNullException(nameof(web3));

            Function fillOrderFunction = web3.Eth.GetContract(_abi, exchangeAddress).GetFunction("fillOrder");
            object[] parameters = new object[]
            {
                order.EIP712Order,
                takerAssetFillAmount,
                signature
            };

            return new CallData(fillOrderFunction, parameters);
        }

        /// <summary>
        /// Constructs <see cref="CallData"/> object for FillOrder call.
        /// Use this overload when Sender of the transaction is Taker
        /// </summary>
        /// <param name="order">0x order</param>
        /// <param name="takerAssetAmount">Amount to fill</param>
        /// <param name="signature">Maker signature (i.e. signature of the order)</param>
        /// <param name="network">Ethereum network</param>
        /// <param name="web3">Nethereum <see cref="Web3"/> object</param>
        /// <returns><see cref="CallData"/> instance</returns>
        /// <exception cref="ArgumentNullException">Thrown when any of the arguments is null</exception>
        public static CallData FillOrderCallData(Order order, BigInteger takerAssetAmount, byte[] signature, Network network, Web3 web3)
            => FillOrderCallData(order, takerAssetAmount, signature, _contractAddressses[network], web3);
    }
}
