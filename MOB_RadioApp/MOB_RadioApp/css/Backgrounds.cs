using MOB_RadioApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Essentials;

namespace MOB_RadioApp.css
{
    public static class Backgrounds
    {
        public static List<Background> GetColors()
        {
            List<Background> backgrounds = new List<Background>()
            {
                new Background(){Key = 1, Name = "Default", Color = new List<string> {"blueView"}},
                new Background(){Key = 2, Name = "Rode bollen",  Color = new List<string> {"redView"}},
                new Background(){Key = 3, Name = "Kiezel", Color = new List<string> {"purpleView"}},
                new Background(){Key = 4, Name = "Groene bollen", Color = new List<string> {"greenView"}},
                new Background(){Key = 5, Name = "Schwarz", Color = new List<string> {"blackView"}},
                new Background(){Key = 6, Name = "Doe ké normoahl", Color = new List<string> {"blueNormalView"}},
                new Background(){Key = 7, Name = "private void", Color = new List<string> {"greenNormalView"}},
                new Background(){Key = 8, Name = ": )", Color = new List<string> {"pinkNormalView"}}
            };
            return backgrounds;
        }
        public static Background GetBackground()
        {
            return Backgrounds.GetColors().Where(x => x.Key == int.Parse(Preferences.Get(Pref.background, "1"))).FirstOrDefault() ;
        }
    }



    public class Background
    {
        public int Key { get; set; }
        public string Name { get; set; }
        public List<string> Color { get; set; }
    }
}