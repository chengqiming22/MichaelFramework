using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.ActiveRecord;
using System.ComponentModel;

namespace MichaelFramework.Model.Permissions
{
    [ActiveRecord("T_Role")]
    public class Role : PermissionHolder
    {
        [Browsable(false), HasAndBelongsToMany(typeof(Permission), Table = "T_Role_Permission",
            ColumnKey = "a_role_id", ColumnRef = "a_permission_id", Cascade = ManyRelationCascadeEnum.AllDeleteOrphan)]
        public IList<Permission> Permissions
        {
            get
            {
                if (permissions == null) permissions = new List<Permission>();
                return permissions;
            }
            set { permissions = value; }
        }

        private IList<Role> roles = null;
        [Browsable(false), HasAndBelongsToMany(typeof(Role), Table = "T_Role_Role",
            ColumnKey = "a_role_id1", ColumnRef = "a_role_id2")]
        public IList<Role> Roles
        {
            get
            {
                if (roles == null) roles = new List<Role>();
                return roles;
            }
            set { roles = value; }
        }

        public override eCheckPermissionResult HasPermission(eAuthorization auth,
            eEntityLevel entityLevel, string classId, string entityId,
            string propertyId, params eOperation[] ops)
        {
            eCheckPermissionResult r = base.HasPermission(auth, entityLevel,
                classId, entityId, propertyId, ops);
            if (r == eCheckPermissionResult.Undistributed)
                r = rolesHasPermission(this.Roles, auth, entityLevel,
                       classId, entityId, propertyId, ops);
            return r;
        }

        [Browsable(false)]
        public IEnumerable<Role> AllRoles
        {
            get
            {
                IEnumerable<Role> list = new List<Role>(Roles);
                foreach (Role r in Roles)
                    list = list.Union(r.AllRoles);
                return list;
            }
        }
    }
}
