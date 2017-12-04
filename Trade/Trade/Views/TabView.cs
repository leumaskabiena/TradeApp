using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trade.Views.Xaml;
using Xamarin.Forms;

namespace Trade.Views
{
    public class TabView: TabbedPage
    {
        public TabView()
        {
            Page NotifyInPage, NotifyOutPage = null;
            switch (Device.RuntimePlatform)
            {
                case Device.iOS:
                    NotifyInPage = new NavigationPage(new NotifyIn())
                    {
                        Title = "Demand"
                    };

                    NotifyOutPage = new NavigationPage(new NotifyOut())
                    {
                        Title = "Reply"
                    };
                    //itemsPage.Icon = "tab_feed.png";
                    //aboutPage.Icon = "tab_about.png";
                    break;
                default:
                    NotifyInPage = new NotifyIn()
                    {
                        Title = "Demand"
                    };

                    NotifyOutPage = new NotifyOut()
                    {
                        Title = "Reply"
                    };
                    break;
            }

            Children.Add(NotifyInPage);
            Children.Add(NotifyOutPage);

            Title = Children[0].Title;
        }
        protected override void OnCurrentPageChanged()
        {
            base.OnCurrentPageChanged();
            Title = CurrentPage?.Title ?? string.Empty;
        }
    }
}
