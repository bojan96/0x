namespace ZeroX.Assets
{
    /// <summary>
    /// 0x compatible asset
    /// </summary>
    public abstract class Asset
    {
       /// <summary>
       /// Gets byte representation of asset data
       /// </summary>
        public byte[] AssetData { get; }

        protected Asset(byte[] assetData)
            => AssetData = assetData;

    }
}
