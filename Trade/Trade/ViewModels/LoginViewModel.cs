using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trade.Helpers;
using Trade.Models;
using Trade.Views;
using Trade.Views.Xaml;
using Xamarin.Forms;

namespace Trade.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        readonly Itrade itrade;
        Page page;
        public LoginViewModel(Page page)
        {
            this.itrade = DependencyService.Get<Itrade>();
            this.page = page;
        }

        string username = string.Empty;
        public const string UsernamePropertyName = "UserName";
        public string UserName
        {
            get { return username; }
            set { SetProperty(ref username, value, UsernamePropertyName); }
        }

        string password = string.Empty;
        public const string PasswordPropertyName = "Password";
        public string Password
        {
            get { return password; }
            set { SetProperty(ref password, value, PasswordPropertyName); }
        }

        public const string LoginCommandPropertyName = "LoginCommand";
        Command loginCommand;
        public Command LoginCommand
        {
            get
            {
                return loginCommand ??
                    (loginCommand = new Command(async () => await ExecuteLoginCommand()));
            }
        }
      
        private async Task ExecuteLoginCommand()
        {

            bool isLoginSuccess = false;
            if (IsBusy)
                return;

            IsBusy = true;
            loginCommand.ChangeCanExecute();

            try
            {
                isLoginSuccess = await itrade.GetAccessToken(this.UserName, this.Password);
                
            }
            catch (Exception ex)
            {
                isLoginSuccess = false;
            }

            finally
            {
                IsBusy = false;
                loginCommand.ChangeCanExecute();
            }

            if (isLoginSuccess)
            {
                await page.Navigation.PushModalAsync(new TradeMainPage());
                //if (Device.OS == TargetPlatform.Android)
                //    Application.Current.MainPage = new TradeMainPage();
                //await Application.Current.MainPage.Navigation.PushAsync(new Home());
            }
            else
            {
               await page.DisplayAlert("Login Error", "Login Error! please try Again", "Ok");
            }
        }

        public const string JoinUsCommandPropertyName = "JoinUsCommand";
        Command joinUsCommand;
        public Command JoinUsCommand
        {
            get
            {
                return joinUsCommand ??
                    (joinUsCommand = new Command(async () => await ExecuteJoinUsCommand()));
            }
        }

        private async Task ExecuteJoinUsCommand()
        {
            joinUsCommand.ChangeCanExecute();
            await page.Navigation.PushAsync(new RegisterPage());
        }
    }
}

