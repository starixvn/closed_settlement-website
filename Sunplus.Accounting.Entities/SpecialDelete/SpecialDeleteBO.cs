using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Sunplus.Accounting.Entities.SpecialDelete
{
    #region SpecialDeleteBO
    [Serializable]
    [DataContract]
    public class SpecialDeleteBO
    {
        [DataMember]
        public string Type { get; set; }

        [DataMember]
        public int Order { get; set; }

        [DataMember]
        public string Account { get; set; }

        [DataMember]
        public string Refno { get; set; }

        [DataMember]
        public string Bettype{ get; set; }

        [DataMember]
        public string Betteam { get; set; }

        [DataMember]
        public string Odds { get; set; }

        [DataMember]
        public string OddType { get; set; }

        [DataMember]
        public string Amount { get; set; }

        [DataMember]
        public string Currency { get; set; }

        [DataMember]
        public string Hdp{ get; set; }

        [DataMember]
        public string TransTime { get; set; }

        [DataMember]
        public string Score{ get; set; }

        [DataMember]
        public string Creator{ get; set; }

        [DataMember]
        public string Status{ get; set; }

        [DataMember]
        public string IsDelete { get; set; }

        [DataMember]
        public string IsMP { get; set; }

        [DataMember]
        public int SaFilter { get; set; }
    }
    #endregion

    #region SpecialDeleteParam
    [Serializable]
    [DataContract]
    public class SpecialDeleteParam
    {
        [DataMember]
        public string MatchId { get;set; }

        [DataMember]
        public string FromRefNo { get; set; }

        [DataMember]
        public string ToRefNo { get; set; }

        [DataMember]
        public string Hdp { get; set; }

        [DataMember]
        public string Liveindicator { get; set; }

        [DataMember]
        public string Bettypes { get; set; }

        [DataMember]
        public string Betteam1378 { get; set; }

        [DataMember]
        public string Betteam515 { get; set; }

        [DataMember]
        public string Betteam4 { get; set; }

        [DataMember]
        public string Betteam16 { get; set; }

        [DataMember]
        public string Betteam6 { get; set; }

        [DataMember]
        public string Betteam14 { get; set; }

        [DataMember]
        public string Type { get; set; }

        [DataMember]
        public string HomeScore { get; set; }

        [DataMember]
        public string AwayScore { get; set; }

        [DataMember]
        public string FromTransdate { get; set; }

        [DataMember]
        public string ToTransdate { get; set; }

        [DataMember]
        public string Betteam430 { get; set; } // update for 1 first correct score

        [DataMember]
        public int SelectBettype { get; set; }

        [DataMember]
        public int SAFilter { get; set; }
        
    }
    /// <summary>
    /// SpecialDeleteParamEarlyKO
    /// </summary>
    [Serializable]
    [DataContract]
    public class SpecialDeleteParamEarlyKO : SpecialDeleteParam
    {
        [DataMember]
        public string KichOffDate { get; set; }
    }
    [DataContract]
    public class SpecialDeleteParamPager: SpecialDeleteParam
    {
        [DataMember]
        public int Page { get; set; }
        [DataMember]
        public int Counter { get; set; }
        [DataMember]
        public bool Refresh { get; set; }
      

    }
    #endregion

    #region DataforMailing
    public class SpecialDeleteDataforMailing
    {
        public DataTable Items { get; set; }

        public string Name { get; set; }

        public string To { get; set; }

        public string Cc { get; set; }

        public string Bcc { get; set; }

        public string Alias { get; set; }
    }

    public class SpecialDeleteDataforMaillings
    {
        public List<SpecialDeleteDataforMailing> Items { get; set; }
    }
    #endregion

    #region SpecialDeleteBO EarlyKO
    [DataContract]
    public class SpecialDeleteBetEarlyKO : SpecialDeleteBO
    {
        [DataMember]
        public string TransId { get; set; }

        [DataMember]
        public string SubTransId { get; set; }

        [DataMember]
        public string CurrencyId { get; set; }

        [DataMember]
        public int ParlayType { get; set; }
    }
    #endregion

    #region Bettype 
    [Serializable]
    [DataContract]
    public class Bettype
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Name{ get; set; }

        [DataMember]
        public bool Liveindicator { get; set; }
    }
    #endregion

}
