using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace kahua.ktree.viewmodel
{
    public abstract class NotifiableBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void onNotifyPropertyChanged([CallerMemberName]string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected void set<T>(ref T field, T value, [CallerMemberName] string propertyName = "")
        {
            if (EqualityComparer<T>.Default.Equals(field, value))
                return;

            field = value;
            onNotifyPropertyChanged(propertyName);
        }
    }

}
