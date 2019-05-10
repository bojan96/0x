using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nethereum.Web3;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using ZeroX.Assets;
using ZeroX.Contracts;
using ZeroX.Orders;
using ZeroX.Utilities;
using Transaction = ZeroX.Transactions.Transaction;

namespace IntegrationTests
{
    [TestClass]
    public class ExchangeTests
    {

        /// <summary>
        /// Address of Exchange contract
        /// </summary>
        private const string ExchangeAddress = "0x48bacb9266a570d521063ef5dd96e61686dbe788";

        /// <summary>
        /// Caller private key
        /// </summary>
        private const string CallerPrivateKey = "0xdf02719c4df8b9b8ac7f551fcb5d9ef48fa27eef7a66453879f4d8fdc6e78fb1";

        /// <summary>
        /// Owner of REP ERC20 token, has balance of at least 100000 REP
        /// </summary>
        private const string MakerAddress = "0x5409ed021d9299bf6814279a6a1411a7e866a631";

        private const string MakerPrivateKey = "0xf2f48ee19680706196e2e339e5da3491186e0c4c5030670656b0e0164837257d";

        private const string TakerPrivateKey = "0x5d862464fe9303452126c8bc94274b8c5f9874cbd219789b3eb2128075a76f72";

        /// <summary>
        /// Owner of DGD ERC20 token, has balance of at least 100000 DGD
        /// </summary>
        private const string TakerAddress = "0x6ecbe1db9ef729cbe972c83fb886247691fb6beb";

        /// <summary>
        /// REP ERC20 token
        /// </summary>
        private const string MakerTokenAddress = "0x34d402f14d58e001d8efbe6585051bf9706aa064";

        /// <summary>
        /// DGD ERC20 token
        /// </summary>
        private const string TakerTokenAddress = "0x25b8fe1de9daf8ba351890744ff28cf7dfa8f5e3";

        private const string RpcURL = "http://localhost:8545";

        [TestMethod]
        public async Task ExchangeTokens()
        {
            EthereumAddress exchangeAddress = (EthereumAddress)ExchangeAddress;
            Exchange exchange = new Exchange(RpcURL, exchangeAddress, new Account(CallerPrivateKey));
            Order order = new Order
            {
                MakerAddress = (EthereumAddress)MakerAddress,
                TakerAddress = (EthereumAddress)TakerAddress,
                SenderAddress = exchange.CallerAccount.Address,
                FeeRecipientAddress = exchange.CallerAccount.Address,
                MakerFee = 10,
                TakerFee = 10,
                MakerAssetAmount = 100,
                TakerAssetAmount = 100,
                MakerAsset = ERC20Asset.Create((EthereumAddress)MakerTokenAddress),
                TakerAsset = ERC20Asset.Create((EthereumAddress)TakerTokenAddress),
                // Setting day to 22 causes one missing byte in signature
                ExpirationTimeSeconds = (long)(new DateTime(2030, 1, 27, 0, 0, 0, DateTimeKind.Utc)
                - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds,
                Salt = new Random().Next()
            };

            byte[] makerSignature = order.Sign(exchangeAddress, MakerPrivateKey);
            Debug.Assert(makerSignature.Length == 66);
            Transaction tx = Exchange.FillOrderGet0xTx(order, order.TakerAssetAmount, makerSignature);
            byte[] takerSignature = tx.Sign(exchangeAddress, TakerPrivateKey);
            Debug.Assert(takerSignature.Length == 66);

            string hash = await exchange.FillOrderExecTxAsync(
                order,
                order.TakerAssetAmount,
                makerSignature,
                takerSignature,
                new TxParameters(1000000));
        }
    }
}
