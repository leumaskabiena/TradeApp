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
    public class NotifyOutViewModel:BaseViewModel
    {
        readonly Itrade itrade;
        ObservableCollection<BetItem> notificationList = new ObservableCollection<BetItem>();
        public NotifyOutViewModel()
        {
            this.itrade = DependencyService.Get<Itrade>();
            GetNotification();
        }
       
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
            var lst = await itrade.GetMyUpdateBetList();
            var betItem = new BetItem();
            foreach (var item in lst.ToList())
            {
                betItem.itemref = item.itemref;
                betItem.Currentprice = item.Currentprice;
                betItem.date = item.date;
                betItem.ItemName = item.ItemName;
                betItem.ItemDescription = item.ItemDescription;

                if (item.IsAccept==1)
                {
                    betItem.Message = "Bet Was declined";
                }
                else if(item.IsAccept == 2)
                {
                    betItem.Message = "Bet Was Accept";
                }
                NotificationList.Add(betItem);
            }
        }
        //Update List
        async void UpdateList(string itemref)
        {
            await itrade.UpdateList(itemref);
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
                //update IsRead
                UpdateList(betItem.itemref);
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
    }
}
