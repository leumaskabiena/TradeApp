using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Trade.Helpers;
using Trade.Models;
using Trade.Models.TableViewModel;
using Trade.Services;
using Xamarin.Forms;

[assembly: Dependency(typeof(TradeService))]
namespace Trade.Services
{
    public class TradeService : Itrade
    {
        static string baseUrl = "";

        public TradeService()
        {
            
        }
        public async Task<bool> GetAccessToken(string username, string password)
        {
            var httpClient = new HttpClient();
            baseUrl = "http://localhost:63862/Token";
            using (httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(baseUrl);

                // We want the response to be JSON.
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                // Build up the data to POST.
                List<KeyValuePair<string, string>> postData = new List<KeyValuePair<string, string>>();
                postData.Add(new KeyValuePair<string, string>("grant_type", "password"));
                postData.Add(new KeyValuePair<string, string>("username", username));
                postData.Add(new KeyValuePair<string, string>("password", password));
                FormUrlEncodedContent content = new FormUrlEncodedContent(postData);
                // Post to the Server and parse the response.
                try
                {
                    var response = await httpClient.PostAsync("Token", content);
                    response.EnsureSuccessStatusCode();
                    string jsonString = response.Content.ReadAsStringAsync().Result;

                    //object responseData = JsonConvert.DeserializeObject(jsonString);
                    Login responseData = JsonHelper.Deserialize<Login>(jsonString);
                    Settings.AuthLoginToken = responseData.access_token;

                    //Get Number of Bet that you have
                    await GetNumberOfBet();

                    return true;

                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }

        public async Task<bool> RegisterAsync(string username, string password, string confPassword)
        {
            var httpClient = new HttpClient();
            baseUrl = "http://localhost:63862/api/Account/Register";
            using (httpClient = new HttpClient())
            {
                //client.BaseAddress = new Uri(baseUrl);
                //client.DefaultRequestHeaders.Accept.Clear();
                //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var model = new Register();

                model.Email = username;
                model.Password = password;
                model.ConfirmPassword = confPassword;
                var json = JsonConvert.SerializeObject(model);
                HttpContent content = new StringContent(json);

                try
                {
                    content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                    var response = await httpClient.PostAsync(baseUrl, content);

                    return response.IsSuccessStatusCode;
                }
                catch
                {
                    return false;
                }
            }
        }

        public async Task<bool> PostItemAsync(TradeItem model)
        {

            baseUrl = "http://localhost:63862/api/Item/Create";
            var httpClient = new HttpClient();

            try
            {
                if (!string.IsNullOrEmpty(Settings.AuthLoginToken))
                {
                    httpClient.BaseAddress = new Uri(baseUrl);
                    httpClient.DefaultRequestHeaders.Accept.Clear();
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    // Add the Authorization header with the AccessToken.
                    httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + Settings.AuthLoginToken);

                    var json = JsonConvert.SerializeObject(model);

                    HttpContent httpContent = new StringContent(json);

                    httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                    var result = await httpClient.PostAsync(baseUrl, httpContent);
                    //update notification
                    Settings.Notificaton = await GetNumberOfBet();
                    return result.IsSuccessStatusCode;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<List<HomeItem>> GetHomeItemAsync()
        {
            var httpClient = new HttpClient();
            baseUrl = "http://localhost:63862/api/home/Index";
            List<HomeItem> Items = new List<HomeItem>();
            try
            {
                using (httpClient = new HttpClient())
                {
                    var response = await httpClient.GetAsync(baseUrl);
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        Items = JsonHelper.Deserialize<List<HomeItem>>(content);
                        //update notification
                        Settings.Notificaton = await GetNumberOfBet();
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }
            return Items;
        }

        public async Task<List<TradeItem>> GetMyItemAsync()
        {
            baseUrl = "http://localhost:63862/api/Item/MyIndex";
            List<TradeItem> Items = new List<TradeItem>();
            var httpClient = new HttpClient();
            if (!string.IsNullOrEmpty(Settings.AuthLoginToken))
            {
                httpClient.BaseAddress = new Uri(baseUrl);
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // Add the Authorization header with the AccessToken.
                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + Settings.AuthLoginToken);

                try
                {
                    var response = await httpClient.GetAsync(baseUrl);
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        // Items = JsonConvert.DeserializeObject<List<HomeItem>>(content);
                        Items = JsonHelper.Deserialize<List<TradeItem>>(content);
                        //update notification
                        Settings.Notificaton = await GetNumberOfBet();
                        return Items;
                    }

                }
                catch (Exception)
                {
                    return null;
                }

            }
            return null;

        }

        public async Task<bool> BetItem(BetItem model)
        {
            var httpClient = new HttpClient();
            baseUrl = "http://localhost:63862/api/Bet/CreateBet";
            if (!string.IsNullOrEmpty(Settings.AuthLoginToken))
            {
                httpClient.BaseAddress = new Uri(baseUrl);
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // Add the Authorization header with the AccessToken.
                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + Settings.AuthLoginToken);

                try
                {
                    var json = JsonConvert.SerializeObject(model);

                    HttpContent httpContent = new StringContent(json);

                    httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                    var result = await httpClient.PostAsync(baseUrl, httpContent);

                    return true;

                }
                catch (Exception)
                {
                    return false;
                }
            }
            return false;
        }

        public async Task<int> GetNumberOfBet()
        {
            baseUrl = "http://localhost:63862/api/Bet/Notify";
            int num=0;
            var httpClient = new HttpClient();
            if (!string.IsNullOrEmpty(Settings.AuthLoginToken))
            {
                httpClient.BaseAddress = new Uri(baseUrl);
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // Add the Authorization header with the AccessToken.
                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + Settings.AuthLoginToken);

                try
                {
                    var response = await httpClient.GetAsync(baseUrl);
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        num = JsonHelper.Deserialize<int>(content);
                        Settings.Notificaton = num;
                    }
                    
                }
                catch (Exception)
                {
                    num = -1;
                }

            }
            return num;

        }

