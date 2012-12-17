using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.ActiveRecord;

namespace MichaelFramework.Model.Logging
{
    public enum eLogLevel
    {
        System = 0,
        User
    }

    [ActiveRecord("T_Log")]
    public class Log:LogBase
    {
        [Property("a_user_name")]
        public string UserName { get; set; }

        [Property("a_update_date")]
        public DateTime UpdateDate { get; set; }

        [Property("a_level")]
        public eLogLevel Level { get; set; }
    }
}
