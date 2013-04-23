using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sunplus.Accounting.Entities
{
    /// <summary>
    /// Summary description for MenuBO
    /// </summary>
    [Serializable]
    [DataContract]
    public class MenuBO : IMenuBO
    {
        #region IMenuBO Members

        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Url { get; set; }

        [DataMember]
        public bool IsOpenNewWindow { get; set; }

        [DataMember]
        public List<MenuBO> SubMenu { get; set; }

        [DataMember]
        public int ParentId { get; set; }

        [DataMember]
        public string FunctionName { get; set; }

        [DataMember]
        public string Target { get; set; }

        [DataMember]
        public bool IsHide { get; set; }

        [DataMember]
        public int Order { get; set; }

        [DataMember]
        public int Level { get; set; }

        [DataMember]
        public string System { get; set; }

        #endregion
    }
}