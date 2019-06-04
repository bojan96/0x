using System;

namespace ZeroX.Assets
{
    public static class AssetFactory
    {
        public static Asset Create(byte[] assetData)
        {
            Asset asset;

            if (ERC20Asset.ValidateAssetData(assetData))
                asset = ERC20Asset.Create(assetData);
            else if (ERC721Asset.ValidateAssetData(assetData))
                asset = ERC721Asset.Create(assetData);
            else
                throw new ArgumentException("Not valid asset data", nameof(assetData));

           return asset;
        }
    }
}
