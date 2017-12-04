using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trade.Models;
using Trade.Models.TableViewModel;
using Xamarin.Forms;

namespace Trade.ViewModels
{
    public class NotifyInViewModel:BaseViewModel
    {
        readonly Itrade itrade;
        public NotifyInViewModel()
        {
            this.itrade = DependencyService.Get<Itrade>();
            GetNotification();
        }
        ObservableCollection<BetItem> notificationList = new ObservableCollection<BetItem>();
        public ObservableCollection<BetItem> NotificationList
        {
            get => notificationList;
            set
            {
                SetProperty(ref notificationList, value);
            }
        }
        async Task GetNotification()
        {
            var lst = await itrade.GetMyNotUpdateBetList();
            foreach (var item in lst.ToList())
            {
                BetItem betItem = new BetItem
                {
                    itemref = item.itemref,
                    Currentprice = item.Currentprice,
                    Newprice = item.Newprice,
                    BetterName = item.BetterName,
                    IsAccept = item.IsAccept,
                    date = item.date,
                    ItemName = item.ItemName,
                    ItemDescription = item.ItemDescription
                };
                NotificationList.Add(betItem);
            }
        }
        BetItem DuplicateBetItem;
        public void ShowExtraButtton(BetItem betItem)
        {
            if (DuplicateBetItem == betItem)
            {
                //click twice on the same item it will hide
                betItem.isSelected = !betItem.isSelected;
                UpdatePerson(betItem);
            }
            else
            {
                if (DuplicateBetItem != null)
                {
                    //hide previous selected item
                    DuplicateBetItem.isSelected = false;
                    UpdatePerson(DuplicateBetItem);
                }
                //show seletected item
                betItem.isSelected = true;
                UpdatePerson(betItem);
            }
            DuplicateBetItem = betItem;
        }

        private void UpdatePerson(BetItem betItem)
        {
            var index = NotificationList.IndexOf(betItem);
            NotificationList.Remove(betItem);
            NotificationList.Insert(index, betItem);
        }
        public const string DeclineCommandPropertyName = "DeclineCommand";
        Command declineCommand;
        public Command DeclineCommand
        {
            get
            {
                return declineCommand ??
                    (declineCommand = new Command(async () => await ExecuteDeclineCommand()));
            }
        }

        private async Task ExecuteDeclineCommand()
        {
            bool IsDecline = false;
            if (IsBusy)
                return;

            declineCommand.ChangeCanExecute();
            try
            {
                UpdateBet updateBet = new UpdateBet
                {
                    id = DuplicateBetItem.itemref,
                    ans = 1
                };
                IsDecline = await (itrade.BetToUpdate(updateBet));
            }
            catch (Exception ex)
            {
                IsDecline = false;
            }
            finally
            {
                IsBusy = false;
                declineCommand.ChangeCanExecute();
            }
            if (IsDecline)
            {
                await Application.Current.MainPage.DisplayAlert("Feedback", "You have Declined the Bet", "Ok");
            }
            {
                await Application.Current.MainPage.DisplayAlert("Feedbackr", "An Error Occured! please try Again", "Ok");
            }
        }
        public const string AcceptCommandPropertyName = "AcceptCommand";
        Command acceptCommand;
        public Command AcceptCommand
        {
            get
            {
                return acceptCommand ??
                    (acceptCommand = new Command(async () => await ExecuteAcceptCommand()));
            }
        }

        private async Task ExecuteAcceptCommand()
        {
            bool IsAccept = false;
            if (IsBusy)
                return;

            acceptCommand.ChangeCanExecute();
            try
            {
                UpdateBet updateBet = new UpdateBet
                {
                    id = DuplicateBetItem.itemref,
                    ans = 2
                };
                IsAccept = await (itrade.BetToUpdate(updateBet));
            }
            catch (Exception ex)
            {
                IsAccept = false;
            }
            finally
            {
                IsBusy = false;
                acceptCommand.ChangeCanExecute();
            }
            if (IsAccept)
            {
                await Application.Current.MainPage.DisplayAlert("Feedback", "You have accepted the Bet", "Ok");
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Feedbackr", "An Error Occured! please try Again", "Ok");
            }
        }
    }
}
