using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trade.Models.TableViewModel;
namespace Trade.Models
{
    public interface Itrade
    {
        Task<bool> GetAccessToken(string username, string password);
        Task<bool> RegisterAsync(string username, string password, string confPassword);
        Task<bool> PostItemAsync(TradeItem model);
        Task<List<HomeItem>> GetHomeItemAsync();
        Task<List<TradeItem>> GetMyItemAsync();
        Task<bool> BetItem(BetItem model);
        Task<int> GetNumberOfBet();
        Task<List<BetItem>> GetMyNotUpdateBetList();
        Task<bool> BetToUpdate(UpdateBet model);
        Task<TradeItem> GetItemDetailAsync(string id);
        Task<bool> DeleteItemAsync(string id);
        Task<bool> EditItemAsync(TradeItem model);
        Task<List<BetItem>> GetMyUpdateBetList();
        Task UpdateList(string itemref);
    }
}
