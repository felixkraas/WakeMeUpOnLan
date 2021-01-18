using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace WakeMeUpOnLan.Core.Models {
    public class ApiUser : BaseStorable {
        private string _name;
        private string _apiKey;
        private ObservableCollection<WolTarget> _allowedTargets;

        public ApiUser() : base() {
            AllowedTargets = new ObservableCollection<WolTarget>();
        }

        public string Name {
            get => _name;
            set => SetProperty( ref _name, value );
        }

        public string ApiKey {
            get => _apiKey;
            set => SetProperty( ref _apiKey, value );
        }

        public ObservableCollection<WolTarget> AllowedTargets {
            get => _allowedTargets;
            set => SetProperty( ref _allowedTargets, value );
        }
    }
}
