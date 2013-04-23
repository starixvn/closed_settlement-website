using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fanex.Tarus.Infrastructure.Helper.SportHelper
{
    public static class CalcOdds
    {
        #region Bettypes
        static List<int> Bettypes
        {
            get
            {
                List<int> bList = new List<int>() {1, 2, 3, 7, 12, 8, 20, 124, 125};

                for (int i = 81; i <= 88; i++)
                {
                    bList.Add(i);
                }


                for (int i = 90; i <= 120; i++)
                {
                    bList.Add(i);
                }

                return bList;
            }
        }
        #endregion

        /// <summary>
        /// f_GetOddsByBettype
        /// </summary>
        /// <param name="odds"></param>
        /// <param name="bettype"></param>
        /// <param name="oddsType"></param>
        /// <returns></returns>
        public static decimal Odds(decimal odds, int bettype, int oddsType)
        {
            decimal oddsValue = odds;
            var bettypes = Bettypes;
            if (bettypes.Contains(bettype) && oddsType == 1)
            {
                oddsValue = odds + 1;
            }
            else if (bettypes.Contains(bettype) && oddsType == 5)
            {
                oddsValue = odds*100;
            }
           
            return oddsValue;
        }

        public static int OddsType (int bettype, int oddsType)
        {
            int _oddsType = 0;
            var bettypes = Bettypes;
            if(bettypes.Contains(bettype))
            {
                _oddsType = oddsType;
            }
            return _oddsType;
        }
    }
}
