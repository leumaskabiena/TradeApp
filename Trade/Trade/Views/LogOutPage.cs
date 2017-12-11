﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Trade.ViewModels;
using Xamarin.Forms;

namespace Trade.Views
{
    public class LogOutPage : ContentPage
    {
        LoginViewModel loginViewModel;
        public LogOutPage()
        {
            //this.Title = "Login Page";
            //NavigationPage.SetHasNavigationBar(this, true);
            BindingContext = loginViewModel = new LoginViewModel(this);

            var activityIndicator = new ActivityIndicator
            {
                Color = Color.Blue,
            };
            activityIndicator.SetBinding(IsVisibleProperty, "IsBusy");
            activityIndicator.SetBinding(ActivityIndicator.IsRunningProperty, "IsBusy");
            //  BackgroundColor = Color.Blue;
            BackgroundImage = "backgroundLogin.png";
            var layout = new StackLayout
            {
                BackgroundColor = Color.DarkOrange,
                Padding = 20,
                VerticalOptions = LayoutOptions.CenterAndExpand
            };
            layout.Children.Add(activityIndicator);
            var label = new Label
            {
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                TextColor = Color.White,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                XAlign = TextAlignment.Center,
                YAlign = TextAlignment.Center,

            };

            var backgroundImage = new Image()
            {
                Aspect = Aspect.Fill,
                Source = FileImageSource.FromFile("backgroundLogin.png")
            };

            layout.Children.Add(label);

           

            

            var relativelayout = new RelativeLayout();

            var button = new Button
            {
                Text = "Log Out",
                TextColor = Color.White,
                BackgroundColor = Color.LawnGreen
            };
            button.SetBinding(Button.CommandProperty, LoginViewModel.LogoutCommandPropertyName);
            layout.Children.Add(button);

           

            relativelayout.Children.Add(backgroundImage,
                Constraint.Constant(0),
                Constraint.Constant(0),
                Constraint.RelativeToParent((parent) => { return parent.Width; }),
                Constraint.RelativeToParent((parent) => { return parent.Height; }));

            relativelayout.Children.Add(layout,
                Constraint.Constant(0),
                Constraint.Constant(0),
                Constraint.RelativeToParent((parent) => { return parent.Width; }),
                Constraint.RelativeToParent((parent) => { return parent.Height; }));


            ////button.Clicked += (sender, e) => {
            ////    if (String.IsNullOrEmpty(username.Text) || String.IsNullOrEmpty(password.Text))
            ////    {
            ////        DisplayAlert("Validation Error", "Username and Password are required", "Re-try");
            ////    } else {
            ////        // REMEMBER LOGIN STATUS!
            ////        App.Current.Properties["IsLoggedIn"] = true;


            ////        //ilm.ShowRootPage();

            ////    }
            ////};

            Content = new ScrollView { Content = relativelayout };
        }
    }
}