using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Trade.Models.TableViewModel;
using Trade.ViewModels;
using Xamarin.Forms;

namespace Trade.Views
{
    public class ItemEdit : ContentPage
    {
        EditViewModel editViewModel;
        Image img;
        private MediaFile mediaFile;
        public ItemEdit(TradeItem tradeItem)
        {
            BindingContext = editViewModel = new EditViewModel(tradeItem);
            NavigationPage.SetHasNavigationBar(this, true);

            Title = "Edit Item";
            var layout = new StackLayout
            {
                Padding = 10
            };
            var lblItemName = new Label
            {
                Text = "Item Name"
            };
            var ItemName = new Entry
            {
                Placeholder="Enter New Name",
                Text= editViewModel.ItemName    
            };
            ItemName.SetBinding(Entry.TextProperty, EditViewModel.ItemNamePropertyName);
            layout.Children.Add(lblItemName);
            layout.Children.Add(ItemName);

            var lblPrice = new Label
            {
                Text = "Price"
            };
            var Price = new Entry
            {
                Placeholder="Enter New price",
                Text= editViewModel.Price
            };
            Price.SetBinding(Entry.TextProperty, EditViewModel.PricePropertyName);
            layout.Children.Add(lblPrice);
            layout.Children.Add(Price);

            var lblDescription = new Label
            {
                Text = "Description"
            };
            var Description = new Editor
            {
                HeightRequest=200,
                Text = editViewModel.Description
            };
            Description.SetBinding(Editor.TextProperty, EditViewModel.DescriptionPropertyName);
            layout.Children.Add(lblDescription);
            layout.Children.Add(Description);

            img = new Image
            {

                WidthRequest = 300,
                HeightRequest = 200,
                IsVisible = false,
                HorizontalOptions = LayoutOptions.Center
            };
            layout.Children.Add(img);

            var PickPhoto = new Button
            {
                Text = "Pick Photo",
                TextColor = Color.White,
                BackgroundColor =
               Color.FromHex("#8BC34A")
            };
            PickPhoto.Clicked += PickPhoto_Clicked;
            var TakePhoto = new Button
            {
                Text = "Take Photo",
                TextColor = Color.White,
                BackgroundColor =
               Color.FromHex("#8BC34A")
            };
            TakePhoto.Clicked += TakePhoto_Clicked;
            editViewModel.Img = tradeItem.Lstsrc;

            var sublayout = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                Children =
                {
                    PickPhoto,
                    TakePhoto
                }
            };

            layout.Children.Add(sublayout);

            var Update = new Button
            {
                Text = "UPDATE ITEM"
            };
            Update.SetBinding(Button.CommandProperty, EditViewModel.UpdateCommandPropertyName);
            layout.Children.Add(Update);


            Content = layout;
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

                try
                {
                    return mediaFile.GetStream();
                }
                catch
                {
                    return null;
                }

            });
            img.IsVisible = true;
            editViewModel.ImgSrc = mediaFile.GetStream();
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
                    Name = editViewModel.ItemName + ".png",
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
            editViewModel.ImgSrc = mediaFile.GetStream();
        }
    }
}