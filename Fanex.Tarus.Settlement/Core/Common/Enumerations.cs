// -----------------------------------------------------------------------
// <copyright file="Enumerations.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Tarus.Settlement.Core.Common
{

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class Enumerations
    {
        public enum Bettype
        {
            PartyBaccarat = 1101,
            PartyRoutle = 1102,
            LiveBaccarat = 1103,
            LiveRoutle = 1104,
        }

        public class GameType
        {
            public static string PartyBaccarat = "L0001";
            public static string PartyRoutle = "L0003";
            public static string LiveBaccarat = "L0006";
            public static string LiveRoutle = "L0006";
        }
    }
}
