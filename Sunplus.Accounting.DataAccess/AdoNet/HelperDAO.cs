using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DNAide.Data;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.DataAccess.IBCDLL.Customer;
using Sunplus.Accounting.Entities;
using System.Data;
using Sunplus.Accounting.Core.Configurations;
using Sunplus.Accounting.Core.Configurations.StoreProcedureGroup;

namespace Sunplus.Accounting.DataAccess.AdoNet
{
    public class HelperDAO:DataAccess, IHelperDAO
    {
        public IEnumerable<Country> GetCountryList()
        {
            var countries = new List<Country>();
            var spElement =
                GetStoreProcedure(Sunplus.Accounting.Core.Configurations.StoreProcedureGroup.HelperSession.StoreCollection.HelperControl,
                                  "GetCountries");
            var db = GetDatabase(spElement.DbName);
            using (var dbCommand = db.GetStoredProcCommand(spElement.Name))
            {
                //db.AddInParameter(dbCommand, "UserID", DbType.Int32, userId);
                var reader = db.ExecuteReader(dbCommand);

                if (reader != null)
                {
                    while (reader.Read())
                    {
                        var entity = new Country()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name"))
                        };
                        countries.Add(entity);
                    }
                }
            }
            return countries;
        }

        public Result AddCountry(string countryName)
        {
            var result = new Result();
           
            var spElement =
                GetStoreProcedure(Sunplus.Accounting.Core.Configurations.StoreProcedureGroup.HelperSession.StoreCollection.HelperControl,
                                  "InsertCountry");
            var db = GetDatabase(spElement.DbName);
            using (var dbCommand = db.GetStoredProcCommand(spElement.Name))
            {
                db.AddInParameter(dbCommand, "name", DbType.String, countryName);
                db.AddOutParameter(dbCommand, "ErrCode", DbType.Int32, 4);
                db.AddOutParameter(dbCommand, "ErrMsg", DbType.String, 4000);

                int numberOfRecordsAffected = db.ExecuteNonQuery(dbCommand);

                result.ErrorCode = Convert.ToInt32(db.GetParameterValue(dbCommand, "ErrCode"));
                result.ErrorMessage = Convert.ToString(db.GetParameterValue(dbCommand, "ErrMsg"));
            }

            return result;
        }

        public DataTable GetSites(Enumerations.AccountingSystem system)
        {
            DbResult ds = null;


            var param = new Hashtable {{"system", system.ToString().ToUpper()}};
            var spElement =
                GetStoreProcedure(HelperSession.StoreCollection.HelperControl, "SiteSel");
            ds = DbHelper.Execute(spElement.Name, param, spElement.DbName);

            DataTable result = null;
            if (ds != null && ds.ResultSet != null && ds.ResultSet.Tables.Count > 0)
                result = ds.ResultSet.Tables[0];
            return result;
        }

        public DataTable GetCust(Enumerations.AccountingSystem system)
        {
            throw new NotImplementedException();
        }
    }
}
