using System;
using System.Collections.Generic;
using System.Reflection;
using System.ComponentModel;

namespace MichaelFramework.Model
{
    public abstract class UndoableObjectBase : UndoableBase
    {
        private class PropertyValue
        {
            public object OldValue { get; set; }
            public object NewValue { get; set; }
        }

        # region 构造函数

        public UndoableObjectBase()
        {
            undoStack = new Stack<KeyValuePair<PropertyInfo, PropertyValue>>();
            redoStack = new Stack<KeyValuePair<PropertyInfo, PropertyValue>>();
        }

        # endregion

        # region 状态操作

        private Stack<KeyValuePair<PropertyInfo, PropertyValue>> undoStack = null;

        private Stack<KeyValuePair<PropertyInfo, PropertyValue>> redoStack = null;

        public void ClearStateTrace()
        {
            undoStack.Clear();
            redoStack.Clear();
        }

        [Browsable(false)]
        public bool Undoable { get { return undoStack.Count > 0; } }

        [Browsable(false)]
        public bool Redoable { get { return redoStack.Count > 0; } }

        # endregion

        # region Do,Undo,Redo

        private PropertyValue curPropertyValue = null;
        protected override void OnPropertyChanging(PropertyChangingEventArgs e)
        {
            base.OnPropertyChanging(e);
            if (editing && !e.Cancel)
            {
                curPropertyValue = new PropertyValue();
                curPropertyValue.OldValue = e.PropertyValue;
            }
        }

        protected override void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (editing && curPropertyValue != null)
            {
                curPropertyValue.NewValue = e.PropertyValue;
                redoStack.Clear();
                undoStack.Push(new KeyValuePair<PropertyInfo, PropertyValue>(
                    this.GetType().GetProperty(e.PropertyName), curPropertyValue));
            }
            base.OnPropertyChanged(e);
        }

        private bool editing = true;

        public bool Undo()
        {
            if (!Undoable) return false;

            editing = false;

            KeyValuePair<PropertyInfo, PropertyValue> state = undoStack.Pop();
            if (state.Value.OldValue.GetType().IsGenericType &&
                state.Value.OldValue.GetType().GetGenericTypeDefinition()
                == typeof(UndoableObjectListBase<>).GetGenericTypeDefinition())
                state.Value.OldValue.GetType().GetMethod("Undo").Invoke(state.Value.OldValue,null);
            else
                state.Key.SetValue(this, state.Value.OldValue, null);

            redoStack.Push(state);

            editing = true;

            return true;
        }

        public bool Redo()
        {
            if (!Redoable) return false;
            editing = false;

            KeyValuePair<PropertyInfo, PropertyValue> state = redoStack.Pop();
            if (state.Value.NewValue.GetType().IsGenericType && 
                state.Value.NewValue.GetType().GetGenericTypeDefinition()
                == typeof(UndoableObjectListBase<>).GetGenericTypeDefinition())
                state.Value.NewValue.GetType().GetMethod("Redo").Invoke(state.Value.NewValue, null);
            else
                state.Key.SetValue(this, state.Value.NewValue, null);

            undoStack.Push(state);

            editing = true;

            return true;
        }

        # endregion
    }
}
