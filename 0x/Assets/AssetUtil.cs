using System;

namespace ZeroX.Assets
{
    public static class AssetUtil
    {

        /// <summary>
        /// Constructs one of the supported asset types, also cast to given asset type 
        /// </summary>
        /// <typeparam name="T">Asset type</typeparam>
        /// <param name="assetData">Asset data</param>
        /// <returns>New instance</returns>
        /// <exception cref="InvalidCastException">Unable to cast to specified asset data</exception>
        public static T Create<T>(byte[] assetData) where T : Asset
        {
            Asset asset;

            if (ERC20Asset.ValidateAssetData(assetData))
                asset = ERC20Asset.Create(assetData);
            else if (ERC721Asset.ValidateAssetData(assetData))
                asset = ERC721Asset.Create(assetData);
            else
                throw new ArgumentException("Not valid asset data", nameof(assetData));

           return (T)asset;
        }
    }
}
