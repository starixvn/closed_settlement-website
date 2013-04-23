using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DNAide.Data;

using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.DataAccess.Report;
using Sunplus.Core.LoggingLib;
using Sunplus.Accounting.Core.Configurations.StoreProcedureGroup;

namespace Sunplus.Accounting.DataAccess.IBCDLL.Report
{
    public class NumberGameWinLoss
    {
        public static DbResult WinlossByBetTypeDetail(DateTime wldate, int liveindicator, int bettype)
        {
            DbResult result = null;
            try
            {
                Hashtable p = new Hashtable();
                p.Add("winlostdate", wldate);
                p.Add("liveindicator", liveindicator);
                p.Add("bettype", bettype);
                result = DbHelper.Execute("Acc_Rpt_NumberGame_Details", p, DBType.WAREHOUSE, 300);
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);

            }
            return result;
        }

        public static DbResult WinlossByBetTypeDetail(DateTime wldate, int liveindicator, int bettype, int SiteID, int Currency)
        {
            DbResult result = null;
            try
            {
                Hashtable p = new Hashtable();
                p.Add("winlostdate", wldate);
                p.Add("liveindicator", liveindicator);
                p.Add("bettype", bettype);
                p.Add("siteid", SiteID);
                p.Add("currency", Currency);

                var sp = ReportSession.Instance[ReportSession.StoreCollection.Gaming]["Acc_Rpt_NumberGame_Details"];
                result = DbHelper.Execute(sp.Name, p, sp.DbName, 300);

                /*if (wldate < ReportUtils.BalanceUp.BalanceUpDay)
                {
                    var sp = ReportSession.Instance[ReportSession.StoreCollection.Gaming]["Acc_Rpt_NumberGame_Details_Old"];
                    result = DbHelper.Execute(sp.Name, p, sp.DbName, 300);
                }
                else
                {
                    var sp = ReportSession.Instance[ReportSession.StoreCollection.Gaming]["Acc_Rpt_NumberGame_Details"];
                    result = DbHelper.Execute(sp.Name, p, sp.DbName, 300);
                }*/
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);

            }
            return result;
        }
        public static DbResult WinlossByBetTypeDetail(DateTime wldate, int liveindicator, int bettype, string companies, string currencies)
        {
            DbResult result = null;
            try
            {
                Hashtable p = new Hashtable();
                p.Add("winlostdate", wldate);
                p.Add("liveindicator", liveindicator);
                p.Add("bettype", bettype);
                p.Add("siteid", companies);
                p.Add("currency", currencies);

                var sp = ReportSession.Instance[ReportSession.StoreCollection.Gaming]["Acc_Rpt_NumberGame_Details"];
                result = DbHelper.Execute(sp.Name, p, sp.DbName, 300);
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);

            }
            return result;
        }

        public static DbResult WinlossByBetTypeDetail14(DateTime wldate, int liveindicator, int bettype)
        {
            DbResult result = null;
            try
            {
                Hashtable p = new Hashtable();
                p.Add("winlostdate", wldate);
                p.Add("liveindicator", liveindicator);
                p.Add("bettype", bettype);
                string _DBType = DBType.REPORT;
                result = DbHelper.Execute("Acc_Rpt_NumberGame_Details14", p, _DBType, 300);
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);

            }
            return result;
        }

        public static DbResult WinlossByBetTypeDetail14(DateTime wldate, int liveindicator, int bettype, int SiteID, int Currency)
        {
            DbResult result = null;
            try
            {
                Hashtable p = new Hashtable();
                p.Add("winlostdate", wldate);
                p.Add("liveindicator", liveindicator);
                p.Add("bettype", bettype);
                p.Add("siteid", SiteID);
                p.Add("currency", Currency);

                var sp = ReportSession.Instance[ReportSession.StoreCollection.Gaming]["Acc_Rpt_NumberGame_Details14"];
                result = DbHelper.Execute(sp.Name, p, sp.DbName, 300);

                /*if (wldate < ReportUtils.BalanceUp.BalanceUpDay)
                {
                    var sp = ReportSession.Instance[ReportSession.StoreCollection.Gaming]["Acc_Rpt_NumberGame_Detail_Old"];
                    result = DbHelper.Execute(sp.Name, p, sp.DbName, 300);
                }
                else
                {
                    var sp = ReportSession.Instance[ReportSession.StoreCollection.Gaming]["Acc_Rpt_NumberGame_Details14"];
                    result = DbHelper.Execute(sp.Name, p, sp.DbName, 300);
                }*/
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);

            }
            return result;
        }
        public static DbResult WinlossByBetTypeDetail14(DateTime wldate, int liveindicator, int bettype, string companies, string currencies)
        {
            DbResult result = null;
            try
            {
                Hashtable p = new Hashtable();
                p.Add("winlostdate", wldate);
                p.Add("liveindicator", liveindicator);
                p.Add("bettype", bettype);
                p.Add("siteid", companies);
                p.Add("currency", currencies);

                var sp = ReportSession.Instance[ReportSession.StoreCollection.Gaming]["Acc_Rpt_NumberGame_Details14"];
                result = DbHelper.Execute(sp.Name, p, sp.DbName, 300);
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);

            }
            return result;
        }

        public static DbResult WinlossByBetTypeDetail_Old(DateTime wldate, int liveindicator, int bettype, int SiteID, int Currency)
        {
            DbResult result = null;
            try
            {
                Hashtable p = new Hashtable();
                p.Add("winlostdate", wldate);
                p.Add("liveindicator", liveindicator);
                p.Add("bettype", bettype);
                p.Add("siteid", SiteID);
                p.Add("currency", Currency);

                var sp = ReportSession.Instance[ReportSession.StoreCollection.Gaming]["Acc_Rpt_NumberGame_Detail_Old"];
                result = DbHelper.Execute(sp.Name, p, sp.DbName, 300);

                /*if (wldate < ReportUtils.BalanceUp.BalanceUpDay)
                {
                    var sp = ReportSession.Instance[ReportSession.StoreCollection.Gaming]["Acc_Rpt_NumberGame_Detail_Old"];
                    result = DbHelper.Execute(sp.Name, p, sp.DbName, 300);
                }
                else
                {
                    var sp = ReportSession.Instance[ReportSession.StoreCollection.Gaming]["Acc_Rpt_NumberGame_Details14"];
                    result = DbHelper.Execute(sp.Name, p, sp.DbName, 300);
                }*/
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);

            }
            return result;
        }
        public static DbResult WinlossByBetTypeDetail_Old(DateTime wldate, int liveindicator, int bettype, string companies, string currencies)
        {
            DbResult result = null;
            try
            {
                Hashtable p = new Hashtable();
                p.Add("winlostdate", wldate);
                p.Add("liveindicator", liveindicator);
                p.Add("bettype", bettype);
                p.Add("siteid", companies);
                p.Add("currency", currencies);

                var sp = ReportSession.Instance[ReportSession.StoreCollection.Gaming]["Acc_Rpt_NumberGame_Detail_Old"];
                result = DbHelper.Execute(sp.Name, p, sp.DbName, 300);

            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);

            }
            return result;
        }

        public static DbResult WinlossByBetType(DateTime wldate)
        {
            DbResult result = null;
            try
            {
                Hashtable p = new Hashtable();

                p.Add("winlostdate", wldate);
                string _DBType = (ReportUtils.IsRepDB) ? DBType.REPORT : DBType.MAIN; //DatabaseType.ARCHIVE_1 : DatabaseType.MAIN
                result = DbHelper.Execute("Acc_Rpt_NumberGame", p, _DBType, 300);
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);

            }
            return result;
        }

        public static DbResult WinlossByBetType(DateTime wldate, int Currency, int SiteID)
        {
            DbResult result = null;
            try
            {
                Hashtable p = new Hashtable();

                p.Add("winlostdate", wldate);
                p.Add("siteid", SiteID);
                p.Add("currency", Currency);
                var sp = ReportSession.Instance[ReportSession.StoreCollection.Gaming]["Acc_Rpt_NumberGame"];
                result = DbHelper.Execute(sp.Name, p, sp.DbName, 300);

                /*if (wldate < ReportUtils.BalanceUp.BalanceUpDay)
                {
                    var sp = ReportSession.Instance[ReportSession.StoreCollection.Gaming]["Acc_Rpt_NumberGame_Old"];
                    result = DbHelper.Execute(sp.Name, p, sp.DbName, 300);
                    
                }
                else
                {
                    var sp = ReportSession.Instance[ReportSession.StoreCollection.Gaming]["Acc_Rpt_NumberGame"];
                    result = DbHelper.Execute(sp.Name, p, sp.DbName, 300);
                }*/
                
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);

            }
            return result;
        }

        public static DbResult WinlossByBetType(DateTime wldate, string currencies, string companies)
        {
            DbResult result = null;
            try
            {
                var p = new Hashtable();

                p.Add("winlostdate", wldate);
                p.Add("siteid", companies);
                p.Add("currency", currencies);
                var sp = ReportSession.Instance[ReportSession.StoreCollection.Gaming]["Acc_Rpt_NumberGame"];
                result = DbHelper.Execute(sp.Name, p, sp.DbName, 300);
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);

            }
            return result;
        }
        public static DbResult WinlossByBetType14(DateTime wldate)
        {
            DbResult result = null;
            try
            {
                Hashtable p = new Hashtable();

                p.Add("winlostdate", wldate);
                string _DBType = DBType.REPORT;
                result = DbHelper.Execute("Acc_Rpt_NumberGame14", p, _DBType, 300);
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);

            }
            return result;
        }

        public static DbResult WinlossByBetType14(DateTime wldate, int SiteID, int Currency)
        {
            DbResult result = null;
            try
            {
                Hashtable p = new Hashtable();

                p.Add("winlostdate", wldate);
                p.Add("siteid", SiteID);
                p.Add("currency", Currency);

                var sp = ReportSession.Instance[ReportSession.StoreCollection.Gaming]["Acc_Rpt_NumberGame14"];
                result = DbHelper.Execute(sp.Name, p, sp.DbName, 300);

                /*if (wldate < ReportUtils.BalanceUp.BalanceUpDay)
                {
                    var sp = ReportSession.Instance[ReportSession.StoreCollection.Gaming]["Acc_Rpt_NumberGame_Old"];
                    result = DbHelper.Execute(sp.Name, p, sp.DbName, 300);
                }
                else
                {
                    var sp = ReportSession.Instance[ReportSession.StoreCollection.Gaming]["Acc_Rpt_NumberGame14"];
                    result = DbHelper.Execute(sp.Name, p, sp.DbName, 300);
                }*/
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);

            }
            return result;
        }
        public static DbResult WinlossByBetType14(DateTime wldate, string companies, string currencies)
        {
            DbResult result = null;
            try
            {
                Hashtable p = new Hashtable();

                p.Add("winlostdate", wldate);
                p.Add("siteid", companies);
                p.Add("currency", currencies);

                var sp = ReportSession.Instance[ReportSession.StoreCollection.Gaming]["Acc_Rpt_NumberGame14"];
                result = DbHelper.Execute(sp.Name, p, sp.DbName, 300);
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);

            }
            return result;
        }

        public static DbResult WinlossByBetType_Old(DateTime wldate, int SiteID, int Currency)
        {
            DbResult result = null;
            try
            {
                Hashtable p = new Hashtable();

                p.Add("winlostdate", wldate);
                p.Add("siteid", SiteID);
                p.Add("currency", Currency);

                var sp = ReportSession.Instance[ReportSession.StoreCollection.Gaming]["Acc_Rpt_NumberGame_Old"];
                result = DbHelper.Execute(sp.Name, p, sp.DbName, 300);

                /*if (wldate < ReportUtils.BalanceUp.BalanceUpDay)
                {
                    var sp = ReportSession.Instance[ReportSession.StoreCollection.Gaming]["Acc_Rpt_NumberGame_Old"];
                    result = DbHelper.Execute(sp.Name, p, sp.DbName, 300);
                }
                else
                {
                    var sp = ReportSession.Instance[ReportSession.StoreCollection.Gaming]["Acc_Rpt_NumberGame14"];
                    result = DbHelper.Execute(sp.Name, p, sp.DbName, 300);
                }*/
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);

            }
            return result;
        }
        public static DbResult WinlossByBetType_Old(DateTime wldate, string companies, string currencies)
        {
            DbResult result = null;
            try
            {
                Hashtable p = new Hashtable();

                p.Add("winlostdate", wldate);
                p.Add("siteid", companies);
                p.Add("currency", currencies);

                var sp = ReportSession.Instance[ReportSession.StoreCollection.Gaming]["Acc_Rpt_NumberGame_Old"];
                result = DbHelper.Execute(sp.Name, p, sp.DbName, 300);
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);

            }
            return result;
        }

        public static DbResult SelLicensees()
        {
            DbResult result = null;
            try
            {
                var sp = ReportSession.Instance[ReportSession.StoreCollection.Gaming]["SelLicensees"];
                result = DbHelper.Execute(sp.Name, sp.DbName);
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);

            }
            return result;
        }

        public static DbResult SellAllBettypes()
        {
            DbResult result = null;
            try
            {
                var sp = ReportSession.Instance[ReportSession.StoreCollection.Gaming]["SelAllNumberGameBettypes"];
                result = DbHelper.Execute(sp.Name, sp.DbName);
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);

            }
            return result;
        }
    }
}
