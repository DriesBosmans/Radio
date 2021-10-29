using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace MOB_RadioApp.ViewModels
{
    public abstract class BaseViewModel2 :  BaseViewModel
    {
        protected void SetValue<T>(ref T backingField,T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingField, value)) 
                return;
            backingField = value;
            OnPropertyChanged(propertyName);
        }
    }
}
