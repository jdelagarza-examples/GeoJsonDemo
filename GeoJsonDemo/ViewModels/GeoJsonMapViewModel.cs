using System;
using System.Linq;
using System.Windows.Input;
using GeoJSON.Net.Feature;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace GeoJsonDemo.ViewModels
{
    public class GeoJsonMapViewModel : ViewModel
    {
        FeatureCollection _geoJsonFeatures;
        public FeatureCollection GeoJsonFeatures
        {
            get => _geoJsonFeatures;
            set => SetProperty(ref _geoJsonFeatures, value);
        }

        Xamarin.Forms.Maps.Position _mapPosition;
        public Xamarin.Forms.Maps.Position MapPosition
        {
            get => _mapPosition;
            set => SetProperty(ref _mapPosition, value, nameof(MapPosition));
        }

        public ICommand FeatureSelectedCommand
        {
            get => new Command((object args) =>
            {
                if (args == null || !(args is Feature feature)) return;
                _ = 0;
            });
        }

        public ICommand MapFinishedCommand
        {
            get => new Command(() =>
            {
            });
        }

        async void ReadLocalGeojson(string resource)
        {
            await System.Threading.Tasks.Task.Delay(TimeSpan.FromSeconds(5));
            string geojson = await ApplicationManager.Current.ReadResource(resource);
            GeoJsonFeatures = JsonConvert.DeserializeObject<FeatureCollection>(geojson);
            if (resource.Contains("i60"))
            {
                MapPosition = new Xamarin.Forms.Maps.Position(31.68750373196066, -106.43608403104382);
            }
            else
            {
                MapPosition = new Xamarin.Forms.Maps.Position(32.36393678379417, - 116.91491352942533);
            }
        }

        public override void OnAppearing(Page page)
        {
            base.OnAppearing(page);
            string path = "GeoJsonDemo.Resources.";
            ReadLocalGeojson(path + "i60.geo.json");
            //ReadLocalGeojson(path + "q4v.geo.json");
        }

        protected override void OnInitializeProperties()
        {
            base.OnInitializeProperties();
            _mapPosition = new Xamarin.Forms.Maps.Position(ApplicationManager.Current.LocationRequest?.Location?.Latitude ?? 0.0, ApplicationManager.Current.LocationRequest?.Location?.Longitude ?? 0.0);
        }
    }
}
