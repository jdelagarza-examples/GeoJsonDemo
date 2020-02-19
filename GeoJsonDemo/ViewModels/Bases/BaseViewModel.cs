using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace GeoJsonDemo.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public BaseViewModel()
        {
            OnInitializeProperties();
        }

        protected virtual void OnInitializeProperties()
        {

        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (Equals(value, null) ? !Equals(field, null) : !Equals(value, field))
            {
                field = value;
                PropertyChangedEventHandler handler = this.PropertyChanged;
                if (handler != null)
                {
                    handler(this, new PropertyChangedEventArgs(propertyName));
                }
                return true;
            }
            return false;
        }
    }
}