using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.ActiveRecord;
using System.Reflection;
using Castle.Components.Validator;
using System.ComponentModel;

namespace MichaelFramework.Model.Permissions
{
    public enum eCheckPermissionResult
    {
        Yes = 0,
        No,
        Undistributed
    }

    public class PermissionHolder : ObjectBase
    {
        [DisplayName("名称"), Category("基本信息")]
        [Property("a_name", Unique = true), ValidateIsUnique("该名称已存在")]
        public string Name { get; set; }

        [DisplayName("描述"), Category("基本信息"), Property("a_description")]
        public string Description { get; set; } 

        protected IList<Permission> permissions;

        private bool checkExpression(eExpression exp, object value, string value1, string value2)
        {
            IComparable v = value as IComparable;
            IComparable v1 = value1, v2 = value2;
            if (value is Int32)
            {
                v1 = Int32.Parse(value1);
                if (!string.IsNullOrEmpty(value2))
                    v2 = Int32.Parse(value2);
            }
            else if (value is Double)
            {
                v1 = Double.Parse(value1);
                if (!string.IsNullOrEmpty(value2))
                    v2 = Double.Parse(value2);
            }
            else if (value is DateTime)
            {
                v1 = DateTime.Parse(value1);
                if (!string.IsNullOrEmpty(value2))
                    v2 = DateTime.Parse(value2);
            }
            int n1 = v.CompareTo(v1);
            int n2 = v.CompareTo(v2);
            switch (exp)
            {
                case eExpression.Between:
                    if (n1 >= 0 && n2 <= 0) return true;
                    break;
                case eExpression.LessThan:
                    if (n1 < 0) return true;
                    break;
                case eExpression.LessThanOrEqual:
                    if (n1 <= 0) return true;
                    break;
                case eExpression.Equal:
                    if (n1 == 0) return true;
                    break;
                case eExpression.GreaterThanOrEqual:
                    if (n1 >= 0) return true;
                    break;
                case eExpression.GreaterThan:
                    if (n2 > 0) return true;
                    break;
            }

            return false;
        }

        private eCheckPermissionResult InterpretPermissionConditions(
            IEnumerable<Permission> conditionalList, eAuthorization auth,
            eEntityLevel entityLevel, string classId,string entityId)
        {
            if (((int)entityLevel) <= 1)
                return eCheckPermissionResult.Undistributed;

            Type type = Type.GetType(classId);
            if (type == null)
                return eCheckPermissionResult.Undistributed;

            object entity = null;
            PropertyInfo p = null;
            object value = null;

            entity = type.InvokeMember("Find",BindingFlags.Public | BindingFlags.Static |
                  BindingFlags.FlattenHierarchy | BindingFlags.InvokeMethod,
                  null, null, new object[] { new Guid(entityId) });

            int allowCount = 0;
            int denyCount = 0;
            foreach (Permission pm in conditionalList)
            {
                bool conditionMeet = true;
                foreach (Condition c in pm.Conditions)
                {
                    p = type.GetProperty(c.PropertyName, BindingFlags.Public |
                         BindingFlags.Instance | BindingFlags.GetProperty | BindingFlags.SetProperty);
                    value = p.GetValue(entity, null);
                    if (!checkExpression(c.Expression, value, c.Value1, c.Value2))
                    {
                        conditionMeet = false;
                        break;
                    }
                }
                if (conditionMeet)
                {
                    if (pm.Authorization == eAuthorization.Deny && auth == eAuthorization.Allow)
                        return eCheckPermissionResult.No;
                    if (pm.Authorization == eAuthorization.Allow) allowCount++;
                    else if (pm.Authorization == eAuthorization.Deny) denyCount++;
                }
            }
            if (denyCount == 0 && allowCount == 0)
                return eCheckPermissionResult.No;

            if (auth == eAuthorization.Allow && denyCount > 0
                || auth == eAuthorization.Deny && denyCount == 0 && allowCount > 0)
                return eCheckPermissionResult.No;
            if (auth == eAuthorization.Allow && denyCount == 0 && allowCount > 0)
                return eCheckPermissionResult.Yes;

            return eCheckPermissionResult.Undistributed;
        }

