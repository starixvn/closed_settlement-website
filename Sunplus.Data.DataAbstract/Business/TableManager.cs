using Sunplus.Data.DataAbstract.BootStrappers;
using Sunplus.Data.DataAbstract.Storage;

namespace Sunplus.Data.DataAbstract.Business
{
    public class TableManager
    {
        private readonly IDataStorage _strorageDAO;
        public TableManager() 
        {
            _strorageDAO = CommonBootStrapper.Locator.GetInstance<IDataStorage>();
        }

        public bool Store<T>(T data, Queue.ActionType action)
        {
            return this.Store<T>(data, action, null);
        }

        public bool Store<T>(T data, Queue.ActionType action, object identify)
        {
            bool result = false;
            switch(action)
            {
                case Queue.ActionType.Insert:
                    result = _strorageDAO.Insert<T>(string.Empty, data);
                    break;
                case Queue.ActionType.Update:
                    result = _strorageDAO.Update<T>(string.Empty, data);
                    break;
                case Queue.ActionType.Delete:
                    result = _strorageDAO.Delete<T>(string.Empty, identify);
                    break;
                default:
                    break;
            }
            return result;
        }
    }
}
