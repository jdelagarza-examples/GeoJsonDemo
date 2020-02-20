using System;
using GeoJsonDemo.Pages;
using GeoJsonDemo.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GeoJsonDemo
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            //_ = ApplicationManager.Current.GetLocationAsync();
            ApplicationManager.Current.UpdateLocation(TimeSpan.FromSeconds(10));

            GeoJsonMapViewModel viewModel = new GeoJsonMapViewModel();
            GeoJsonMapPage page = new GeoJsonMapPage(viewModel);

            NavigationPage navigationPage = new NavigationPage(page);
            MainPage = navigationPage;
        }

        protected override void OnStart()
        {
            base.OnStart();
        }

        protected override void OnSleep()
        {
            base.OnSleep();
        }

        protected override void OnResume()
        {
            base.OnResume();
        }
    }
}
