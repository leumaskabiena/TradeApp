using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Trade.Helpers;
using Trade.Views.Xaml;
using Xamarin.Forms;

namespace Trade.Views
{
    public class MainPage : TabbedPage
    {
        public MainPage()
        {
            Page home, About, Trade, myTrade, Notification=null;
            if (Settings.AuthLoginToken.Equals(string.Empty))
            {
                home = new HomeAndroid() { Title = "Home" };
                About = new Account() { Title = "Account" };

                Children.Add(home);
                Children.Add(About);

                Title = Children[0].Title;
            }
            else
            {
                home = new HomeAndroid() { Title = "Home" };
                Trade = new TradePage() { Title = "Trade" };
                myTrade = new MyTrade() { Title = "My Trade" };
                Notification = new TabView() { Title = "Notificatin" };
                About = new Account() { Title = "Account" };

                Children.Add(home);
                Children.Add(Trade);
                Children.Add(myTrade);
                Children.Add(Notification);
                Children.Add(About);

                Title = Children[0].Title;
            }
        }
        protected override void OnCurrentPageChanged()
        {
            base.OnCurrentPageChanged();
            Title = CurrentPage?.Title ?? string.Empty;
        }
    }
}