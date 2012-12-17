using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.ActiveRecord;

namespace MichaelFramework.Model.Permissions
{
    public enum eAuthorization
    {
        Allow = 0,
        Deny,
    }

    public enum eOperation
    {
        None=0,
        Read,
        Add,
        Edit,
        Delete
    }

    [ActiveRecord("T_Permission")]
    public class Permission : ObjectBase
    {
        [Nested]
        public Entity Entity { get; set; }

        [Property("a_operation")]
        public eOperation Operation { get; set; }

        private IList<Condition> conditions = null;
        [HasAndBelongsToMany(typeof(Condition), Table = "T_Permission_Condition",
            ColumnKey = "a_permission_id", ColumnRef = "a_condition_id",Cascade=ManyRelationCascadeEnum.All)]
        public IList<Condition> Conditions
        {
            get
            {
                if (conditions == null) conditions = new List<Condition>();
                return conditions;
            }
            set { conditions = value; }
        }

        [Property("a_authorization")]
        public eAuthorization Authorization { get; set; }

        [Property("a_with_grantpermission")]
        public bool WithGrantPermission { get; set; }
    }
}
