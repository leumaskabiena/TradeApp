using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Trade.Style;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Trade.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public class MainListPage : ContentPage
    {
        public MainListPage()
        {

            NavigationPage.SetHasNavigationBar(this, true);
            NavigationPage.SetHasBackButton(this, false);
            List<string> lst = new List<string>();

            var customCell = new DataTemplate(typeof(CustomListview));
            var listView = new ListView
            {
                // ItemsSource = people,
                ItemTemplate = customCell
            };
            var layout = new StackLayout
            {

            };
            layout.Children.Add(listView);
            Content = layout;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            this.Title = "Trade";
        }
    }
}