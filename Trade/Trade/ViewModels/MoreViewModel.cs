using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trade.Helpers;
using Trade.Models;
using Trade.Models.TableViewModel;
using Trade.Views;
using Xamarin.Forms;

namespace Trade.ViewModels
{
    public class MoreViewModel : BaseViewModel
    {
        readonly Itrade itrade;
        string itemName = string.Empty;
        string price = string.Empty;
        string date = string.Empty;
        string itemRef = string.Empty;
        string ItemID = string.Empty;
        string description = string.Empty;
        string sellerName = string.Empty;
        bool isDeleteAndEditVisible = false;
        bool isBetVisible = false;

        List<ImageSrc> lstSrc = new List<ImageSrc>();

        Command deleteCommand;
        Command editCommand;
        Command betCommand;

        public string ItemName
        {
            get => itemName;
            set { itemName = value;  OnPropertyChanged();}
        }
        public string Price
        {
            get => price;
            set { price = value; OnPropertyChanged(); }
        }
        public string Date
        {
            get => date;
            set { date = value; OnPropertyChanged(); }
        }
        public string ItemRef
        {
            get => itemRef;
            set { itemRef = value; OnPropertyChanged(); }
        }
        public string ItemReference
        {
            get => ItemID;
            set { ItemID = value; OnPropertyChanged(); }
        }
        public string Description
        {
            get => description;
            set { description = value; OnPropertyChanged(); }
        }
        public string SellerName
        {
            get => sellerName;
            set { sellerName = value; OnPropertyChanged(); }
        }
        public List<ImageSrc> LstSrc
        {
            get => lstSrc;
            set { lstSrc = value; OnPropertyChanged(); }
        }
        public bool IsDeleteAndEditVisible
        {
            get => isDeleteAndEditVisible;
            set { isDeleteAndEditVisible = value; OnPropertyChanged(); }
        }
        public bool IsBetVisible
        {
            get => isBetVisible;
            set { isBetVisible = value; OnPropertyChanged(); }
        }

        public MoreViewModel(string id)
        {
            this.itrade = DependencyService.Get<Itrade>();
            GetItem(id);
        }
       
        async void GetItem(string id)
        {
            var x = await itrade.GetItemDetailAsync(id);
            ItemName = x.ItemName;
            ItemReference = x.ItemRef;
            ItemRef = "Item Ref : " + x.ItemRef;
            Price ="Item Price : "+ x.ItemPrice;
            Description = "Item Description : " + x.ItemDescription;
            Date ="Created on "+ x.date.ToString();
            SellerName = "Seller Name : " + x.UserName;

            List<ImageSrc> templst = new List<ImageSrc>();
            foreach (var item in x.Lstsrc)
            {
                ImageSrc z = new ImageSrc
                {
                    src = item
                };
                templst.Add(z);
            }
            LstSrc = templst;

            // check if item belong to the user
            if (Settings.AuthUserName.Equals(x.UserName))
                IsDeleteAndEditVisible = true;

            else
            {
                //check if you are login
                if(!Settings.AuthLoginToken.Equals(string.Empty))
                    IsBetVisible = true;
            }
                
        }
        
        public Command DeleteCommand
        {
            get
            {
                return deleteCommand ??
                    (deleteCommand = new Command(async () => await ExecuteDeleteCommand()));
            }
        }
        private async Task ExecuteDeleteCommand()
        {
            bool Warning = await Application.Current.MainPage.DisplayAlert("Delete", "You are about to Delete", "Continue", "Cancel");
            if (Warning)
            {
                bool IsDelete = false;

                if (IsBusy)
                    return;

                IsBusy = true;
                deleteCommand.ChangeCanExecute();

                try
                {
                    IsDelete = await itrade.DeleteItemAsync(ItemReference);
                }
                catch (Exception ex)
                {
                    IsDelete = false;
                    await Application.Current.MainPage.DisplayAlert("Feedback", "Delete Not Successful", "Try Again");
                }
                finally
                {
                    IsBusy = false;
                    deleteCommand.ChangeCanExecute();
                } 
                if(IsDelete)
                {
                    await Application.Current.MainPage.DisplayAlert("Feedback", "Delete Successful", "OK");
                    await Application.Current.MainPage.Navigation.PushAsync(new TradeMainPage());
                }
            }
            
        }


        public Command EditCommand
        {
            get
            {
                return editCommand ??
                    (editCommand = new Command(async () => await ExecuteEditCommand()));
            }
        }
        private async Task ExecuteEditCommand()
        {
            bool IsEdit = false;
           
            if (IsBusy)
                return;

            IsBusy = true;
            editCommand.ChangeCanExecute();

            TradeItem tradeItem = new TradeItem();
            try
            {
               
                tradeItem = await itrade.GetItemDetailAsync(ItemReference);
               
            }
            catch (Exception ex)
            {
                IsEdit = false;
            }
            finally
            {
                IsBusy = false;
                IsEdit = true;
                editCommand.ChangeCanExecute();
            }
            if(IsEdit)
            {
                await Application.Current.MainPage.Navigation.PushAsync(new ItemEdit(tradeItem));
            }
        }

        public Command BetCommand
        {
            get
            {
                return betCommand ??
                    (betCommand = new Command(async () => await ExecuteBetCommand()));
            }
        }
        private async Task ExecuteBetCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;
            betCommand.ChangeCanExecute();

            try
            {
                HomeItem homeItem = new HomeItem
                {
                    id = ItemReference,
                    title = ItemName,
                    Name = SellerName,
                    price = double.Parse(Price),
                    description = Description,
                    Lstsrc = LstSrc
                };
                await Application.Current.MainPage.Navigation.PushAsync(new Bet(homeItem));
                IsBusy = false;
                betCommand.ChangeCanExecute();
            }
            catch (Exception)
            {
                await Application.Current.MainPage.DisplayAlert("Trade", "An Error occured Please Try Later", "OK");
            }
            
        }
    }
}
