using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.ActiveRecord;
using System.ComponentModel;

namespace MichaelFramework.Model.Permissions
{
    [ActiveRecord("T_Group")]
    public class Group : PermissionHolder
    {
        [Browsable(false), HasAndBelongsToMany(typeof(Permission), Table = "T_Group_Permission",
            ColumnKey = "a_group_id", ColumnRef = "a_permission_id",Cascade=ManyRelationCascadeEnum.AllDeleteOrphan)]
        public IList<Permission> Permissions
        {
            get 
            { 
                if (permissions == null)permissions = new List<Permission>(); 
                return permissions; 
            }
            set { permissions = value; }
        }

        private IList<Role> roles = null;
        [Browsable(false), HasAndBelongsToMany(typeof(Role), Table = "T_Group_Role",
            ColumnKey = "a_group_id", ColumnRef = "a_role_id")]
        public IList<Role> Roles 
        {
            get 
            {
                if (roles == null) roles = new List<Role>();
                return roles;
            }
            set
            {
                roles = value;
            }
        }

        private IList<User> users = null;
        [Browsable(false), HasAndBelongsToMany(typeof(User), Table = "T_User_Group",
            ColumnKey = "a_group_id", ColumnRef = "a_user_id")]
        public IList<User> Users 
        {
            get 
            {
                if (users == null) users = new List<User>();
                return users;
            }
            set { users = value; }
        }

        public override eCheckPermissionResult HasPermission(eAuthorization auth,
            eEntityLevel entityLevel, string classId, string entityId, 
            string propertyId, params eOperation[] ops)
        {
            eCheckPermissionResult r = base.HasPermission(auth, entityLevel, 
                classId,entityId, propertyId, ops);
            if (r == eCheckPermissionResult.Undistributed)
                r = rolesHasPermission(this.Roles, auth, entityLevel,
                    classId, entityId, propertyId, ops);
            return r;
        }
    }
}
