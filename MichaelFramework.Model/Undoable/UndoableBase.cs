using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Reflection;

namespace MichaelFramework.Model
{
    public abstract class UndoableBase : ObjectBase
    {
        public event EventHandler<PropertyChangingEventArgs> PropertyChanging;

        public event EventHandler<PropertyChangedEventArgs> PropertyChanged;

        protected virtual void OnPropertyChanging(PropertyChangingEventArgs e)
        {
            if (PropertyChanging != null)
                PropertyChanging(this, e);
        }

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, e);
        }

        protected void SetFieldValue(string propertyName, string filedName, object value)
        {
            PropertyInfo pi = this.GetType().GetProperty(propertyName);
            FieldInfo fi = this.GetType().GetField(filedName, BindingFlags.NonPublic
                | BindingFlags.Public | BindingFlags.Instance);
            if (fi.GetValue(this) == value) return;

            PropertyChangingEventArgs e = new PropertyChangingEventArgs() { PropertyName = pi.Name, PropertyValue = pi.GetValue(this, null) };
            OnPropertyChanging(e);
            if (!e.Cancel)
            {
                fi.SetValue(this, value);
                OnPropertyChanged(new PropertyChangedEventArgs() { PropertyName = pi.Name, PropertyValue = pi.GetValue(this, null) });
            }
        }
    }

    public class PropertyChangingEventArgs : EventArgs
    {
        public string PropertyName { get;set; }

        public object PropertyValue { get;set; }

        public bool Cancel { get; set; }
    }

    public class PropertyChangedEventArgs : EventArgs
    {
        public string PropertyName { get; set; }

        public object PropertyValue { get; set; }
    }
}
