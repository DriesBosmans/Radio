﻿using MOB_RadioApp.css;
using MOB_RadioApp.Models;
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
    public partial class FavouritesControl : ContentView
    {
        public FavouritesControl()
        {
            InitializeComponent();
            // Change backgrounds
            FavouritesStyle.StyleClass = Backgrounds.GetBackground().Color;
            MessagingCenter.Subscribe<MainViewModel>(this, ProjectSettings.background, (sender) =>
            {
                FavouritesStyle.StyleClass = Backgrounds.GetBackground().Color;
            });
        }
    }
}