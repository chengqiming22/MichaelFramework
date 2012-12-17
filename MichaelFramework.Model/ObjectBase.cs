using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Castle.ActiveRecord;
using Castle.Components.Validator;
using System.Reflection;
using Castle.ActiveRecord.Framework;

namespace MichaelFramework.Model
{
    abstract public class ObjectBase:ActiveRecordValidationBase<ObjectBase>
    {
        [Browsable(false), PrimaryKey("a_id")]
        public virtual Guid Id { get; set; }

        [Browsable(false), Version("a_version")]
        public virtual Int32 Version { get; set; }

        [Browsable(false)]
        public override IDictionary PropertiesValidationErrorMessages
        {
            get
            {
                return base.PropertiesValidationErrorMessages;
            }
        }

        [Browsable(false)]
        public override string[] ValidationErrorMessages
        {
            get
            {
                return base.ValidationErrorMessages;
            }
        }

        [Browsable(false)]
        public bool Deleted { get; private set; }

        protected override void OnUpdate()
        {
            base.OnUpdate();
            Deleted = false;
            ObjectBase.FireObjectPropertyChange(this);
        }

        protected override void OnDelete()
        {
            base.OnDelete();
            Deleted = true;
            ObjectBase.FireObjectPropertyChange(this);
            ObjectBase.UnregisterActions(this);
        }

        # region tag registration

        private static Dictionary<object, List<Action>> actions = new Dictionary<object, List<Action>>();

        public static void RegisterAction(ObjectBase obj, Action action)
        {
            if (!actions.ContainsKey(obj))
                actions.Add(obj, new List<Action>());
            if (!actions[obj].Contains(action))
                actions[obj].Add(action);
        }

        public static void UnregisterAction(ObjectBase obj, Action action)
        {
            if (actions.ContainsKey(obj))
            {
                if (actions[obj].Contains(action))
                    actions[obj].Remove(action);
                if (actions[obj].Count == 0)
                    actions.Remove(obj);
            }
        }

        public static void UnregisterActions(ObjectBase obj )
        {
            if (actions.ContainsKey(obj))
            {
                actions.Remove(obj);
            }
        }

        public static void FireObjectPropertyChange(ObjectBase obj)
        {
            if (!actions.ContainsKey(obj)) return;
            actions[obj].ForEach(action => { action.Invoke(); });
        }

        # endregion
    }
}