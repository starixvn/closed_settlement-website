using System.Configuration;

namespace Sunplus.Accounting.Core.Configurations.StoreProcedureGroup
{
    public class RacingSession : System.Configuration.ConfigurationSection
    {
        public enum StoreCollection
        {
            RacingControl, MeetingComm
        }

        [ConfigurationProperty("RacingControl", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(StoreProcedureCollection),
            AddItemName = "add",
            ClearItemsName = "clear",
            RemoveItemName = "remove")]
        public StoreProcedureCollection RacingControl
        {
            get
            {
                var spCollection =
                    (StoreProcedureCollection)base["RacingControl"];
                return spCollection;
            }
        }

        [ConfigurationProperty("MeetingComm", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(StoreProcedureCollection),
            AddItemName = "add",
            ClearItemsName = "clear",
            RemoveItemName = "remove")]
        public StoreProcedureCollection MeetingComm
        {
            get
            {
                var spCollection =
                    (StoreProcedureCollection)base["MeetingComm"];
                return spCollection;
            }
        }

        public static RacingSession Instance
        {
            get
            {
                var racingSession = ConfigurationManager.GetSection("StoreProcedureGroup/Racing") as RacingSession;
                return racingSession;
            }
        }


        public StoreProcedureCollection this[StoreCollection name]
        {
            get
            {

                var spCollection = (StoreProcedureCollection)this.GetType().GetProperty(name.ToString()).GetValue(this, System.Reflection.BindingFlags.GetField, null, null, null);
                return spCollection;
            }
        }
    }
}
