using System;
using System.ComponentModel;
using Android.Content;
using Android.Gms.Maps;
using GeoJsonDemo.Controls;
using GeoJsonDemo.Droid.CustomRenderers;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Maps.Android;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CustomMapView), typeof(MapCustomRenderer))]
namespace GeoJsonDemo.Droid.CustomRenderers
{
    public class MapCustomRenderer : MapRenderer
    {
        public MapCustomRenderer(Context context) : base(context)
        {
        }

        protected override void OnMapReady(GoogleMap map)
        {
            base.OnMapReady(map);
            System.Diagnostics.Debug.WriteLine($"--> OnMapReady <--");
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Map> e)
        {
            base.OnElementChanged(e);
            System.Diagnostics.Debug.WriteLine($"--> OnElementChanged <--");
            if (e.NewElement != null)
            {
                Control.GetMapAsync(this);
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            System.Diagnostics.Debug.WriteLine($"--> {e.PropertyName} <--");
        }
    }
}
