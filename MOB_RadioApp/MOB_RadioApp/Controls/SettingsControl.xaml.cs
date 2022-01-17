using MOB_RadioApp.css;
using MOB_RadioApp.Models;
using MOB_RadioApp.Popups;
using MOB_RadioApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MOB_RadioApp.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsControl : ContentView
    {
        public SettingsControl()
        {
            InitializeComponent();
            SettingsStyle.StyleClass = Backgrounds.GetBackground().Color;
            MessagingCenter.Subscribe<MainViewModel>(this, "Background", (sender) =>
            {
                SettingsStyle.StyleClass = Backgrounds.GetBackground().Color;
            });
           
        }

    }
}