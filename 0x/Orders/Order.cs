﻿using EIP712;
using Nethereum.Util;
using System;
using System.Numerics;
using ZeroX.Assets;
using ZeroX.Utilities;

namespace ZeroX.Orders
{
    public class Order
    {
        // TODO: Add docs, property validation

        public EthereumAddress MakerAddress { get; set; }
        public EthereumAddress TakerAddress { get; set; }
        public EthereumAddress FeeRecipientAddress { get; set; }
        public EthereumAddress SenderAddress { get; set; }
        public BigInteger MakerAssetAmount { get; set; }
        public BigInteger TakerAssetAmount { get; set; }
        public BigInteger MakerFee { get; set; }
        public BigInteger TakerFee { get; set; }
        public BigInteger ExpirationTimeSeconds { get; set; }
        public BigInteger Salt { get; set; }
        public Asset MakerAsset { get; set; }
        public Asset TakerAsset { get; set; }

        /// <summary>
        /// Hashes given order
        /// </summary>
        /// <param name="exchangeAddress">Address of Exchange contract</param>
        /// <returns>Order signature</returns>
        /// <exception cref="ArgumentNullException">exchangeAddress is null</exception>
        public byte[] Hash(EthereumAddress exchangeAddress)
        {
            if (exchangeAddress == null)
                throw new ArgumentNullException(nameof(exchangeAddress));

            EIP712Domain domain = GetEIP712Domain(exchangeAddress);

            return EIP712Service.Hash(EIP712Order, domain);
        }

        private EIP712Domain GetEIP712Domain(EthereumAddress exchangeAddress)
            => new EIP712Domain
            {
                Name = "0x Protocol",
                Version = "2",
                VerifyingContract = exchangeAddress.ToString()
            };

        public byte[] Sign(EthereumAddress exchangeAddress, string privateKey)
        {
            EthereumSignature signature = EIP712Service.Sign(EIP712Order,
                GetEIP712Domain(exchangeAddress), privateKey);

            return ByteUtil.Merge(signature.V, signature.R, signature.S, Constants.EIP712SignatureType);
        }

        public bool VerifySignature(EthereumAddress exchangeAddress, EthereumAddress signerAddress, byte[] signature)
            => EIP712Service.VerifySignature(EIP712Order, GetEIP712Domain(exchangeAddress), signerAddress, signature);
        

        internal OrderInternal EIP712Order
        {
            get => new OrderInternal
            {
                MakerAddress = MakerAddress.ToString(),
                TakerAddress = TakerAddress.ToString(),
                FeeRecipientAddress = FeeRecipientAddress.ToString(),
                SenderAddress = SenderAddress.ToString(),
                MakerAssetAmount = MakerAssetAmount,
                TakerAssetAmount = TakerAssetAmount,
                MakerFee = MakerFee,
                TakerFee = TakerFee,
                ExpirationTimeSeconds = ExpirationTimeSeconds,
                Salt = Salt,
                MakerAssetData = MakerAsset.AssetData,
                TakerAssetData = TakerAsset.AssetData
            }; 
        }
    }
}
