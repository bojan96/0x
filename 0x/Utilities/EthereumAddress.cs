using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.Util;
using System;
using System.Collections.Generic;

namespace ZeroX.Utilities
{
    public class EthereumAddress
    {

        private string _address;
        private const string ZeroAddressLiteral = "0x0000000000000000000000000000000000000000";

        /// <summary>
        /// Ethereum address with zero values (i.e. 0x0000000000000000000000000000000000000000)
        /// </summary>
        public static EthereumAddress ZeroAddress { get; } = new EthereumAddress(ZeroAddressLiteral);

        /// <summary>
        /// Indicates whether current instance is checksum address 
        /// </summary>
        public bool IsChecksumAddress => AddressUtil.Current.IsChecksumAddress(_address);

        /// <summary>
        /// Gets byte representation of Ethereum address
        /// </summary>
        public byte[] Bytes => _address.HexToByteArray();

        /// <summary>
        /// Constructs an new instance of <see cref="EthereumAddress"/> from string
        /// </summary>
        /// <param name="address">Ethereum address</param>
        /// <exception cref="ArgumentNullException"><paramref name="address"/> is null</exception>
        public EthereumAddress(string address)
        {
            if (address == null)
                throw new ArgumentNullException(nameof(address));
            if (!AddressUtil.Current.IsValidEthereumAddressHexFormat(address))
                throw new FormatException("Invalid address format");
            _address = address;
        }

        /// <summary>
        /// Compares current instance and specified object
        /// </summary>
        /// <param name="obj">The object to compare</param>
        /// <returns>true if <paramref name="obj"/> is instance of <see cref="EthereumAddress"/> and 
        /// is equal to current instance, false otherwise</returns>
        public override bool Equals(object obj)
        {
            var address = obj as EthereumAddress;
            return obj != null && (ReferenceEquals(this, obj) || 
                AddressUtil.Current.AreAddressesTheSame(_address, address._address));
        }

        /// <summary>
        /// Generates hash code for current instance
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
            => 2037418838 + EqualityComparer<string>.Default.GetHashCode(_address);

        /// <summary>
        /// Compares two <see cref="EthereumAddress"/> object for equality
        /// </summary>
        /// <param name="lhs">Left hand side value to compare</param>
        /// <param name="rhs">Right hand side value to compare</param>
        /// <returns><c>true</c> if two specified values are equal, false otherwise</returns>
        public static bool operator == (EthereumAddress lhs, EthereumAddress rhs)
            => ReferenceEquals(lhs, rhs) || (lhs?.Equals(rhs)).GetValueOrDefault();

        /// <summary>
        /// Compares two <see cref="EthereumAddress"/> object for inequality
        /// </summary>
        /// <param name="lhs">Left hand side value to compare</param>
        /// <param name="rhs">Right hand side value to compare</param>
        /// <returns><c>true</c> if two specified values are not equal, false otherwise</returns>
        public static bool operator != (EthereumAddress lhs, EthereumAddress rhs)
            => !(lhs == rhs);

        /// <summary>
        /// Converts current instance to string representation
        /// </summary>
        /// <returns><see cref="string"/> representation of <see cref="EthereumAddress"/> object</returns>
        public override string ToString()
            => _address;

        /// <summary>
        /// Cast <see cref="EthereumAddress"/> object to <see cref="string"/>
        /// </summary>
        /// <param name="address">Value to cast</param>
        public static implicit operator string(EthereumAddress address)
            => address.ToString();

        /// <summary>
        /// Cast <see cref="string"/> representation of Ethereum addres to <see cref="EthereumAddress"/> object
        /// </summary>
        /// <param name="address">Value to cast</param>
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

        /// <summary>
        /// Construct <see cref="EthereumAddress"/> object from byte representation of address
        /// </summary>
        /// <param name="address">Byte representation of Ethereum address</param>
        /// <returns><see cref="EthereumAddress"/> instance equal to byte representation</returns>
        public static EthereumAddress FromByteArray(byte[] address)
        {
            if (address == null)
                throw new ArgumentNullException(nameof(address));

            return new EthereumAddress(address.ToHex(true));
        }
    }
}
