using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Sunplus.Accounting.DataAccess;
using Sunplus.Accounting.Entities;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Business.BootStrappers;
using Sunplus.Accounting.Core.Configurations;

namespace Sunplus.Accounting.Business
{
    public class Helper : IHelper 
    {
        private IHelperDAO _helperDao;
        public Helper()
        {
            _helperDao = CommonBootStrapper.Locator.GetInstance<IHelperDAO>();    
        }

        

        public Helper(IHelperDAO helperDao)
        {
            _helperDao = helperDao;
        }

        public List<SiteDTO> GetSites(Enumerations.AccountingSystem system)
        {
            var listSiteDTO = new List<SiteDTO>();
            var licenseSite = new SiteDTO();
            licenseSite.Name = "Licensee";
            var creditSite = new SiteDTO();
            creditSite.Name = "Credit";
            var result = _helperDao.GetSites(system);
            //throw new NotImplementedException();
            var listLicense = new List<Site>();
            var listCredit = new List<Site>();
            foreach (DataRow row in result.Rows)
            {
                
                if(row["islicensee"].ToString() == "True")
                {
                    
                    var license = new Site();
                    license.Id = int.Parse(row[0].ToString());
                    license.Name = row[1].ToString();
                    listLicense.Add(license);
                    //licenseeSite.Sites = listSite;
                }
                else
                {
                    var credit = new Site();
                    credit.Id = int.Parse(row[0].ToString());
                    credit.Name = row[1].ToString();
                    listCredit.Add(credit);
                }
            }
            licenseSite.Sites = listLicense;
            creditSite.Sites = listCredit;
            if (creditSite.Sites.Count != 0)
                listSiteDTO.Add(creditSite);
            if(licenseSite.Sites.Count != 0)
                listSiteDTO.Add(licenseSite);
            
            return listSiteDTO;
        }


        public List<Site> GetSiteLicensee()
        {
            List<Site> sites = new List<Site>();
            
            return sites;
        }

        public string[] GetStandardCurrencyList()
        {
            List<string> listStandardCurrencyList = new List<string>();
            var helperCollection = HelperSection.Instance[HelperSection.HelperCollectionType.Currency]["StandardCurrencies"].Items;

            foreach (HelperElement helperElement in helperCollection)
            {
                listStandardCurrencyList.Add(helperElement.Value);
            }

            string[] result = listStandardCurrencyList.ToArray();
            Array.Sort(result);
            return result;
        }
    }
}
