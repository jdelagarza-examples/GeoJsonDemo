using System;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace GeoJsonDemo.ViewModels
{
    public class PageViewModel : BaseViewModel
    {
        bool _isBusy;
        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value, nameof(IsBusy));
        }

        bool _isEnabled;
        public bool IsEnabled
        {
            get => _isEnabled;
            set => SetProperty(ref _isEnabled, value, nameof(IsEnabled));
        }

        string _title;
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value, nameof(Title));
        }

        ImageSource _icon;
        public ImageSource Icon
        {
            get => _icon;
            set => SetProperty(ref _icon, value, nameof(Icon));
        }

        ImageSource _backgroundImage;
        public ImageSource BackgroundImage
        {
            get => _backgroundImage;
            set => SetProperty(ref _backgroundImage, value, nameof(BackgroundImage));
        }

        Color _backgroundColor;
        public Color BackgroundColor
        {
            get => _backgroundColor;
            set => SetProperty(ref _backgroundColor, value, nameof(BackgroundColor));
        }

        Page _page;
        public Page Page
        {
            get => _page;
            set => SetProperty(ref _page, value, nameof(Page));
        }

        public virtual void OnAppearing(Page page)
        {
            Page = page ?? Application.Current.MainPage;
        }

        public virtual void OnDisappearing()
        {

        }

        public virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {

        }

        public virtual void OnPropertyChanging([CallerMemberName] string propertyName = null)
        {

        }

        protected override void OnInitializeProperties()
        {
            base.OnInitializeProperties();
            _isBusy = false;
            _isEnabled = true;
            _title = "";
            _icon = ImageSource.FromFile("");
            _backgroundImage = ImageSource.FromFile("");
            _backgroundColor = Color.FromHex("#e0e0e0");
            _page = Application.Current.MainPage;
        }
    }
}