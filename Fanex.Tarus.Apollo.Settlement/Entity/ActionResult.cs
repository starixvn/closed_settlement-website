using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fanex.Tarus.Apollo.Settlement.Entity
{
    public class ActionResult
    {
        public ActionResult()
        {
            ErrCode = 0;
            ErrMsg = string.Empty;
        }
        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }
    }
}
