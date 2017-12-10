using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Trade.Helpers;
using Trade.Models.TableViewModel;
using Trade.ViewModels;
using Xamarin.Forms;

namespace Trade.Views
{
        public class Bet : ContentPage
        {
            BetViewModel betViewModel;
            public Bet(HomeItem data)
            {
                BindingContext = betViewModel = new BetViewModel();
                NavigationPage.SetHasNavigationBar(this, true);

                Title = "Bet for " + data.title;
                BackgroundColor = Color.Bisque;

                var layout = new StackLayout
                {
                    Padding = 10,
                };
                var sublayout = new StackLayout
                {
                    Padding = 10,
                };
                var ItemName = new Label
                {
                    Text = " Item Name : " + data.title,
                    TextColor = Color.White,
                    BackgroundColor = Color.Blue,
                    FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label))
                };
                // name.SetBinding(Label.TextProperty, BetViewModel.NamePropertyName);
                sublayout.Children.Add(ItemName);

                var ItemRef = new Label
                {
                    Text = "Ref : " + data.id,
                    TextColor = Color.White,
                    BackgroundColor = Color.Blue,
                    FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label))
                };

                var Ref = new Entry { Text = data.id };
                betViewModel.ItemRef = data.id;
                sublayout.Children.Add(ItemRef);

                var SellerName = new Label
                {
                    Text = " Seller Name : " + data.Name,
                    TextColor = Color.White,
                    BackgroundColor = Color.Blue,
                    FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label))
                };
                // name.SetBinding(Label.TextProperty, BetViewModel.NamePropertyName);
                sublayout.Children.Add(SellerName);

                var currentPrice = new Label
                {
                    Text = "Current Price : " + data.price,
                    TextColor = Color.White,
                    FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                    BackgroundColor = Color.Blue
                };
                // currentPrice.SetBinding(Entry.TextProperty, data.description);
                sublayout.Children.Add(currentPrice);

                var Description = new Label
                {
                    Text = "Description : " + data.description,
                    TextColor = Color.White,
                    FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                    BackgroundColor = Color.Blue
                };
                // currentPrice.SetBinding(Entry.TextProperty, data.description);
                sublayout.Children.Add(Description);

                layout.Children.Add(sublayout);

                var newPrice = new Entry { Placeholder = "Enter Price", TextColor = Color.Gray, Text = "", Keyboard = Keyboard.Numeric };
                newPrice.SetBinding(Entry.TextProperty, BetViewModel.PricePropertyName);
                NumericValidationBehavior.SetAttachBehavior(newPrice, true);
                layout.Children.Add(newPrice);

                var BetButton = new Button
                {
                    Text = "Bet",
                    TextColor = Color.White,
                    BackgroundColor =
                    Color.LawnGreen
                };
                BetButton.SetBinding(Button.CommandProperty, BetViewModel.BetCommandPropertyName);
                layout.Children.Add(BetButton);

                Content = layout;
            }

        }
}