using System;

/// <summary>
/// Summary description for DataMappingAttributes
/// </summary>

namespace Sunplus.Accounting.DataAccess.Entity.BusinessObject
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class DataMappingAttribute : Attribute
    {
        #region Private Variables

        private readonly string _dataFieldName;
        private readonly object _nullValue;

        #endregion

        #region Constructors

        public DataMappingAttribute(string dataFieldName, object nullValue)
        {
            _dataFieldName = dataFieldName;
            _nullValue = nullValue;
        }

        public DataMappingAttribute(object nullValue) : this(string.Empty, nullValue)
        {
        }

        #endregion

        #region Public Properties

        public string DataFieldName
        {
            get { return _dataFieldName; }
        }

        public object NullValue
        {
            get { return _nullValue; }
        }

        #endregion
    }
}