using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace WakeMeUpOnLan.Core.Models {
    public class BaseStorable : INotifyPropertyChanged {

        public BaseStorable() {
            Id = new Guid();
            DateCreated = DateTime.Now;
        }

        private Guid _id;
        public Guid Id {
            get => _id;
            set => SetProperty( ref _id, value );
        }

        public DateTime DateCreated {
            get;
            set;
        }

        #region INotifyPropertyChanged

        protected bool SetProperty<T>( ref T backingStore, T value,
            [CallerMemberName] string propertyName = "",
            Action onChanged = null ) {

            bool hasValueChanged = false;
            if( !EqualityComparer<T>.Default.Equals( backingStore, value ) ) {
                hasValueChanged = true;
                backingStore = value;
                onChanged?.Invoke();
                OnPropertyChanged( propertyName );
            }
            return hasValueChanged;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged( string propertyName ) {
            PropertyChanged?.Invoke( this, new PropertyChangedEventArgs( propertyName ) );
        }

        #endregion

    }
}
