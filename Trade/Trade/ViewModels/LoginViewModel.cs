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

        public const string LogoutCommandPropertyName = "LogoutCommand";
        Command logoutCommand;
        public Command LogoutCommand
        {
            get
            {
                return logoutCommand ??
                    (logoutCommand = new Command(async () => await ExecuteLogoutCommand()));
            }
        }

        private async Task ExecuteLogoutCommand()
        {
            bool isLogOutSuccess = false;
            if (IsBusy)
                return;

            IsBusy = true;
            logoutCommand.ChangeCanExecute();
            try
            {
                Settings.AuthLoginToken = string.Empty;
                Settings.AuthUserName = "";
                isLogOutSuccess = true;
            }
            catch(Exception ex)
            {
                isLogOutSuccess = false;
            }
            finally
            {
                IsBusy = false;
                logoutCommand.ChangeCanExecute();
            }
            if(isLogOutSuccess)
            {
                await page.DisplayAlert("Logout", "Successful", "Ok");
            }
            else
            {
                await page.DisplayAlert("Logout Error", "Logout Error! please try Again", "Ok");
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
               
                if (Device.OS == TargetPlatform.Android)
                 await Application.Current.MainPage.Navigation.PushModalAsync(new MainPage());
                else
                    await page.Navigation.PushModalAsync(new TradeMainPage());
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

