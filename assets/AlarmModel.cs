using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace stok_takip.assets
{
    public class AlarmModel
    {
        public int UrunId { get; set; }
        public string UrunAdi { get; set; }
        public int MevcutStok { get; set; }
        public int AlarmSeviyesi { get; set; }
        public string TedarikciAdi { get; set; }
        public int StokAcigi => AlarmSeviyesi - MevcutStok;
    }
}