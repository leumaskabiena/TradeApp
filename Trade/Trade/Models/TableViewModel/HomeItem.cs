using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trade.Models.TableViewModel
{
    public class HomeItem
    {
        public string id { get; set; }
        public string title { get; set; }
        public string Name { get; set; }
        public double price { get; set; }
        public byte[] src { get; set; }
        public List<ImageSrc> Lstsrc { get; set; }
        public string description { get; set; }
        public bool isSelected { get; set; }
        public bool isBetVisible { get; set; }

    }
}
