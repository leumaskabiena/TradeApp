using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trade.Models;
using Trade.Models.TableViewModel;
using Trade.Views;
using Xamarin.Forms;

namespace Trade.ViewModels
{
    public class BetViewModel : BaseViewModel
    {
        readonly Itrade itrade;

        public BetViewModel()
        {
            this.itrade = DependencyService.Get<Itrade>();
        }

        string itemRef;
        public const string ItemRefPropertyName = "ItemRef";
        public string ItemRef
        {
            get { return itemRef; }
            set { SetProperty(ref itemRef, value, ItemRefPropertyName); }
        }

        double price = 0;
        public const string PricePropertyName = "Price";
        public double Price
        {
            get { return price; }
            set { SetProperty(ref price, value, PricePropertyName); }
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

            if (IsBusy)
                return;

            IsBusy = true;
            betCommand.ChangeCanExecute();
            try
            {
                var data = new BetItem
                {
                    Newprice = this.Price,
                    itemref = this.ItemRef
                };
                isBetSucess = await itrade.BetItem(data);


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
                await Application.Current.MainPage.DisplayAlert("Feedback", "You Have successufuly bet", "Ok");
                await Application.Current.MainPage.Navigation.PushModalAsync(new TradeMainPage());
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Bet Error", "Bet Error! please try Again", "Ok"); 
            }
        }
    }
}
