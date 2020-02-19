using System;
using GeoJSON.Net.Feature;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace GeoJsonDemo.Controls
{
    public class CustomMapView : Map
    {
        public static readonly BindableProperty FeaturesProperty = BindableProperty.Create(nameof(Features), typeof(FeatureCollection), typeof(CustomMapView));

        public FeatureCollection Features
        {
            get => (FeatureCollection)GetValue(FeaturesProperty);
            set => SetValue(FeaturesProperty, value);
        }
    }
}
