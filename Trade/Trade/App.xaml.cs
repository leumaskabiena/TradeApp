using PushNotification.Plugin;
using System;
using Trade.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Trade
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            if (Device.RuntimePlatform == Device.iOS)
                MainPage = new TradeMainPage();
            else
                MainPage = new NavigationPage(new TradeMainPage() {Title="Home" });
        }
        protected override void OnStart()
        {
          //  base.OnStart();
            //CrossPushNotification.Current.Register();
        }
    }
}