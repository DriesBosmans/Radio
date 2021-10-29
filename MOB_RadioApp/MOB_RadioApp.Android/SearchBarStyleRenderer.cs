using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MOB_RadioApp.Droid;
using MOB_RadioApp.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(SearchBarStyle), typeof(SearchBarStyleRenderer))]

namespace MOB_RadioApp.Droid
{
    public class SearchBarStyleRenderer : SearchBarRenderer
    {
        public SearchBarStyleRenderer(Context context) : base(context)
        {

        }
        protected override void OnElementChanged(ElementChangedEventArgs<SearchBar> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                //Remove underline
                var plateId = Resources.GetIdentifier("android:id/search_plate", null, null);
                var plate = Control.FindViewById(plateId);
                plate.SetBackgroundColor(Android.Graphics.Color.Transparent);

                ////Change icon
                //var searchView = Control;
                //searchView.Iconified = true;
                //searchView.SetIconifiedByDefault(false);
                //// (Resource.Id.search_mag_icon); is wrong / Xammie bug
                //int searchIconId = Context.Resources.GetIdentifier("android:id/search_mag_icon", null, null);
                //var icon = searchView.FindViewById(searchIconId);
                //(icon as ImageView).SetImageResource(searchIconId);
           
            }
        }
    }
}