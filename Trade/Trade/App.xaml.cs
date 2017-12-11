using PushNotification.Plugin;
using System;
using Trade.Models;
using Trade.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Trade
{
    public partial class App : Application, ILoginManager
    {
        public static MasterDetailPage MD { get; set; }
        public static NavigationPage NV { get; set; }
        public App()
        {
            InitializeComponent();

            if (Device.RuntimePlatform == Device.iOS)
                MainPage = new TradeMainPage();
            else if (Device.RuntimePlatform == Device.Android)
                MainPage = new NavigationPage(new MainPage());
            else
                MainPage = new NavigationPage(new TradeMainPage() { Title="Home"});
        }
        protected override void OnStart()
        {
          //  base.OnStart();
            //CrossPushNotification.Current.Register();
        }
        #region ILoginManager implementation
        public void ShowRootPage()
        {
            new TradeMainPage();
        } 
        #endregion
    }
    
}