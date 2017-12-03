using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Trade.Helpers;
using Trade.Style;
using Trade.Views.Xaml;
using Xamarin.Forms;

namespace Trade.Views
{
    public class MenuTableView : TableView
    {

    }
    public class MenuPage : ContentPage
    {

        public ListView Menu { get; set; }
        TradeMainPage rootPage;
        TableView tableView;

        public MenuPage(TradeMainPage rootPage)
        {
            Title = "Trade";// The Title property must be set.
            this.rootPage = rootPage;
            var layout = new StackLayout
            {
                Spacing = 0,
                VerticalOptions = LayoutOptions.FillAndExpand,
                BackgroundColor = Color.FromHex("#FF9800"),
            };
            var UserName = new Label
            {
                Text = Settings.AuthUserName,
                BackgroundColor = Color.FromHex("#FF9800"),
                TextColor = Color.White,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                IsVisible = false
            };
            var Login = new Button
            {
                Text="Log In",
                TextColor = Color.White,
                IsVisible=true
            };
            var Logout = new Button
            {
                Text="Log Out",
                TextColor = Color.White,
                IsVisible = false
            };
           
            Login.Clicked += (sender, e) =>
            {
                //  Application.Current.MainPage Navigation.PushAsync(new LoginPage() { Title="Login"});
                Application.Current.MainPage.Navigation.PushAsync(new LoginPage());
            };
            Logout.Clicked += (sender, e) =>
            {
                Settings.AuthLoginToken = string.Empty;
                Navigation.PushAsync(new TradeMainPage());
                UserName.IsVisible = false;
                Login.IsVisible = true;
                Logout.IsVisible = false;
                rootPage.IsPresented = false;  // close the slide-out
            };
            var section = new TableSection();
            if (!Settings.AuthLoginToken.Equals(string.Empty))
            {
                UserName.IsVisible = true;
                Login.IsVisible = false;
                Logout.IsVisible = true;
                //number of notification
                int num = Settings.Notificaton;

                section = new TableSection()
                {
                    new MenuCell { Text = "Notification",Notify= num.ToString() , Host = this, ImageSrc = "index2.png" },
                    new MenuCell { Text = "Home", Host = this, ImageSrc = "home_black.png" },
                    new MenuCell { Text = "Trade", Host = this, ImageSrc = "create.png" },
                    new MenuCell { Text = "MyTrade", Host = this, ImageSrc = "index2.png" },                  
                    new MenuCell { Text = "About", Host = this, ImageSrc = "about_black.png" }
                };
            }

            else
            {
                section = new TableSection()
                {
                    new MenuCell { Text = "Home", Host = this, ImageSrc = "home_black.png" },
                    new MenuCell { Text = "About", Host = this, ImageSrc = "about_black.png" }
                };
            }

         


            var root = new TableRoot() { section };

            tableView = new MenuTableView()
            {
                Root = root,
                Intent = TableIntent.Data,
                //BackgroundColor = Color.FromHex("2C3E50"),
                BackgroundColor = Color.White,

            };

            var settingView = new SettingsUserView();

            //settingView.tapped += (object sender, TapViewEventHandler e) =>
            //{

            //    Navigation.PushAsync(new Profile());
            //    // var home = new NavigationPage(new Profile());
            //    // rootPage.Detail = home;
            //};

            layout.Children.Add(settingView);
            //layout.Children.Add(new BoxView()
            //{
            //    HeightRequest = 1,
            //    BackgroundColor = AppStyle.DarkLabelColor,
            //});
            layout.Children.Add(UserName);
            layout.Children.Add(tableView);
            layout.Children.Add(Login);
            layout.Children.Add(Logout);

            Content = layout;

            var tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped +=
                (sender, e) =>
                {
                    //NavigationPage profile = new NavigationPage(new Profile(settingView.profileViewModel.myProfile))
                    //{
                    //   // BarBackgroundColor = App.BrandColor,
                    //    BarTextColor = Color.White
                    //};
                    //rootPage.Detail = profile;
                    //rootPage.IsPresented = false;
                };
            settingView.GestureRecognizers.Add(tapGestureRecognizer);

        }

        Page home, About, Trade, myTrade, Notification;
        public void Selected(string item)
        {

            switch (item)
            {
                case "Home":
                    if (home == null)
                        home = new Home();

                    rootPage.Detail = home;
                    break;
                case "Trade":
                    Trade = new TradePage();
                    rootPage.Detail = Trade;
                    break;
                case "MyTrade":
                    myTrade = new MyTrade();
                    rootPage.Detail = myTrade;
                    break;
                case "Notification":
                    Notification = new Notification();
                    rootPage.Detail = Notification;
                    break;
                case "About":
                    About = new AboutPage()
                    {
                        // BarBackgroundColor = App.BrandColor,
                        ///// BarTextColor = Color.White
                    };
                    rootPage.Detail = About;
                    break;
            };
            rootPage.IsPresented = false;  // close the slide-out
        }
    }


}   