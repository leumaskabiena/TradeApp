using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Trade.Models.TableViewModel;
using Trade.ViewModels;
using Xamarin.Forms;

namespace Trade.Style
{
    public class CustomListview : ListView
    {
        public static BindableProperty ItemClickCommandProperty = BindableProperty.Create(nameof(ItemClickCommand), typeof(ICommand), typeof(CustomListview), null);

        public ICommand ItemClickCommand
        {
            get
            {
                return (ICommand)this.GetValue(ItemClickCommandProperty);
            }
            set
            {
                this.SetValue(ItemClickCommandProperty, value);
            }
        }
        public CustomListview()
        {
            this.ItemTapped += CustomListview_ItemTapped;
        }

        private void CustomListview_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            string source = e.Item.GetType().Name;

            switch (source)
            {
                case "HomeItem":
                    if (e.Item != null)
                    {
                        var vm = BindingContext as HomeViewModel;
                        //  ItemClickCommand?.Execute(e.Item);
                        HomeItem homeItem = e.Item as HomeItem;
                        vm.ShowExtraButtton(homeItem);
                        SelectedItem = null;
                    }
                    break;
                case "TradeItem":
                    if (e.Item != null)
                    {
                        var vm = BindingContext as MyTradeViewModel;
                        //  ItemClickCommand?.Execute(e.Item);
                        TradeItem tradeItem = e.Item as TradeItem;
                        vm.ShowExtraButtton(tradeItem);
                        SelectedItem = null;
                    }
                    break;
                case "BetItem":
                    if (e.Item != null)
                    {
                        try
                        {
                            var vm = BindingContext as NotifyInViewModel;
                            //  ItemClickCommand?.Execute(e.Item);
                            BetItem betItem = e.Item as BetItem;
                            vm.ShowExtraButtton(betItem);
                            SelectedItem = null;
                        }
                        catch (Exception)
                        {
                            var vm = BindingContext as NotifyOutViewModel;
                            //  ItemClickCommand?.Execute(e.Item);
                            BetItem betItem = e.Item as BetItem;
                            vm.ShowExtraButtton(betItem);
                            SelectedItem = null;
                        }
                    }
                    break;
            }
        }
    }
}
