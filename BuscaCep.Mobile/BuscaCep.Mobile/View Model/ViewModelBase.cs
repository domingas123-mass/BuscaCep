using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace BuscaCep.Mobile.View_Model
{
    class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        ///  protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        // {
        // if (PropertyChanged is null)
        // return;
        //PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
      

        // }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
       => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        private bool _isBusy = false;
        public bool IsVsy
        {
            get => _isBusy;
            set
            {
                _isBusy = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(isNotBusy));

            }
        }

        public bool isNotBusy
        {
            get => !_isBusy;
        }
    }
}
