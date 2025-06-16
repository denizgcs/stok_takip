using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace stok_takip.assets
{
    public class StokDurumuModel
    {
        public int UrunId { get; set; }
        public string UrunAdi { get; set; }
        public string Kategori { get; set; }
        public int StokAdedi { get; set; }
        public string TedarikciAdi { get; set; } 
    }
}