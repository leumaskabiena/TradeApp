using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Trade.Views.Xaml;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Trade.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public class TradeMainPage : MasterDetailPage
    {
        MenuPage menuPage;
        public TradeMainPage()
        {
            
            menuPage = new MenuPage(this);
            Master = menuPage;
            Detail = new Home() { Title="Home"};
        }
    }
}