        public async Task<List<BetItem>> GetMyNotUpdateBetList()
        {
            baseUrl = "http://localhost:63862/api/Bet/MyGetNotification";
            List<BetItem> Items = new List<BetItem>();
            var httpClient = new HttpClient();
            if (!string.IsNullOrEmpty(Settings.AuthLoginToken))
            {
                httpClient.BaseAddress = new Uri(baseUrl);
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // Add the Authorization header with the AccessToken.
                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + Settings.AuthLoginToken);

                try
                {
                    var response = await httpClient.GetAsync(baseUrl);
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        // Items = JsonConvert.DeserializeObject<List<HomeItem>>(content);
                        Items = JsonHelper.Deserialize<List<BetItem>>(content);
                        //update notification
                        Settings.Notificaton = await GetNumberOfBet();
                        return Items;
                    }

                }
                catch (Exception)
                {
                    return null;
                }

            }
            return null;
        }

        public async Task<bool> BetToUpdate(UpdateBet model)
        {
            var httpClient = new HttpClient();
            baseUrl = "http://localhost:63862/api/Bet/UpdateBet";
            if (!string.IsNullOrEmpty(Settings.AuthLoginToken))
            {
                httpClient.BaseAddress = new Uri(baseUrl);
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // Add the Authorization header with the AccessToken.
                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + Settings.AuthLoginToken);

                try
                {
                    var json = JsonConvert.SerializeObject(model);

                    HttpContent httpContent = new StringContent(json);

                    httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                    var result = await httpClient.PostAsync(baseUrl, httpContent);
                    //update notification
                    Settings.Notificaton = await GetNumberOfBet();
                    return true;

                }
                catch (Exception)
                {
                    return false;
                }
            }
            return false;
        }

        public async Task<TradeItem> GetItemDetailAsync(string id)
        {
            baseUrl = $"http://localhost:63862/api/item/Details/{id}";
            var item = new TradeItem();
            var httpClient = new HttpClient();
            
                httpClient.BaseAddress = new Uri(baseUrl);
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // Add the Authorization header with the AccessToken.
               // httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + Settings.AuthLoginToken);

                try
                {
                    var response = await httpClient.GetAsync(baseUrl);
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        item = JsonHelper.Deserialize<TradeItem>(content);
                    }
                return item;
                }
                catch (Exception)
                {
                   return item;
                }
        }

        public async Task<bool> DeleteItemAsync(string id)
        {
            baseUrl = $"http://localhost:63862/api/item/Delete/{id}";
           
            var httpClient = new HttpClient();

            httpClient.BaseAddress = new Uri(baseUrl);
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // Add the Authorization header with the AccessToken.
            httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + Settings.AuthLoginToken);

            try
            {
                var response = await httpClient.GetAsync(baseUrl);
                return response.IsSuccessStatusCode;

            }
            catch (Exception)
            {
                return false;
            }
         
        }

        public async Task<bool> EditItemAsync(TradeItem model)
        {
            baseUrl = "http://localhost:63862/api/Item/Edit";
            var httpClient = new HttpClient();

            try
            {
                if (!string.IsNullOrEmpty(Settings.AuthLoginToken))
                {
                    httpClient.BaseAddress = new Uri(baseUrl);
                    httpClient.DefaultRequestHeaders.Accept.Clear();
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    // Add the Authorization header with the AccessToken.
                    httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + Settings.AuthLoginToken);

                    var json = JsonConvert.SerializeObject(model);

                    HttpContent httpContent = new StringContent(json);

                    httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                    var result = await httpClient.PostAsync(baseUrl, httpContent);

                    return result.IsSuccessStatusCode; ;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
