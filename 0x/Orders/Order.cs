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
        // TODO: Add proper exception handling

        public EthereumAddress MakerAddress { get; set; }
        public EthereumAddress TakerAddress { get; set; }
        public EthereumAddress FeeRecipientAddress { get; set; }
        public EthereumAddress SenderAddress { get; set; }
        public BigInteger MakerAssetAmount { get; set; }
        public BigInteger TakerAssetAmount { get; set; }
        public BigInteger MakerFee { get; set; }
        public BigInteger TakerFee { get; set; }
        public DateTime ExpirationTime { get; set; }
        public BigInteger Salt { get; set; }
        public Asset MakerAsset { get; set; }
        public Asset TakerAsset { get; set; }

        /// <summary>
        /// Hashes given order
        /// </summary>
        /// <param name="exchangeAddress">Exchange contract address</param>
        /// <returns>Order signature</returns>
        /// <exception cref="ArgumentNullException">exchangeAddress is <c>null</c></exception>
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

        /// <summary>
        /// Signature signing 
        /// </summary>
        /// <param name="exchangeAddress">Exchange contract address</param>
        /// <param name="privateKey">Private key</param>
        /// <returns>Order signature (currently only EIP712 signature type is supported)</returns>
        public byte[] Sign(EthereumAddress exchangeAddress, string privateKey)
        {
            EthereumSignature signature = EIP712Service.Sign(EIP712Order,
                GetEIP712Domain(exchangeAddress), privateKey);

            return ByteUtil.Merge(signature.V, signature.R, signature.S, Constants.EIP712SignatureType);
        }

        // Potentially async in the future due to fact some signature types require interaction with Ethereum network
        /// <summary>
        /// Verifies order signature
        /// </summary>
        /// <param name="exchangeAddress">Exchange contract address</param>
        /// <param name="signerAddress">Signer address</param>
        /// <param name="signature">Signature</param>
        /// <returns><c>true</c> if signature is valid, false otherwise</returns>
        public bool VerifySignature(EthereumAddress exchangeAddress, EthereumAddress signerAddress, byte[] signature)
        {
            if (signature[65] != Constants.EIP712SignatureType[0])
                throw new NotImplementedException("Only EIP712 signature types are supported");

            return EIP712Service.VerifySignature(EIP712Order, GetEIP712Domain(exchangeAddress), 
                signerAddress, ReformatSignature(signature));
        }
        /// <summary>
        /// Converst VRST signature into RSV 
        /// T stands for signature type
        /// </summary>
        /// <param name="signature"></param>
        /// <returns></returns>
        private byte[] ReformatSignature(byte[] signature) 
            => ByteUtil.Merge(signature.Slice(1, signature.Length - 1), signature.Slice(0, 1));
        
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
                ExpirationTimeSeconds = ExpirationTime.GetUnixTime(),
                Salt = Salt,
                MakerAssetData = MakerAsset.AssetData,
                TakerAssetData = TakerAsset.AssetData
            }; 
        }
    }
}
