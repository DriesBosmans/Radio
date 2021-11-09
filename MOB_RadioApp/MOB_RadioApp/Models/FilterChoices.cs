using MOB_RadioApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace MOB_RadioApp.Models
{
    public class FilterChoices : BaseViewModel2
    {
        private string _filterGenre;
        private string _filterLanguage;
        public string FilterGenre
        {
            get {  return _filterGenre; }
            set { SetValue(ref _filterGenre, value); }
        }
        
        public string FilterLanguage
        {
            get {  return _filterLanguage; }
            set {  SetValue(ref _filterLanguage, value);}
        }

        public FilterChoices(string genre, string language)
        {
            FilterGenre = genre;
            FilterLanguage = language;
        }
    }
}
