using System;
using Sunplus.Data.DataAbstract.Entity;

namespace Sunplus.Data.DataAbstract.Queue
{
    public enum ItemType : int
    {
        Bettrans = 1,
        Match,
        League,
        Sport,
        Bettype,
        Team,
        Outright,
        Exchange
    }

    public enum ActionType : byte
    {
        Insert = 1,
        Update,
        Delete
    }

    [Serializable]
    public class ItemQueue
    {
        public ActionType Action { get; set; }
        public ItemType Type { get; set; }
        public byte[] Data { get; set; }
    }

    [Serializable]
    public class ItemNotify
    {
        public ActionType Action { get; set; }
        public BetListDetail Data { get; set; }
    }

    [Serializable]
    public class ItemMonitor
    {
        public ActionType Action { get; set; }
        public object Data { get; set; }
    }

    [Serializable]
    public class QueueData
    {
        public ActionType Action { get; set; }
        public ItemType Type { get; set; }
        public Bettran Bettran { get; set; }
        public Match Match { get; set; }
        public League League { get; set; }
        public BetType Bettype { get; set; }
        public Sport Sport { get; set; }
        public Team Team { get; set; }
        public Outright OutRight { get; set; }
        public Exchange Exchange { get; set; }
    }

    [Serializable]
    public class ItemSocket
    {
        public ActionType Action { get; set; }
        public ItemType Type { get; set; }
        public string Data { get; set; }
    }
}
