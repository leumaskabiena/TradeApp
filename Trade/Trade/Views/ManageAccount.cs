using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace Trade.Views
{
    public class ManageAccount : ContentPage
    {
        public ManageAccount()
        {
            var login = new Button
            {
                Text = "login",
                IsVisible = false
            };

            var layout = new StackLayout { };
            Content = layout;
        }
    }
}