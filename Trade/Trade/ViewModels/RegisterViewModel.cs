using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trade.Helpers;
using Trade.Models;
using Trade.Views;
using Xamarin.Forms;

namespace Trade.ViewModels
{
    public class RegisterViewModel : BaseViewModel
    {
        readonly Itrade itrade;
        Page page;
        public RegisterViewModel(Page page)
        {
            this.itrade = DependencyService.Get<Itrade>();
            this.page = page;
        }
        public const string RegisterCommandPropertyName = "RegisterCommand";
        Command registerCommand;
        public Command RegisterCommand
        {
            get
            {
                return registerCommand ??
                    (registerCommand = new Command(async () => await ExecuteRegisterCommand()));
            }
        }
        private async Task ExecuteRegisterCommand()
        {
            bool isRegisterSuccess = false;
            bool isLoginSuccess = false;
          
            if (IsBusy)
                return;

            IsBusy = true;
            registerCommand.ChangeCanExecute();

            try
            {

                isRegisterSuccess = await itrade.RegisterAsync(this.UserName, this.Password, this.Confirmpassword);

            }
            catch (Exception ex)
            {
                isRegisterSuccess = false;
            }

            finally
            {
                IsBusy = false;
                registerCommand.ChangeCanExecute();
            }

            if (isRegisterSuccess)
            {
                await page.DisplayAlert("Feedback", "You have successfully Joined Us", "Ok");
                IsBusy = true;
                try
                {
                    isLoginSuccess = await itrade.GetAccessToken(this.UserName, this.Password);
                }
                catch (Exception)
                {

                    isLoginSuccess = false; 
                }
                finally
                {
                    IsBusy = false;
                    registerCommand.ChangeCanExecute();
                }
                if(isLoginSuccess)
                {
                    Settings.AuthUserName = this.username;
                    await page.Navigation.PushAsync(new TradeMainPage());
                }
                else
                {
                    await page.DisplayAlert("Login Error", "Login Error! please try Again", "Ok");
                }
            }
            else
            {
                await page.DisplayAlert("Registration Error", "An Error occured! Please try Again later", "Ok");

            }
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

        string confirmpassword = string.Empty;
        public const string ComfirmPasswordPropertyName = "Confirmpassword";
        public string Confirmpassword
        {
            get { return confirmpassword; }
            set { SetProperty(ref confirmpassword, value, ComfirmPasswordPropertyName); }
        }
    }
}


