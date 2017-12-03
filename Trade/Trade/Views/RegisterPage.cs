using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Trade.ViewModels;
using Xamarin.Forms;

namespace Trade.Views
{
    public class RegisterPage : ContentPage
    {
        RegisterViewModel registerViewModel;
        public RegisterPage()
        {
            BindingContext = registerViewModel = new RegisterViewModel(this);

            NavigationPage.SetHasNavigationBar(this, true);
            Title = "Registration";
            var activityIndicator = new ActivityIndicator
            {
                Color = Color.Blue,
            };
            activityIndicator.SetBinding(IsVisibleProperty, "IsBusy");
            activityIndicator.SetBinding(ActivityIndicator.IsRunningProperty, "IsBusy");
            //  BackgroundColor = Color.Blue;
            BackgroundImage = "LoginScreen.png";
            var layout = new StackLayout
            {
                Padding = 20,
                VerticalOptions = LayoutOptions.CenterAndExpand,
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
                Source = FileImageSource.FromFile("Background.png")
            };

            layout.Children.Add(label);

            var username = new Entry { Placeholder = "UserName", TextColor = Color.Gray };
            username.SetBinding(Entry.TextProperty, RegisterViewModel.UsernamePropertyName);
            layout.Children.Add(username);

            var password = new Entry { Placeholder = "Password", IsPassword = true, TextColor = Color.Gray };
            password.SetBinding(Entry.TextProperty, RegisterViewModel.PasswordPropertyName);
            layout.Children.Add(password);

            var Confpassword = new Entry { Placeholder = "Confirm Password", IsPassword = true, TextColor = Color.Gray };
            Confpassword.SetBinding(Entry.TextProperty, RegisterViewModel.ComfirmPasswordPropertyName);
            layout.Children.Add(Confpassword);

            var relativelayout = new RelativeLayout();

            var button = new Button
            {
                Text = "Join Us",
                TextColor = Color.White,
                BackgroundColor =
                Color.FromHex("#8BC34A")
            };
            button.SetBinding(Button.CommandProperty, RegisterViewModel.RegisterCommandPropertyName);

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

        //protected override void OnAppearing()
        //{
        //    base.OnAppearing();
        //    Title = "Registretion";
        //}
    }
    
}