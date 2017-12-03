using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trade.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Trade.Views.Xaml
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class More : ContentPage
    {
        MoreViewModel moreViewModel;
        public More(string id)
        {
            InitializeComponent();
            this.BindingContext = moreViewModel = new MoreViewModel(id);
                       
        }
    }
}