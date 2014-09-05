using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIC
{
    public class PropertyChangedEventArgs<T> : PropertyChangedEventArgs
    {
        public PropertyChangedEventArgs(string propertyName, T oldValue, T newValue)
            : base(propertyName)
        {
            this.OldValue = oldValue;
            this.NewValue = newValue;
        }

        public T NewValue { get; private set; }
        public T OldValue { get; private set; }
    }

    public delegate void PropertyChangedEventHandler<T>(object sender, PropertyChangedEventArgs<T> e);

    /// <summary>
    /// PIC属性变化事件
    /// </summary>
    public class PICPropertyChangedEventArgs : PropertyChangedEventArgs
    {

        public PICPropertyChangedEventArgs(string propertyName, object oldValue, object newValue)
            : base(propertyName)
        {
            this.OldValue = oldValue;
            this.NewValue = newValue;
        }

        public object NewValue { get; private set; }
        public object OldValue { get; private set; }
    }

    public delegate void PICPropertyChangedEventHandler(object sender, PICPropertyChangedEventArgs e);

    public interface IPICNotifyPropertyChanged
    {
        // Summary:
        //     Occurs when a property value changes.
        event PICPropertyChangedEventHandler PICPropertyChanged;
    }
}
