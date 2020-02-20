using System;
using System.Windows.Input;
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

        public static readonly BindableProperty CurrentFeatureProperty = BindableProperty.Create(nameof(CurrentFeature), typeof(Feature), typeof(CustomMapView));
        public Feature CurrentFeature
        {
            get => (Feature)GetValue(CurrentFeatureProperty);
            set => SetValue(CurrentFeatureProperty, value);
        }

        public static readonly BindableProperty FeatureSelectedCommandProperty = BindableProperty.Create(nameof(FeatureSelectedCommand), typeof(ICommand), typeof(CustomMapView));
        public ICommand FeatureSelectedCommand
        {
            get => (ICommand)GetValue(FeatureSelectedCommandProperty);
            set => SetValue(FeatureSelectedCommandProperty, value);
        }

        public static readonly BindableProperty FeatureLineWidthProperty = BindableProperty.Create(nameof(FeatureLineWidth), typeof(float), typeof(CustomMapView), (float)2.0);
        public float FeatureLineWidth
        {
            get => (float)GetValue(FeatureLineWidthProperty);
            set => SetValue(FeatureLineWidthProperty, value);
        }

        public static readonly BindableProperty FeatureLineColorProperty = BindableProperty.Create(nameof(FeatureLineColor), typeof(Color), typeof(CustomMapView), Color.FromHex("#000000"));
        public Color FeatureLineColor
        {
            get => (Color)GetValue(FeatureLineColorProperty);
            set => SetValue(FeatureLineColorProperty, value);
        }

        public static readonly BindableProperty FeatureFillColorProperty = BindableProperty.Create(nameof(FeatureFillColor), typeof(Color), typeof(CustomMapView), Color.FromHex("#ffffff"));
        public Color FeatureFillColor
        {
            get => (Color)GetValue(FeatureFillColorProperty);
            set => SetValue(FeatureFillColorProperty, value);
        }

        public static readonly BindableProperty IsFeatureClickeableProperty = BindableProperty.Create(nameof(IsFeatureClickeable), typeof(bool), typeof(CustomMapView), false);
        public bool IsFeatureClickeable
        {
            get => (bool)GetValue(IsFeatureClickeableProperty);
            set => SetValue(IsFeatureClickeableProperty, value);
        }

        public static readonly BindableProperty IsPinPositionAddedProperty = BindableProperty.Create(nameof(IsPinPositionAdded), typeof(bool), typeof(CustomMapView), false, BindingMode.OneWay, null, (bindable, oldValue, newValue) => { if (oldValue == newValue) return; (bindable as CustomMapView).OnIsPinPositionAddedPropertyChanged(); });
        public bool IsPinPositionAdded
        {
            get => (bool)GetValue(IsPinPositionAddedProperty);
            set => SetValue(IsPinPositionAddedProperty, value);
        }
        void OnIsPinPositionAddedPropertyChanged()
        {
            if (!IsPinPositionAdded || MapPosition == default(Position)) return;
            if (Pins.Count > 0) Pins.Clear();

            Pin pin = new Pin();
            pin.Label = $"{MapPosition.Latitude}, {MapPosition.Longitude}";
            pin.Type = PinType.Place;
            pin.Position = MapPosition;
            Pins.Add(pin);
        }

        public static readonly BindableProperty MapFinishedCommandProperty = BindableProperty.Create(nameof(MapFinishedCommand), typeof(ICommand), typeof(CustomMapView));
        public ICommand MapFinishedCommand
        {
            get => (ICommand)GetValue(MapFinishedCommandProperty);
            set => SetValue(MapFinishedCommandProperty, value);
        }

        public static readonly BindableProperty MapPositionProperty = BindableProperty.Create(nameof(MapPosition), typeof(Position), typeof(CustomMapView), null, BindingMode.OneWay, null, (bindable, oldValue, newValue) => { if (oldValue == newValue) return; (bindable as CustomMapView).OnMapPositionPropertyChanged(); });
        public Position MapPosition
        {
            get => (Position)GetValue(MapPositionProperty);
            set => SetValue(MapPositionProperty, value);
        }
        void OnMapPositionPropertyChanged()
        {
            MoveToRegion(MapSpan.FromCenterAndRadius(MapPosition, Distance.FromKilometers(0.1)));
            OnIsPinPositionAddedPropertyChanged();
        }
    }
}
