using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.ComponentModel;
using System.Collections.Generic;

public class LogEntity
{
    public int ID
    {
        get
        {
            throw new System.NotImplementedException();
        }
        set
        {
        }
    }

    public DateTime Date
    {
        get
        {
            throw new System.NotImplementedException();
        }
        set
        {
        }
    }

    public int UserId
    {
        get
        {
            throw new System.NotImplementedException();
        }
        set
        {
        }
    }

    public int CustId
    {
        get
        {
            throw new System.NotImplementedException();
        }
        set
        {
        }
    }

    public int ActionId
    {
        get
        {
            throw new System.NotImplementedException();
        }
        set
        {
        }
    }

    public TrackingLog TrackingLog
    {
        get
        {
            throw new System.NotImplementedException();
        }
        set
        {
        }
    }
}

public class TrackingLog
{
    /// <summary>
    /// SP Name
    /// </summary>
    public string SP
    {
        get
        {
            throw new System.NotImplementedException();
        }
        set
        {
        }
    }

    public List<TrackingElement> Element
    {
        get
        {
            throw new System.NotImplementedException();
        }
        set
        {
        }
    }

    public List<TrackingNode> Node
    {
        get
        {
            throw new System.NotImplementedException();
        }
        set
        {
        }
    }
}

public class TrackingNode
{
    /// <summary>
    /// Alias of infomation
    /// </summary>
    public string N
    {
        get
        {
            throw new System.NotImplementedException();
        }
        set
        {
        }
    }

    public List<TrackingElement> Element
    {
        get
        {
            throw new System.NotImplementedException();
        }
        set
        {
        }
    }

    public List<TrakingSport> Sport
    {
        get
        {
            throw new System.NotImplementedException();
        }
        set
        {
        }
    }
}

public class TrackingElement
{
    public string N
    {
        get
        {
            throw new System.NotImplementedException();
        }
        set
        {
        }
    }

    public string Val
    {
        get
        {
            throw new System.NotImplementedException();
        }
        set
        {
        }
    }
}

public class TrakingSport
{
    /// <summary>
    /// Sport Name
    /// </summary>
    public string N
    {
        get
        {
            throw new System.NotImplementedException();
        }
        set
        {
        }
    }

    public TrackingElement Element
    {
        get
        {
            throw new System.NotImplementedException();
        }
        set
        {
        }
    }
}
