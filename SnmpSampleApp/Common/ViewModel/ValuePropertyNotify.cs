using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Common.ViewModel
{
    public class ValuePropertyNotify<T> : PropertyNotify
    {
        static protected PropertyChangedEventArgs ValueChangedEventArgs = new PropertyChangedEventArgs("Value");
        protected T _value;
        public ValuePropertyNotify(T iniVal)
        {
            _value = iniVal;
        }

        public virtual T Value
        {
            get { return this._value; }
            set { RaiseIfSet(ref _value, value); }
        }
    }
}
