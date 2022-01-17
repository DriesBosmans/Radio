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
                new Background(){Key = 1, Name = "Blauw", Color = new List<string> {"blueView"}},
                new Background(){Key = 2, Name = "Rood",  Color = new List<string> {"redView"}},
                new Background(){Key = 3, Name = "Paars", Color = new List<string> {"purpleView"}},
                new Background(){Key = 4, Name = "Groen", Color = new List<string> {"greenView"}},
                new Background(){Key = 5, Name = "Zwart", Color = new List<string> {"blackView"}},
                new Background(){Key = 6, Name = "Blauw 2", Color = new List<string> {"blueNormalView"}},
                new Background(){Key = 7, Name = "Groen 2", Color = new List<string> {"greenNormalView"}}
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