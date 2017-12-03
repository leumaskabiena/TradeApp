using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trade.Models;
using Trade.Models.TableViewModel;
using Trade.Views.Xaml;
using Xamarin.Forms;
using Trade.Views;

namespace Trade.ViewModels
{
    public class TradeViewModel : BaseViewModel
    {
        readonly Itrade itrade;
       
        TradeMainPage rootPage;
        public TradeViewModel()
        {
            this.itrade = DependencyService.Get<Itrade>();
           
        }

        string name = string.Empty;
        public const string NamePropertyName = "Name";
        public string Name
        {
            get { return name; }
            set { SetProperty(ref name, value, NamePropertyName); }
        }

        string price = string.Empty;
        public const string PricePropertyName = "Price";
        public string Price
        {
            get { return price; }
            set { SetProperty(ref price, value, PricePropertyName); }
        }

        string description = string.Empty;
        public const string DescriptionPropertyName = "Description";
        public string Description
        {
            get { return description; }
            set { SetProperty(ref description, value, DescriptionPropertyName); }
        }

        Stream imgSrc;
        public  Stream ImgSrc
        {
            get => imgSrc;
            
            set
            {
                imgSrc = value;
                OnPropertyChanged();
            }
        }
        public void GetStreamPicture(Stream stream)
        {
            ImgSrc = stream;
        }
        public static Stream stream;

        #region Pick and Take Picture
        public const string PickPhotoCommandPropertyName = "PickPhotoCommand";
        Command pickPhotoCommand;
        public Command PickPhotoCommand
        {
            get
            {
                return pickPhotoCommand ??
                    (pickPhotoCommand = new Command(async () => await ExecutePickPhotoCommand()));
            }
        }

        private async Task ExecutePickPhotoCommand()
        {

            await CrossMedia.Current.Initialize();
            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                //await page.DisplayAlert("No Picture", ":( No Picture Available", "OK");
                return;
            }

            var file = await CrossMedia.Current.PickPhotoAsync();
            if (file == null)
            {

            }
            var stream = file.GetStream();
            file.Dispose();
            ImgSrc = stream;
            // OnPropertyChanged(ImgSrc);
            // rootPage.Detail = new TradePage();
            //await page.Navigation.PushModalAsync(new TradePage());            
        }

        public const string TakePhotoCommandPropertyName = "TakePhotoCommand";
        Command takePhotoCommand;
        public Command TakePhotoCommand
        {
            get
            {
                return takePhotoCommand ??
                    (takePhotoCommand = new Command(async () => await ExecuteTakePhotoCommand()));
            }
        }

        private async Task ExecuteTakePhotoCommand()
        {

            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
               // await page.DisplayAlert("No camera found", ":( NO Camera awailable", "OK");
                return;
            }
            var file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
            {
                // Directory="Sample",
                Name = NamePropertyName + ".png",
                SaveToAlbum = true
            });
            var stream = file.GetStream();
            file.Dispose();
            ImgSrc = stream;
        } 
        #endregion

        public const string CreateTradeCommandPropertyName = "CreateTradeCommand";
        Command createTradeCommand;
        public Command CreateTradeCommand
        {
            get
            {
                return createTradeCommand ??
                    (createTradeCommand = new Command(async () => await ExecuteCreateTradeCommand()));
            }
        }

        private async Task ExecuteCreateTradeCommand()
        {

            bool isCreateSucess = false;
            if (IsBusy)
                return;

            try
            {
                //string word = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
                //Random rd = new Random();
                //int num1 = rd.Next(-1, 24);
                //string StampDate = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString();
                //string ItemRef = word.Substring(num1, 3) + this.Name.ToUpper().Substring(0, 1) + StampDate;

                var stream = ImgSrc;
                
                var memoryStream = new System.IO.MemoryStream();
                stream.CopyTo(memoryStream);
                stream = null;

                TradeItem item = new TradeItem
                {
                   // ItemRef = ItemRef,
                    ItemName = this.Name,
                    ItemPrice = Double.Parse(this.Price),
                    ItemDescription = this.description,
                    src= memoryStream.ToArray()
                };


                isCreateSucess = await itrade.PostItemAsync(item);
            }
            catch (Exception ex)
            {
                isCreateSucess = false;
            }
            finally
            {
                IsBusy = false;
                createTradeCommand.ChangeCanExecute();
            }
            if (isCreateSucess)
            {
                await Application.Current.MainPage.DisplayAlert("Trade", "Successufuly create", "OK");
                if (Device.OS == TargetPlatform.Android)
                    Application.Current.MainPage = new Home();

                await Application.Current.MainPage.Navigation.PushAsync(new TradeMainPage());                
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Trade", "An Error occured Please Try Later", "OK");
            }
        }
    }
}
