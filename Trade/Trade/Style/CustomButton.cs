using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Trade.Helpers;
using Trade.Views;
using Xamarin.Forms;

namespace Trade.Style
{
    public class CustomButton : ToolbarItem
    {
       
        public CustomButton()
        {
            this.Visibility();
            if(!Settings.AuthLoginToken.Equals(string.Empty))
            {
                this.Text = "Log out";
            }
            else
            {
                this.Text = "Log in";
            }
            this.Clicked += CustomButton_Clicked;   
        }

        private void CustomButton_Clicked(object sender, EventArgs e)
        {
            if (Settings.AuthLoginToken.Equals(string.Empty))
            {
                Application.Current.MainPage.Navigation.PushAsync(new LoginPage());
            }
            else
            {
                this.Text = "Log in";
                Settings.AuthLoginToken = string.Empty;
            }
        }

        private async void Visibility()
        {
            await Task.Delay(100);
            OnIsVisibleChanged(this, false, isVisible);
        }
        public ContentPage Parent
        {
            set;
            get;
        }
        
        public bool isVisible
        {
            get { return (bool)GetValue(IsVisibleProperty); }
            set { SetValue(IsVisibleProperty, value); }
        }
        public static BindableProperty IsVisibleProperty =
        BindableProperty.Create<CustomButton, bool>(x => x.isVisible, false, propertyChanged: OnIsVisibleChanged);
        private static void OnIsVisibleChanged(BindableObject bindable,bool oldvalue,bool newvalue)
        {
            var item = bindable as CustomButton;
            if (item.Parent == null)
                return;

            var items = item.Parent.ToolbarItems;

            if(newvalue && items.Contains(item))
            {
                items.Add(item);
            }
            else if(!newvalue &&items.Contains(item))
            {
                items.Remove(item);
            }
        }
       
    }
}