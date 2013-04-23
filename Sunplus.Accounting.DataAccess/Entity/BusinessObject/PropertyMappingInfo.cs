using System.Reflection;

/// <summary>
/// Summary description for PropertyMappingInfo
/// </summary>

namespace Sunplus.Accounting.DataAccess.Entity.BusinessObject
{
    internal sealed class PropertyMappingInfo
    {
        #region Private Variables

        private readonly object _defaultValue;
        private readonly PropertyInfo _propInfo;
        private string _dataFieldName;

        #endregion

        #region Constructors

        internal PropertyMappingInfo()
            : this(string.Empty, null, null)
        {
        }

        internal PropertyMappingInfo(string dataFieldName, object defaultValue, PropertyInfo info)
        {
            _dataFieldName = dataFieldName;
            _defaultValue = defaultValue;
            _propInfo = info;
        }

        #endregion

        #region Public Properties

        public string DataFieldName
        {
            get
            {
                if (string.IsNullOrEmpty(_dataFieldName))
                {
                    _dataFieldName = _propInfo.Name;
                }
                return _dataFieldName;
            }
        }

        public object DefaultValue
        {
            get { return _defaultValue; }
        }

        public PropertyInfo PropertyInfo
        {
            get { return _propInfo; }
        }

        #endregion
    }
}