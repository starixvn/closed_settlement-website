using System.Collections.Generic;

/// <summary>
/// Summary description for MappingInfoCache
/// </summary>

namespace Sunplus.Accounting.DataAccess.Entity.BusinessObject
{
    internal static class MappingInfoCache
    {
        private static readonly Dictionary<string, List<PropertyMappingInfo>> cache =
            new Dictionary<string, List<PropertyMappingInfo>>();

        internal static List<PropertyMappingInfo> GetCache(string typeName)
        {
            List<PropertyMappingInfo> info = null;
            try
            {
                info = cache[typeName];
            }
            catch (KeyNotFoundException ex)
            {
            }

            return info;
        }

        internal static void SetCache(string typeName, List<PropertyMappingInfo> mappingInfoList)
        {
            cache[typeName] = mappingInfoList;
        }

        public static void ClearCache()
        {
            cache.Clear();
        }
    }
}