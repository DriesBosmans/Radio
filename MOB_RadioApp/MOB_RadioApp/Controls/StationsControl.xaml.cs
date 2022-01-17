using MOB_RadioApp.css;
using MOB_RadioApp.Services;
using MOB_RadioApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MOB_RadioApp.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainView : ContentView
    {
        public MainView()
        {
            InitializeComponent();
            StationsStyle.StyleClass = Backgrounds.GetBackground().Color;
            MessagingCenter.Subscribe<MainViewModel>(this, "Background", (sender) =>
            {
                StationsStyle.StyleClass = Backgrounds.GetBackground().Color;
            });
            // Animating the background slows the application too much down
            //AnimationService.AnimateBackground(BlueView);
        }
    }
}