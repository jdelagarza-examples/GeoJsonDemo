using System;
using System.Collections.Generic;
using GeoJsonDemo.ViewModels;
using Xamarin.Forms;

namespace GeoJsonDemo.Pages
{
    public partial class GeoJsonMapPage : BaseContentPage
    {
        public GeoJsonMapPage(GeoJsonMapViewModel viewModel) : base(viewModel)
        {
            InitializeComponent();
        }
    }
}
