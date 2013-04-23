namespace Sunplus.Data.DataAbstract.Storage.Cache
{
    public interface ICache : IDataStorage
    {
        void FlushAll();
    }
}
