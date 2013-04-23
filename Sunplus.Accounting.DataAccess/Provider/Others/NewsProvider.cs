using System;
using System.Collections;
using DNAide.Data;
using Sunplus.Accounting.Core.Common;

/// <summary>
/// Summary description for NewsProvider
/// </summary>

namespace Sunplus.Accounting.DataAccess.Provider.Others
{
    public abstract class NewsProvider : DataAccess
    {
        public static NewsProvider Instance()
        {
            return
                (NewsProvider)
                Activator.CreateInstance(
                    Type.GetType(string.Format(Globals.IBCSetting.News.NewsProviderType, GetDAOClient(DAOClient.AdoNet),
                                               GetDataSource(DataSource.SqlServer))));
        }

        public virtual DbResult NewsSel(Hashtable param)
        {
            throw new NotImplementedException();
        }

        public virtual DbResult NewsIns(Hashtable param)
        {
            throw new NotImplementedException();
        }

        public virtual DbResult NewsUpd(Hashtable param)
        {
            throw new NotImplementedException();
        }

        public virtual DbResult NewsDel(Hashtable param)
        {
            throw new NotImplementedException();
        }

        public virtual DbResult NewsSelAll(Hashtable param)
        {
            throw new NotImplementedException();
        }

        public virtual DbResult NewsSelFeature(Hashtable param)
        {
            throw new NotImplementedException();
        }

        public virtual DbResult NewsUpdFeature(Hashtable param)
        {
            throw new NotImplementedException();
        }
    }
}