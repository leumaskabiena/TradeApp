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
using Xamarin.Forms;

namespace Trade.ViewModels
{
    public class MyTradeViewModel : BaseViewModel
    {
        readonly Itrade itrade;
        ObservableCollection<TradeItem> myItemList = new ObservableCollection<TradeItem>();
        Page page;
        public MyTradeViewModel()
        {
            this.itrade = DependencyService.Get<Itrade>();
            this.page = page;
            GetMyItem();
            // Redirect();
        }
        public ObservableCollection<TradeItem> MyItemList
        {
            get => myItemList;
            set
            {
                SetProperty(ref myItemList, value);
            }
        }
        async Task GetMyItem()
        {
            var lst = await itrade.GetMyItemAsync();
            foreach (var item in lst.ToList())
            {
                TradeItem tradeItem = new TradeItem
                {
                    ItemName = item.ItemName,
                    ItemDescription = item.ItemDescription,
                    src=item.src
                };
                myItemList.Add(tradeItem);
            }
        }
        private async Task Redirect()
        {
            var loginToken = Settings.AuthLoginToken;
            if (string.IsNullOrEmpty(loginToken))
                await page.Navigation.PushModalAsync(new LoginPage());
        }

        TradeItem DuplicateTradeItem;
        internal void ShowExtraButtton(TradeItem tradeItem)
        {
            if (DuplicateTradeItem == tradeItem)
            {
                //click twice on the same item it will hide
                tradeItem.isSelected = !tradeItem.isSelected;
                UpdatePerson(tradeItem);
            }
            else
            {
                if (DuplicateTradeItem != null)
                {
                    //hide previous selected item
                    DuplicateTradeItem.isSelected = false;
                    UpdatePerson(DuplicateTradeItem);
                }
                //show seletected item
                tradeItem.isSelected = true;
                UpdatePerson(tradeItem);
            }
            DuplicateTradeItem = tradeItem;
        }

        private void UpdatePerson(TradeItem tradeItem)
        {
            var index = MyItemList.IndexOf(tradeItem);
            MyItemList.Remove(tradeItem);
            MyItemList.Insert(index, tradeItem);
        }
    }
}
