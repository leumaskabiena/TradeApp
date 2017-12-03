using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Trade.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Trade.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public class TradePage : ContentPage
    {
        TradeViewModel tradeViewModel;
        Image img;
        private MediaFile mediaFile;
        public TradePage()
        {
            BindingContext = tradeViewModel = new TradeViewModel();
            
            var activityIndicator = new ActivityIndicator
            {
                Color = Color.FromHex("#8BC34A"),
            };

            activityIndicator.SetBinding(IsVisibleProperty, "IsBusy");
            activityIndicator.SetBinding(ActivityIndicator.IsRunningProperty, "IsBusy");

            var Mainlayout = new StackLayout
            {
                Padding = 20,
                BackgroundColor=Color.Blue
            };

            var name = new Entry { Placeholder = "Item Name", TextColor = Color.Gray };
            name.SetBinding(Entry.TextProperty, TradeViewModel.NamePropertyName);
            Mainlayout.Children.Add(name);

            var price = new Entry { Placeholder = "Item Price", TextColor = Color.Gray, Keyboard = Keyboard.Numeric };
            price.SetBinding(Entry.TextProperty, TradeViewModel.PricePropertyName);
            Mainlayout.Children.Add(price);

            var label = new Label
            {
                Text = "Enter Description below"
            };
            Mainlayout.Children.Add(label);
            var description = new Editor { Text = "Enter Item Description", TextColor = Color.Gray };
            description.SetBinding(Editor.TextProperty, TradeViewModel.DescriptionPropertyName);
            Mainlayout.Children.Add(description);

            var ImageLayout = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.Center,
                Padding = 10
            };

            img = new Image {
                
                WidthRequest=300,
                HeightRequest=200,
                IsVisible=false,
                HorizontalOptions=LayoutOptions.Center
            };
           // var source = tradeViewModel.ImgSrc;
           
            ImageLayout.Children.Add(img);
            Mainlayout.Children.Add(ImageLayout);

            var Sublayout = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions=LayoutOptions.Center,
                BackgroundColor = Color.Yellow
            };

            var PickPhoto = new Button
            {
                Text = "Pick Photo",
                TextColor = Color.White,
                BackgroundColor =
                Color.FromHex("#8BC34A")
            };
            PickPhoto.Clicked += PickPhoto_Clicked;
            
            //PickPhoto.SetBinding(Button.CommandProperty, TradeViewModel.PickPhotoCommandPropertyName);
            Sublayout.Children.Add(PickPhoto);

            var TakePhoto = new Button
            {
                Text = "Take Photo",
                TextColor = Color.White,
                BackgroundColor =
               Color.FromHex("#8BC34A")
            };
            TakePhoto.Clicked += TakePhoto_Clicked;
            //TakePhoto.SetBinding(Button.CommandProperty, TradeViewModel.TakePhotoCommandPropertyName);
            Sublayout.Children.Add(TakePhoto);

            Mainlayout.Children.Add(Sublayout);

            var CreateTrade = new Button
            {
                Text = "Create Trade",
                TextColor = Color.White,
                BackgroundColor =
               Color.FromHex("#8BC34A")
            };
            CreateTrade.SetBinding(Button.CommandProperty, TradeViewModel.CreateTradeCommandPropertyName);
            Mainlayout.Children.Add(CreateTrade);

            Content = Mainlayout;
        }

        private async void TakePhoto_Clicked(object sender, EventArgs e)
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await Application.Current.MainPage.DisplayAlert("No camera found", ":( NO Camera awailable", "OK");
               
            }
            try
            {
                mediaFile = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
                {
                    Name = tradeViewModel.Name + ".png",
                    SaveToAlbum = true,
                    Directory = "Trade"
                });
            }
            catch (Exception)
            {
                await Application.Current.MainPage.DisplayAlert("No Picture", ":( No Picture Available", "OK");
               
            }
            img.Source = ImageSource.FromStream(() => {
                
                return mediaFile.GetStream();
            });
            img.IsVisible = true;
            tradeViewModel.ImgSrc = mediaFile.GetStream();
        }

        private async void PickPhoto_Clicked(object sender, EventArgs e)
        {
            await CrossMedia.Current.Initialize();
            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                await Application.Current.MainPage.DisplayAlert("No Picture", ":( No Picture Available", "OK");
                return;
            }

            try
            {
                mediaFile = await CrossMedia.Current.PickPhotoAsync();
            }
            catch (Exception)
            {
                await Application.Current.MainPage.DisplayAlert("No Picture", ":( No Picture Available", "OK");
                return;
            }
            if (mediaFile == null)
            {
                await Application.Current.MainPage.DisplayAlert("Picture Error", ":( No Picture Available", "OK");
            }
            img.Source = ImageSource.FromStream(() => {
               
                return mediaFile.GetStream(); 
            });
            img.IsVisible = true;
            tradeViewModel.ImgSrc= mediaFile.GetStream();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            Title = "Trade";
        }
    }
}