using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace stok_takip.assets
{
    public class RaporModel
    {
        public int ToplamUrunCesidi { get; set; }
        public long ToplamStokAdedi { get; set; } 
        public decimal ToplamStokDegeri { get; set; }
        public int AlarmdakiUrunSayisi { get; set; }
        public decimal OrtalamaUrunFiyati { get; set; }
        public int SttYaklasanUrunSayisi { get; set; }
    }
}