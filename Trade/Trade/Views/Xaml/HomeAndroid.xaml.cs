using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Trade.Views.Xaml
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomeAndroid : ContentPage
    {
        public HomeAndroid()
        {
            InitializeComponent();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            this.Title = "Home";
            
        }
    }
}