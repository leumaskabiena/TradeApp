using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trade.Models.TableViewModel
{
    public class BetItem
    {
        public string itemref { get; set; }
        public double Currentprice { get; set; }
        public double Newprice { get; set; }
        public string BetterName { get; set; }
        public int IsAccept { get; set; }
        public DateTime date { get; set; }
        public string ItemName { get; set; }
        public string ItemDescription { get; set; }
        public bool isSelected { get; set; }
    }
}