        public virtual eCheckPermissionResult HasPermission(eAuthorization auth,
            eEntityLevel entityLevel, string classId,
            string entityId, string propertyId, params eOperation[] ops)
        {
            if (permissions == null || permissions.Count == 0)
                return eCheckPermissionResult.Undistributed;
            int level = (int)entityLevel;
            foreach (eOperation op in ops)
            {
                IEnumerable<Permission> list = null;
                IEnumerable<Permission> conditionalList = null;

                list = permissions.Where(p => p.Entity.EntityLevel == entityLevel && p.Operation == op);
                if (level >= 1) list = list.Where(p => p.Entity.ClassId == classId);
                if (level >= 2) list = list.Where(p => p.Entity.EntityId == entityId||p.Conditions!=null && p.Conditions.Count > 0);
                if (level >= 3) list = list.Where(p => p.Entity.PropertyId == propertyId);

                if (list.Count() == 0)
                {
                    if (entityLevel == eEntityLevel.Global)
                        return eCheckPermissionResult.Undistributed;
                    eCheckPermissionResult r = HasPermission(auth, entityLevel - 1,
                        classId, entityId, propertyId, op);
                    if (r != eCheckPermissionResult.Yes)
                        return r;
                }
                else
                {
                    conditionalList =list.Where(p => p.Conditions !=null && p.Conditions.Count > 0);
                    list = list.Except(conditionalList);

                    int denyCount = list.Count(p => p.Authorization == eAuthorization.Deny);
                    if (list.Count() > 0)
                    {
                        if (auth == eAuthorization.Deny && denyCount == 0
                           || auth == eAuthorization.Allow && denyCount > 0)
                            return eCheckPermissionResult.No;
                    }
                    else
                    {
                        eCheckPermissionResult result=InterpretPermissionConditions(
                            conditionalList,auth,entityLevel,classId,entityId);
                        if (result != eCheckPermissionResult.Yes)
                            return result;
                    }
                }
            }
            return eCheckPermissionResult.Yes;
        }

        protected eCheckPermissionResult rolesHasPermission(IList<Role> roles,
            eAuthorization auth, eEntityLevel entityLevel, string classId,
            string entityId, string propertyId, params eOperation[] ops)
        {
            if (roles == null || roles.Count == 0)
                return eCheckPermissionResult.Undistributed;
            int yes = 0;
            foreach (Role role in roles)
            {
                eCheckPermissionResult temp = role.HasPermission(
                    auth, entityLevel, classId, entityId, propertyId, ops);
                if (temp == eCheckPermissionResult.No)
                    return eCheckPermissionResult.No;
                if (temp == eCheckPermissionResult.Yes)
                    yes++;
            }
            if (yes > 0)
                return eCheckPermissionResult.Yes;
            return eCheckPermissionResult.Undistributed;
        }

        protected eCheckPermissionResult groupsHasPermission(IList<Group> groups,
           eAuthorization auth, eEntityLevel entityLevel, string classId,
           string entityId, string propertyId, params eOperation[] ops)
        {
            if (groups == null || groups.Count == 0)
                return eCheckPermissionResult.Undistributed;
            int yes = 0;
            foreach (Group g in groups)
            {
                eCheckPermissionResult temp = g.HasPermission(
                    auth, entityLevel, classId, entityId, propertyId, ops);
                if (temp == eCheckPermissionResult.No)
                    return eCheckPermissionResult.No;
                if (temp == eCheckPermissionResult.Yes)
                    yes++;
            }
            if (yes > 0)
                return eCheckPermissionResult.Yes;
            return eCheckPermissionResult.Undistributed;
        }
    }
}
