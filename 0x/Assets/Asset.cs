namespace ZeroX.Assets
{
    public abstract class Asset
    {
       
        public byte[] AssetData { get; }

        protected Asset(byte[] assetData)
            => AssetData = assetData;

    }
}
