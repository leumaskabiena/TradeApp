using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Trade.Helpers;
using Xamarin.Forms;

namespace Trade.Views
{
    public class Account : TabbedPage
    {
       
        public Account()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            Page about, account = null;
            about = new NavigationPage(new AboutPage())
            {
                Title = "About"
            };
            if (Settings.AuthLoginToken.Equals(string.Empty))
            {
                account = new NavigationPage(new LoginPage())
                {

                    Title = "Log In"
                }; 
            }
            else
            {
                account = new NavigationPage(new LoginPage())
                {

                    Title = "Log Out"
                };
            }
            Children.Add(about);
            Children.Add(account);

           
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            NavigationPage.SetHasNavigationBar(this, false);
        }
        //protected override void OnCurrentPageChanged()
        //{
        //    base.OnCurrentPageChanged();
        //    Title = CurrentPage?.Title ?? string.Empty;
        //    NavigationPage.SetHasNavigationBar(this, false);
        //}
    }
}