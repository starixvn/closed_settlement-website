using System.Configuration;

namespace Sunplus.Accounting.Core.Configurations.StoreProcedureGroup
{
    public class ExchangeRateSession : System.Configuration.ConfigurationSection
    {
        public enum StoreCollection
        {
            ExchangeRateControl
        }

        [ConfigurationProperty("ExchangeRateControl", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(StoreProcedureCollection),
            AddItemName = "add",
            ClearItemsName = "clear",
            RemoveItemName = "remove")]
        public StoreProcedureCollection ExchangeRateControl
        {
            get
            {
                var spCollection =
                    (StoreProcedureCollection)base["ExchangeRateControl"];
                return spCollection;
            }
        }

        public static ExchangeRateSession Instance
        {
            get
            {
                var exchangeRateSession = ConfigurationManager.GetSection("StoreProcedureGroup/ExchangeRate") as ExchangeRateSession;
                return exchangeRateSession;
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
