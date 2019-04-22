﻿using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.Util;
using System;
using System.Collections.Generic;

namespace ZeroX.Utilities
{
    public class EthereumAddress
    {

        // TODO: Add docs

        private string _address;
        private const string ZeroAddressLiteral = "0x0000000000000000000000000000000000000000";

        public static EthereumAddress ZeroAddress { get; } = new EthereumAddress(ZeroAddressLiteral);
        public bool IsChecksumAddress => AddressUtil.Current.IsChecksumAddress(_address);
        public byte[] Bytes => _address.HexToByteArray();


        public EthereumAddress(string address)
        {
            if (address == null)
                throw new ArgumentNullException(nameof(address));
            if (!AddressUtil.Current.IsValidEthereumAddressHexFormat(address))
                throw new FormatException("Invalid address format");
            _address = address;
        }

        public override bool Equals(object obj)
        {
            var address = obj as EthereumAddress;
            return obj != null && (ReferenceEquals(this, obj) || 
                AddressUtil.Current.AreAddressesTheSame(_address, address._address));
        }

        // Generated by VS
        public override int GetHashCode()
            => 2037418838 + EqualityComparer<string>.Default.GetHashCode(_address);

        public static bool operator == (EthereumAddress lhs, EthereumAddress rhs)
            => ReferenceEquals(lhs, rhs) || (lhs?.Equals(rhs)).GetValueOrDefault();
        
        public static bool operator != (EthereumAddress lhs, EthereumAddress rhs)
            => !(lhs == rhs);

        public override string ToString()
            => _address;

        public static implicit operator string(EthereumAddress address)
            => address.ToString();

        public static explicit operator EthereumAddress(string address)
        {
            try
            {
                return new EthereumAddress(address);
            }
            catch (Exception ex)
            {
                throw new InvalidCastException($"Can not cast string to EthereumAddress", ex);
            }
        }

        public static EthereumAddress FromByteArray(byte[] address)
        {
            if (address == null)
                throw new ArgumentNullException(nameof(address));

            return new EthereumAddress(address.ToHex(true));
        }
    }
}
