using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace WakeMeUpOnLan.Core.Models {
    public class WolTarget : BaseStorable {

        private string _targetMacAddress;
        private string _name;
        private ObservableCollection<ApiUser> _apiUsers;

        public WolTarget() : base() {
            ApiUsers = new ObservableCollection<ApiUser>();
        }

        public string TargetMacAddress {
            get => _targetMacAddress;
            set => SetProperty( ref _targetMacAddress, value );
        }

        public string Name {
            get => _name;
            set => SetProperty( ref _name, value );
        }

        public ObservableCollection<ApiUser> ApiUsers {
            get => _apiUsers;
            set => SetProperty( ref _apiUsers, value );
        }
    }
}
