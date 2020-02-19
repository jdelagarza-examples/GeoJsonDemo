using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace GeoJsonDemo.ViewModels
{
    public class ListPageViewModel : PageViewModel
    {
        bool _isPullToRefreshEnabled;
        public bool IsPullToRefreshEnabled
        {
            get => _isPullToRefreshEnabled;
            set => SetProperty(ref _isPullToRefreshEnabled, value, nameof(IsPullToRefreshEnabled));
        }

        bool _isRefreshing;
        public bool IsRefreshing
        {
            get => _isRefreshing;
            protected set => SetProperty(ref _isRefreshing, value, nameof(IsRefreshing));
        }

        Color _pullToRefreshBackgroundColor;
        public Color PullToRefreshBackgroundColor
        {
            get => _pullToRefreshBackgroundColor;
            set => SetProperty(ref _pullToRefreshBackgroundColor, value, nameof(PullToRefreshBackgroundColor));
        }

        public ICommand PullToRefreshCommand
        {
            get => new Command((args) =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    OnPullToRefresh(args);
                });
            });
        }

        public ICommand ItemTappedCommand
        {
            get => new Command((args) =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    OnItemTapped(args);
                });
            });
        }

        protected virtual void OnPullToRefresh(object args)
        {

        }

        protected virtual void OnItemTapped(object item)
        {

        }

        protected override void OnInitializeProperties()
        {
            base.OnInitializeProperties();
            _isPullToRefreshEnabled = false;
            _isRefreshing = false;
            _pullToRefreshBackgroundColor = Color.FromHex("#cccccc");
        }
    }
}
