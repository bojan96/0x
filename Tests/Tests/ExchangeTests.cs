﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.Web3;
using System;
using System.Numerics;
using ZeroX.Assets;
using ZeroX.Contracts;
using ZeroX.Orders;
using ZeroX.Utilities;

namespace Tests.Tests
{
    [TestClass]
    public class ExchangeTests
    {
        [TestMethod]
        public void FillOrderViaExecuteTxCallData()
        {
            // We use invalid data just because we want to test encoding of tx data
            // for executeTransaction

            Order order = new Order
            {
                SenderAddress = EthereumAddress.ZeroAddress,
                MakerAddress = EthereumAddress.ZeroAddress,
                TakerAddress = EthereumAddress.ZeroAddress,
                FeeRecipientAddress = EthereumAddress.ZeroAddress,
                MakerAssetAmount = 0,
                TakerAssetAmount = 0,
                MakerFee = 0,
                TakerFee = 0,
                ExpirationTimeSeconds = 0,
                Salt = 0,
                MakerAsset = ERC20Asset.Create(EthereumAddress.ZeroAddress),
                TakerAsset = ERC20Asset.Create(EthereumAddress.ZeroAddress)
            };

            BigInteger takerAssetFillAmount = 0;
            byte[] signature = new byte[1]; 

            CallData callData = Exchange.FillOrderViaExecuteTxCallData(order, takerAssetFillAmount, signature, 
                signature, 0, EthereumAddress.ZeroAddress, new Web3());

            byte[] expectedTxData = ("0xbfc8bfce00000000000000000000000000000000" +
                "00000000000000000000000000000000000000000000000000000000000000" +
                "00000000000000000000000000000000000000000000000000000000000000" +
                "00000000000000000000000000000000008000000000000000000000000000" +
                "000000000000000000000000000000000003a0000000000000000000000000" +
                "00000000000000000000000000000000000002e4b4be83d500000000000000" +
                "00000000000000000000000000000000000000000000000060000000000000" +
                "00000000000000000000000000000000000000000000000000000000000000" +
                "0000000000000000000000000000000000000000000000000002a000000000" +
                "00000000000000000000000000000000000000000000000000000000000000" +
                "00000000000000000000000000000000000000000000000000000000000000" +
                "00000000000000000000000000000000000000000000000000000000000000" +
                "00000000000000000000000000000000000000000000000000000000000000" +
                "00000000000000000000000000000000000000000000000000000000000000" +
                "00000000000000000000000000000000000000000000000000000000000000" +
                "00000000000000000000000000000000000000000000000000000000000000" +
                "00000000000000000000000000000000000000000000000000000000000000" +
                "00000000000000000000000000000000000000000000000000000000000000" +
                "00000000000000000000000000000000000000000000000000000000000000" +
                "00000000000000000000000000000000000000000000000000000000000000" +
                "00000000000180000000000000000000000000000000000000000000000000" +
                "00000000000001e00000000000000000000000000000000000000000000000" +
                "000000000000000024f47261b0000000000000000000000000000000000000" +
                "00000000000000000000000000000000000000000000000000000000000000" +
                "00000000000000000000000000000000000000000000000000000000000000" +
                "000000000000000000000024f47261b0000000000000000000000000000000" +
                "00000000000000000000000000000000000000000000000000000000000000" +
                "00000000000000000000000000000000000000000000000000000000000000" +
                "00000000000000000000000000000100000000000000000000000000000000" +
                "00000000000000000000000000000000000000000000000000000000000000" +
                "00000000000000000000000000000000000000000000000000000000000000" +
                "00000000000000000000000000010000000000000000000000000000000000" +
                "000000000000000000000000000000").HexToByteArray();

            CollectionAssert.AreEqual(expectedTxData, callData.TxData);

        }
    }
}
