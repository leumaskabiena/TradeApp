using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Trade.Helpers;
using Xamarin.Forms;

namespace Trade.Views
{
    public class ManageAccount : ContentPage
    {
        public ManageAccount()
        {
            var login = new Button
            {
            };
            if (!Settings.AuthLoginToken.Equals(string.Empty))
            {
                login.Text = "Log out";
            }
            else
            {
                login.Text = "Log in";
            }
            var layout = new StackLayout { };
            Content = layout;
        }
    }
}