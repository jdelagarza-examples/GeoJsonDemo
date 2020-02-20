using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using Android.Content;
using GeoJsonDemo.Controls;
using GeoJsonDemo.Droid.CustomRenderers;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Maps.Android;
using Xamarin.Forms.Platform.Android;
using static Android.Gms.Maps.GoogleMap;

[assembly: ExportRenderer(typeof(CustomMapView), typeof(MapCustomRenderer))]
namespace GeoJsonDemo.Droid.CustomRenderers
{
    public class MapCustomRenderer : MapRenderer, IOnMapLoadedCallback
    {
        IDictionary<Android.Gms.Maps.Model.Polygon, GeoJSON.Net.Feature.Feature> _features;

        public MapCustomRenderer(Context context) : base(context)
        {
            _features = new Dictionary<Android.Gms.Maps.Model.Polygon, GeoJSON.Net.Feature.Feature>();
        }

        public void OnMapLoaded()
        {
            if (Element != null && Element is CustomMapView) (Element as CustomMapView).MapFinishedCommand?.Execute(null);
        }

        void PolygonClick(object sender, PolygonClickEventArgs e)
        {
            if (e == null) return;
            if (Element == null || !(Element is CustomMapView customMap)) return;
            try
            {
                KeyValuePair<Android.Gms.Maps.Model.Polygon, GeoJSON.Net.Feature.Feature> feature = _features.FirstOrDefault(x => x.Key.Id.Equals(e.Polygon.Id));
                customMap.FeatureSelectedCommand?.Execute(feature.Value);
            }
            catch (Exception)
            {
            }
        }

        void DrawGeoJson(GeoJSON.Net.Feature.FeatureCollection features)
        {
            if (Element == null || NativeMap == null) return;
            NativeMap.Clear();

            if (!(Element is CustomMapView customMap)) return;            
            if (customMap.Features == null) return;

            foreach (GeoJSON.Net.Feature.Feature feature in customMap.Features.Features)
            {
                switch (feature.Geometry.Type)
                {
                    case GeoJSON.Net.GeoJSONObjectType.LineString:
                        ConfigureLineString(feature, customMap);
                        break;
                    case GeoJSON.Net.GeoJSONObjectType.MultiLineString:
                        ConfigureMultiLineString(feature, customMap);
                        break;
                    case GeoJSON.Net.GeoJSONObjectType.Polygon:
                        ConfigurePolygon(feature, customMap);
                        break;
                    case GeoJSON.Net.GeoJSONObjectType.MultiPolygon:
                        ConfigureMultiPolygon(feature, customMap);
                        break;
                }
            }
        }

        void ConfigureLineString(GeoJSON.Net.Feature.Feature feature, CustomMapView customMap)
        {
            if (feature == null || customMap == null) return;
            if (!(feature.Geometry is GeoJSON.Net.Geometry.LineString)) return;

            Android.Gms.Maps.Model.PolylineOptions polylineOptions = new Android.Gms.Maps.Model.PolylineOptions();
            polylineOptions.InvokeWidth(customMap.FeatureLineWidth);
            polylineOptions.Clickable(false);
            if (feature.Properties.ContainsKey("COLOR") && feature.Properties["COLOR"] is string && IsHexColor(feature.Properties["COLOR"] as string)) polylineOptions.InvokeColor(Color.FromHex(feature.Properties["COLOR"] as string).ToAndroid());
            foreach (GeoJSON.Net.Geometry.IPosition coordinate in (feature.Geometry as GeoJSON.Net.Geometry.LineString).Coordinates)
            {
                polylineOptions.Add(new Android.Gms.Maps.Model.LatLng(coordinate.Latitude, coordinate.Longitude));
            }
            NativeMap.AddPolyline(polylineOptions);
        }

        void ConfigureMultiLineString(GeoJSON.Net.Feature.Feature feature, CustomMapView customMap)
        {
            if (feature == null || customMap == null) return;
            if (!(feature.Geometry is GeoJSON.Net.Geometry.MultiLineString)) return;

            Android.Gms.Maps.Model.PolylineOptions polylineOptions = new Android.Gms.Maps.Model.PolylineOptions();
            polylineOptions.InvokeWidth(customMap.FeatureLineWidth);
            polylineOptions.Clickable(false);
            if (feature.Properties.ContainsKey("COLOR") && feature.Properties["COLOR"] is string && IsHexColor(feature.Properties["COLOR"] as string)) polylineOptions.InvokeColor(Color.FromHex(feature.Properties["COLOR"] as string).ToAndroid());
            foreach (GeoJSON.Net.Geometry.LineString lineString in (feature.Geometry as GeoJSON.Net.Geometry.MultiLineString).Coordinates)
            {
                foreach (GeoJSON.Net.Geometry.IPosition coordinate in lineString.Coordinates)
                {
                    polylineOptions.Add(new Android.Gms.Maps.Model.LatLng(coordinate.Latitude, coordinate.Longitude));
                }
            }
            NativeMap.AddPolyline(polylineOptions);
        }

