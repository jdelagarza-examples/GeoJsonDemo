using System;
using System.Runtime.CompilerServices;
using GeoJsonDemo.ViewModels;
using Xamarin.Forms;

namespace GeoJsonDemo.Pages
{
    public class BaseContentPage : ContentPage
    {
        public static readonly BindableProperty ViewModelProperty = BindableProperty.Create(nameof(ViewModel), typeof(ViewModel), typeof(BaseContentPage), default(ViewModel), BindingMode.OneWay, null, (bindable, oldValue, newValue) => { if (oldValue == newValue) return; ((BaseContentPage)bindable).OnViewModelPropertyChanged(); });

        public ViewModel ViewModel
        {
            get => (ViewModel)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        void OnViewModelPropertyChanged()
        {
            BindingContext = ViewModel;
            SetBinding(IsBusyProperty, new Binding("IsBusy", BindingMode.TwoWay));
            SetBinding(IsEnabledProperty, new Binding("IsEnabled", BindingMode.TwoWay));
            SetBinding(TitleProperty, new Binding("Title", BindingMode.TwoWay));
            SetBinding(IconImageSourceProperty, new Binding("Icon", BindingMode.TwoWay));
            SetBinding(BackgroundColorProperty, new Binding("BackgroundColor", BindingMode.TwoWay));
        }

        public bool IsBackButtonPressedEnabled;

        public BaseContentPage()
        {
            NavigationPage.SetBackButtonTitle(this, "");
            IsBackButtonPressedEnabled = true;
        }

        public BaseContentPage(ViewModel viewModel)
        {
            ViewModel = viewModel;
            NavigationPage.SetBackButtonTitle(this, "");
            IsBackButtonPressedEnabled = true;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            ViewModel?.OnAppearing(this);
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            ViewModel?.OnDisappearing();
        }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);
            ViewModel?.OnPropertyChanged(propertyName);
        }

        protected override void OnPropertyChanging([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanging(propertyName);
            ViewModel?.OnPropertyChanging(propertyName);
        }

        protected override bool OnBackButtonPressed()
        {
            if (!IsBackButtonPressedEnabled)
            {
                System.Diagnostics.Debug.WriteLine($"Debe cerrar la aplicación desde el menú de multitarea");
            }
            return !IsBackButtonPressedEnabled;
        }
    }
}