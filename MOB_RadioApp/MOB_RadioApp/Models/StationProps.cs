using MOB_RadioApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace MOB_RadioApp.Models
{
    public abstract class StationProps : BaseViewModel2
    {
        public bool IsFavorite { get; set; } = false;
        public int Order { get; set; } = 4;
        private bool _isSelected = false;
        public bool IsSelected 
        { 
            get { return _isSelected; }
            set { SetValue(ref _isSelected, value);
            OnPropertyChanged(nameof(IsSelected));
                OnPropertyChanged(nameof(Opacity));
                OnPropertyChanged(nameof(TextColor));
              
            }
        } 
        public string Opacity { get => IsSelected ? "0.9" : "0.3";} 
        public string TextColor { get => IsSelected ? "white" : "#E0E0E0"; }
        private string _playUrl;

        public string PlayUrl
        {
            get { return _playUrl; }
            set { SetValue(ref _playUrl, value); }
        }

    }
}
