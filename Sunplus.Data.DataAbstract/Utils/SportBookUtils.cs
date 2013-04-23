using System;
using System.Linq;
using DNAide.Core;
using Norm;
using Sunplus.Data.DataAbstract.BootStrappers;
using Sunplus.Data.DataAbstract.Configurations;
using Sunplus.Data.DataAbstract.Entity;
using Sunplus.Data.DataAbstract.Log;
using Sunplus.Data.DataAbstract.Storage;

namespace Sunplus.Data.DataAbstract.Utils
{
    public static class SportBookUtils
    {
        private static IDataStorage DataStorage = null;
        private static NoSQLElement NoSqlElement = null;



        public static string GetOddName(string oddType)
        {
            try
            {
                if (string.IsNullOrEmpty(oddType))
                    return string.Empty;
               return RemotingRef.RefData.GetOddsTypeName(oddType,"en");
            }
            catch (Exception exception)
            {

                Error.WriteLog(exception);
                return string.Empty;
            }



        }

        public static string GetTeamName(string teamId)
        {
            try
            {
                if (string.IsNullOrEmpty(teamId))
                    return string.Empty;
                //var teamQuery = Database.GetCollection<Team>().AsQueryable();
                //var teamname = teamQuery.Where(t => t.Teamid == teamId).Select(t => t.Teamname).FirstOrDefault();
                //return teamname;


                var result = RemotingRef.RefData.GetTeamName(teamId, "en"); //DataStorage.Select<Team>(teamId).Teamname;
                return result;

            }
            catch (Exception exception)
            {
                
                Error.WriteLog(exception);
                return string.Empty;
            }

            

        }


        public static string GetLeagueName(string leagueId)
        {
            try
            {
                if (string.IsNullOrEmpty(leagueId))
                    return string.Empty;

                var result = RemotingRef.RefData.GetLeagueName(leagueId, "en"); //DataStorage.Select<League>(leagueId).Leaguename;

                return result;
            }
            catch (Exception exception)
            {

                Error.WriteLog(exception);
                return string.Empty;
            }
            
        }

        public static string GetSportName(string sportId)
        {
            try
            {
                if (string.IsNullOrEmpty(sportId))
                    return string.Empty;
                var result = RemotingRef.RefData.GetSportName(sportId, "en"); //DataStorage.Select<Sport>(sportId).SportName;
                return result;
            }
            catch (Exception exception)
            {

                Error.WriteLog(exception);
                return string.Empty;
            }
           
        }


        public static string GetBetTypeName(string bettypeId)
        {
            try
            {
                if (string.IsNullOrEmpty(bettypeId))
                    return string.Empty;
              
                var result = RemotingRef.RefData.GetBetTypeName(bettypeId, "en"); //DataStorage.Select<BetType>(bettypeId).Typenamee;
                return result;

            }
            catch (Exception exception)
            {

                Error.WriteLog(exception);
                return string.Empty;
            }
           
        }

       
        public static string GetCurrencyName(int currencyId)
        {
            return string.Empty;
        }

       
       
        
    }
}