        void ConfigurePolygon(GeoJSON.Net.Feature.Feature feature, CustomMapView customMap)
        {
            if (feature == null || customMap == null) return;
            if (!(feature.Geometry is GeoJSON.Net.Geometry.Polygon)) return;

            Android.Gms.Maps.Model.PolygonOptions polygonOptions = new Android.Gms.Maps.Model.PolygonOptions();
            polygonOptions.InvokeStrokeWidth(customMap.FeatureLineWidth);
            polygonOptions.Clickable(customMap.IsFeatureClickeable);
            if (feature.Properties.ContainsKey("COLOR") && feature.Properties["COLOR"] is string && IsHexColor(feature.Properties["COLOR"] as string)) polygonOptions.InvokeFillColor(Color.FromHex(feature.Properties["COLOR"] as string).ToAndroid());
            foreach (GeoJSON.Net.Geometry.LineString lineString in (feature.Geometry as GeoJSON.Net.Geometry.Polygon).Coordinates)
            {
                foreach (GeoJSON.Net.Geometry.IPosition coordinate in lineString.Coordinates)
                {
                    polygonOptions.Add(new Android.Gms.Maps.Model.LatLng(coordinate.Latitude, coordinate.Longitude));
                }
            }
            Android.Gms.Maps.Model.Polygon polygon = NativeMap.AddPolygon(polygonOptions);
            _features.Add(polygon, feature);
        }

        void ConfigureMultiPolygon(GeoJSON.Net.Feature.Feature feature, CustomMapView customMap)
        {
            if (feature == null || customMap == null) return;
            if (!(feature.Geometry is GeoJSON.Net.Geometry.MultiPolygon)) return;

            Android.Gms.Maps.Model.PolygonOptions polygonOptions = new Android.Gms.Maps.Model.PolygonOptions();
            polygonOptions.InvokeStrokeWidth(customMap.FeatureLineWidth);
            polygonOptions.Clickable(customMap.IsFeatureClickeable);
            if (feature.Properties.ContainsKey("COLOR") && feature.Properties["COLOR"] is string && IsHexColor(feature.Properties["COLOR"] as string)) polygonOptions.InvokeFillColor(Color.FromHex(feature.Properties["COLOR"] as string).ToAndroid());
            foreach (GeoJSON.Net.Geometry.Polygon item in (feature.Geometry as GeoJSON.Net.Geometry.MultiPolygon).Coordinates)
            {
                foreach (GeoJSON.Net.Geometry.LineString lineString in item.Coordinates)
                {
                    foreach (GeoJSON.Net.Geometry.IPosition coordinate in lineString.Coordinates)
                    {
                        polygonOptions.Add(new Android.Gms.Maps.Model.LatLng(coordinate.Latitude, coordinate.Longitude));
                    }
                }
            }
            Android.Gms.Maps.Model.Polygon polygon = NativeMap.AddPolygon(polygonOptions);
            _features.Add(polygon, feature);
        }

        bool IsHexColor(string color)
        {
            if (string.IsNullOrEmpty(color) || string.IsNullOrWhiteSpace(color)) return false;
            return Regex.Match(color, @"^#([A-Fa-f0-9]{6}|[A-Fa-f0-9]{3})$").Success;
        }

        protected override void OnMapReady(Android.Gms.Maps.GoogleMap map)
        {
            base.OnMapReady(map);
            map.SetOnMapLoadedCallback(this);
            map.PolygonClick += PolygonClick;
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Map> e)
        {
            base.OnElementChanged(e);
            if (e.NewElement != null)
            {
                Control.GetMapAsync(this);
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (e.PropertyName == CustomMapView.FeaturesProperty.PropertyName)
            {
                DrawGeoJson((Element as CustomMapView).Features);
            }
        }
    }
}
