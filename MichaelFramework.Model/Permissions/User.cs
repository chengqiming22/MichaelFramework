using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.ActiveRecord;
using System.Collections;
using Castle.Components.Validator;
using MichaelFramework.Utils;
using NHibernate.Criterion;

namespace MichaelFramework.Model.Permissions
{
    [ActiveRecord("T_User", Cache = CacheEnum.NonStrictReadWrite, CacheRegion = "NHibernate.Cache.HashtableCacheProvider")]
    public class User : PermissionHolder
    {
        [HasAndBelongsToMany(typeof(Permission), Table = "T_User_Permission",
            ColumnKey = "a_user_id", ColumnRef = "a_permission_id", Cascade = ManyRelationCascadeEnum.AllDeleteOrphan)]
        public IList<Permission> Permissions
        {
            get
            {
                if (permissions == null) permissions = new List<Permission>();
                return permissions;
            }
            set { permissions = value; }
        }

        /// <summary>
        /// 加密后的密码，该属性应只在查询时使用
        /// </summary>
        [Property("a_password")]
        public string Password { get; set; }

        /// <summary>
        /// 获取或设置明文密码，查询时应用Password属性
        /// </summary>
        public string PlainPassword
        {
            get { return CommonHelper.Decript(Password, "upwd"); }
            set { Password = CommonHelper.Encript(value, "upwd"); }
        }

        private IList<Role> roles = null;
        [HasAndBelongsToMany(typeof(Role), Table = "T_User_Role",
            ColumnKey = "a_user_id", ColumnRef = "a_role_id")]
        public IList<Role> Roles
        {
            get
            {
                if (roles == null) roles = new List<Role>();
                return roles;
            }
            set { roles = value; }
        }

        private IList<Group> groups = null;
        [HasAndBelongsToMany(typeof(Group), Table = "T_User_Group",
            ColumnKey = "a_user_id", ColumnRef = "a_group_id")]
        public IList<Group> Groups
        {
            get
            {
                if (groups == null) groups = new List<Group>();
                return groups;
            }
            set { groups = value; }
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
            if(r == eCheckPermissionResult.Undistributed)
                r = groupsHasPermission(this.Groups, auth, entityLevel,
                       classId, entityId, propertyId, ops);
            return r;
        }

        # region 实例化当前用户

        public static event EventHandler<CurrentUserChangingEventArgs> CurrentUserChanging;
        public static event EventHandler CurrentUserChanged;

        private static User currentUser = null;
        public static User CurrentUser
        {
            get { return currentUser; }
            set
            {
                if (value == null && currentUser == null || value != null && currentUser != null && value.Id == currentUser.Id)
                    return;
                CurrentUserChangingEventArgs e = new CurrentUserChangingEventArgs() { OldUser = currentUser, NewUser = value, Cancel = false };
                if (CurrentUserChanging != null)
                    CurrentUserChanging(null, e);
                if (!e.Cancel)
                {
                    currentUser = value;
                    if (CurrentUserChanged != null)
                        CurrentUserChanged(null, EventArgs.Empty);
                }
            }
        }

        public static User Validate(string username, string pwd)
        {
            return ActiveRecordMediator<User>.FindFirst(
                    Expression.Eq("Name", username), Expression.Eq("Password", CommonHelper.Encript(pwd, "upwd"))) as User;
        }

        # endregion
    }

    public class CurrentUserChangingEventArgs:EventArgs
    {
        public User OldUser { get; set; }

        public User NewUser { get; set; }

        public bool Cancel { get; set; }

        public CurrentUserChangingEventArgs()
        {

        }
    }
}
