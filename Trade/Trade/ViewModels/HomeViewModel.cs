using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trade.Helpers;
using Trade.Models;
using Trade.Models.TableViewModel;
using Trade.Views;
using Trade.Views.Xaml;
using Xamarin.Forms;

namespace Trade.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        readonly Itrade itrade;
        Page page;
        ObservableCollection<HomeItem> homeItemList = new ObservableCollection<HomeItem>();
        string message = string.Empty;
        public const string MessagePropertyName = "Message";
        public string Message
        {
            get { return message; }
            set
            {
                if (HomeItemList.Count() <= 0)
                    message = "Your List is Empty";
                SetProperty(ref message, value, MessagePropertyName);
            }
        }
        
        public HomeViewModel()
        {
            this.itrade = DependencyService.Get<Itrade>();
            GetHomeItem();
           
        }

        public ObservableCollection<HomeItem> HomeItemList
        {
            get => homeItemList;
            set
            {
                SetProperty(ref homeItemList, value);
            }
        }
        
        async Task GetHomeItem()
        {
            var lst = await itrade.GetHomeItemAsync();
            foreach (var item in lst.ToList())
            {
                HomeItem homeItem = new HomeItem
                {
                    id = item.id,
                    title = item.title,
                    price = item.price,
                    Name = item.Name,
                    src = item.src,
                    description = item.description
                };
                HomeItemList.Add(homeItem);
            }
        }

        HomeItem DuplicateHomeItem;
        internal void ShowExtraButtton(HomeItem homeItem)
        {
            if (DuplicateHomeItem == homeItem)
            {
                //click twice on the same item it will hide
                homeItem.isSelected = !homeItem.isSelected;
                if (homeItem.Name!=Settings.AuthUserName)
                {
                    homeItem.isBetVisible = !homeItem.isBetVisible; 
                }
                UpdatePerson(homeItem);
            }
            else
            {
                if (DuplicateHomeItem != null)
                {
                    //hide previous selected item
                    DuplicateHomeItem.isSelected = false;
                    if (DuplicateHomeItem.Name!= Settings.AuthUserName)
                    {
                        DuplicateHomeItem.isBetVisible = false; 
                    }
                    UpdatePerson(DuplicateHomeItem);
                }
                //show seletected item
                homeItem.isSelected = true;
                if (homeItem.Name!= Settings.AuthUserName)
                {
                    homeItem.isBetVisible = true; 
                }
                UpdatePerson(homeItem);
            }
            DuplicateHomeItem = homeItem;
        }

        private void UpdatePerson(HomeItem homeItem)
        {
            var index = HomeItemList.IndexOf(homeItem);
            HomeItemList.Remove(homeItem);
            HomeItemList.Insert(index, homeItem);
        }

        public const string MoreCommandPropertyName = "MoreCommand";
        Command moreCommand;
        public Command MoreCommand
        {
            get
            {
                return moreCommand ??
                    (moreCommand = new Command(async () => await ExecuteMoreCommand()));
            }
        }

        private async Task ExecuteMoreCommand()
        {
            bool isMoreSuccess = false;
            string Itemref = string.Empty;
            if (IsBusy)
                return;

            moreCommand.ChangeCanExecute();

            try
            {
                Itemref = DuplicateHomeItem.id;
                isMoreSuccess = true;
            }
            catch (Exception ex)
            {
                isMoreSuccess = false;
            }
            finally
            {
                IsBusy = false;
                moreCommand.ChangeCanExecute();
            }
            if (isMoreSuccess)
            {
                await Application.Current.MainPage.Navigation.PushAsync(new More(Itemref));
            }

        }

        public const string BetCommandPropertyName = "BetCommand";
        Command betCommand;
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
            bool isBetSucess = false;
            HomeItem data = new HomeItem();

            if (IsBusy)
                return;

            IsBusy = true;
            betCommand.ChangeCanExecute();
            try
            {
                data = DuplicateHomeItem;
                isBetSucess = true;
            }
            catch (Exception ex)
            {
                isBetSucess = false;
            }
            finally
            {
                IsBusy = false;
                betCommand.ChangeCanExecute();
            }
            if (isBetSucess)
            {
                if (string.IsNullOrEmpty(Settings.AuthLoginToken))
                {
                   var rst = await Application.Current.MainPage.DisplayAlert("Login", "You must Login! please try Again", "Ok", "Cancel");
                    if (rst)
                    {
                        await Application.Current.MainPage.Navigation.PushAsync(new LoginPage()); 
                    }
                    else
                    {
                        //await Application.Current.MainPage.Navigation.PushAsync(new TradeMainPage());
                        if (DuplicateHomeItem != null)
                        {
                            //hide previous selected item
                            DuplicateHomeItem.isSelected = false;
                            UpdatePerson(DuplicateHomeItem);
                        }
                    }
                }
                else
                {
                    await Application.Current.MainPage.Navigation.PushAsync(new Bet(data));
                }
            }
        }
    }
}
