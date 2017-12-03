using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trade.Models.TableViewModel
{
    public class TradeItem
    {
        public string ItemRef { get; set; }
        public string ItemName { get; set; }
        public string UserName { get; set; }
        public double ItemPrice { get; set; }
        public string ItemDescription { get; set; }
        public byte[] src { get; set; }
        public List<byte[]> Lstsrc { get; set; }
        public DateTime date { get; set; }
        public bool isSelected { get; set; }
    }
}
