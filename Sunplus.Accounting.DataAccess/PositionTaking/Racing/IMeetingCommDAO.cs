using DNAide.Data;

namespace Sunplus.Accounting.DataAccess.PositionTaking.Racing
{
    public interface IMeetingCommDAO
    {
        DbResult GetPartnerList();

        DbResult GetPartnerDetail(int partnerId);
    }
}