using MediaManager;
using MOB_RadioApp.css;
using MOB_RadioApp.Services;
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
    public partial class MediaplayerControl : ContentView
    {
      
        public MediaplayerControl()
        {
            InitializeComponent();
            
            
            MediaplayerStyle.StyleClass = Backgrounds.GetBackground().Color; ;
            
            //AnimationService.AnimateBackground(PurpleView);
        }

       
    }
}