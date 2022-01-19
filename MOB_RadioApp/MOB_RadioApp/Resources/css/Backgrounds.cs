using MOB_RadioApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Essentials;

namespace MOB_RadioApp.css
{
    public static class Backgrounds
    {   /// <summary>
    /// Get available backgrounds
    /// </summary>
    /// <returns></returns>
        public static List<Background> GetColors()
        {
            // Backgrounds are stored in style.css
            List<Background> backgrounds = new List<Background>()
            {
                new Background(){Key = 1, Name = "Default", Color = new List<string> {"blueView"}},
                new Background(){Key = 2, Name = "Rode bollen",  Color = new List<string> {"redView"}},
                new Background(){Key = 3, Name = "Blauwig", Color = new List<string> {"purpleView"}},
                new Background(){Key = 4, Name = "Groene bollen", Color = new List<string> {"greenView"}},
                new Background(){Key = 5, Name = "Donker", Color = new List<string> {"blackView"}},
                new Background(){Key = 6, Name = "Doe ké normoal", Color = new List<string> {"blueNormalView"}},
                new Background(){Key = 7, Name = "Groen", Color = new List<string> {"greenNormalView"}},
                new Background(){Key = 8, Name = "x )", Color = new List<string> {"pinkNormalView"}}
            };
            return backgrounds;
        }
        /// <summary>
        /// Get current background
        /// </summary>
        /// <returns></returns>
        public static Background GetBackground()
        {
            return Backgrounds.GetColors().Where(x => x.Key == int.Parse(Preferences.Get(ProjectSettings.background, "1"))).FirstOrDefault() ;
        }
    }



    public class Background
    {
        public int Key { get; set; }
        public string Name { get; set; }
        public List<string> Color { get; set; }
    }
}