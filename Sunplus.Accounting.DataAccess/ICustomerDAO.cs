using System.Data;
using DNAide.Data;
using Sunplus.Accounting.Core.Configurations;
using Sunplus.Accounting.Entities;
using Sunplus.Accounting.Entities.Customer;

namespace Sunplus.Accounting.DataAccess
{
    public interface ICustomerDAO
    {
        DataTable GetCustomerInfo(string username);

        Result UpdateCustomerSite(int custId, string newSite, int userId, string ip);

        Result UpdateLevelDangerous(int custId, int updateDownline, int dangerType, int dangerLevel, int userId,
                                    string ip);


        DbResult GetAllCustInfo(string username);
        BetSettingNG CheckEnableNGDownline(string username);
        Result UpdateBetSettingNGDownline(string username, int isEnable, string maxBet, string minBet, string maxMatch, string maxBall);
    }
}
