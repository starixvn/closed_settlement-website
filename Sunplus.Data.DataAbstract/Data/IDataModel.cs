namespace Sunplus.Data.DataAbstract.Data
{
    public interface IDataModel
    {
        object Get(int key);
        
        bool Insert(string key, object value);

        bool Delete(int key);
    }
}
