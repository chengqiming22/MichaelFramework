using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.ActiveRecord;

namespace MichaelFramework.Model.Permissions
{
    public enum eExpression
    {
        Between=0,
        LessThan,
        LessThanOrEqual,
        Equal,
        GreaterThanOrEqual,
        GreaterThan
    }

    [ActiveRecord("T_Condition")]
    public class Condition : ObjectBase
    {
        [Property("a_property_ame")]
        public string PropertyName { get; set; }

        [Property("a_expression")]
        public eExpression Expression { get; set; }
        
        [Property("a_value1")]
        public string Value1 { get; set; }

        [Property("a_value2")]
        public string Value2 { get; set; }
    }
}
