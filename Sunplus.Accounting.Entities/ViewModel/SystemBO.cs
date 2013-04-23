using System.Runtime.Serialization;

/// <summary>
/// Summary description for SystemBO
/// </summary>
/// 

namespace Sunplus.Accounting.Entities.ViewModel
{
    [DataContract]
    public class SystemBO
    {
        public SystemBO(int id, string name)
        {
            Id = id;
            Name = name;
        }

        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Name { get; set; }
    }
}