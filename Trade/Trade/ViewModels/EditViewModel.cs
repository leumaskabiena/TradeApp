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

namespace Trade.ViewModels
{
    public class EditViewModel:BaseViewModel
    {
        readonly Itrade itrade;
        public EditViewModel()
        {
            this.itrade = DependencyService.Get<Itrade>();
        }
        public EditViewModel(TradeItem tradeItem)
        {
            this.itrade = DependencyService.Get<Itrade>();
            itemRef = tradeItem.ItemRef;
            itemName = tradeItem.ItemName;
            price = tradeItem.ItemPrice.ToString();
            description = tradeItem.ItemDescription;
      
        }
        string itemRef;
        public const string ItemRefPropertyName = "ItemRef";
        public string ItemRef
        {
            get { return itemRef; }
            set { SetProperty(ref itemRef, value, ItemRefPropertyName); OnPropertyChanged(); }
        }
        string itemName;
        public const string ItemNamePropertyName = "ItemName";
        public string ItemName
        {
            get { return itemName; }
            set { SetProperty(ref itemName, value, ItemNamePropertyName); OnPropertyChanged(); }
        }

        string price = string.Empty;
        public const string PricePropertyName = "Price";
        public string Price
        {
            get { return price; }
            set { SetProperty(ref price, value, PricePropertyName); OnPropertyChanged(); }
        }

        string description;
        public const string DescriptionPropertyName = "Description";
        public string Description
        {
            get { return description; }
            set { SetProperty(ref description, value, DescriptionPropertyName); OnPropertyChanged(); }
        }
        //Get the new pic 
        Stream imgSrc;
        public Stream ImgSrc
        {
            get => imgSrc;

            set
            {
                imgSrc = value;
                OnPropertyChanged();
            }
        }

        List<byte[]> img;
        public List<byte[]> Img
        {
            get => img;
            set => img = value;
        }
        public const string UpdateCommandPropertyName = "UpdateCommand";
        Command updateCommand;
        public Command UpdateCommand
        {
            get
            {
                return updateCommand ??
                    (updateCommand = new Command(async () => await ExecuteUpdateCommand()));
            }
        }

        

        private async Task ExecuteUpdateCommand()
        {
            bool IsUpdated = false;
            if (IsBusy)
                return;

            IsBusy = true;
            updateCommand.ChangeCanExecute();

            try
            {
                
                var tradeItem = new TradeItem();

                tradeItem.ItemRef = ItemRef;
                tradeItem.ItemName = ItemName;
                tradeItem.ItemPrice = double.Parse(Price);
                tradeItem.ItemDescription = Description;
                //there is no new pic
                if (imgSrc == null)
                {
                    tradeItem.Lstsrc = Img;
                }
                //there is a new pic
                else
                {
                    var stream = ImgSrc;

                    var memoryStream = new System.IO.MemoryStream();
                    stream.CopyTo(memoryStream);
                    stream = null;
                    Img.Add(memoryStream.ToArray());
                    tradeItem.Lstsrc = Img;
                }
                IsUpdated = await itrade.EditItemAsync(tradeItem);

            }
            catch
            {
                IsUpdated = false;
            }
            finally
            {
                IsBusy = false;
                updateCommand.ChangeCanExecute();
            }
            if(IsUpdated)
            {
                await Application.Current.MainPage.DisplayAlert("Trade", "Item Successufuly Update", "OK");
                await Application.Current.MainPage.Navigation.PushAsync(new More(ItemRef));
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Trade", "An Error occured Please Try Later", "OK");
            }
        }
    }
}
