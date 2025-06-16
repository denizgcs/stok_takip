using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace stok_takip.assets
{
    public class product 
    {
        
        
        private int id;
        private string ad;
        private string kategori;
        private string tedarikci; 
        private int fiyat;
        private DateTime stt;
        private int alarm;
        private int tedarikciId;

      
        public int Id { get => id; set => id = value; }
        public string Ad { get => ad; set => ad = value; }
        public string Kategori { get => kategori; set => kategori = value; }
        public string Tedarikci { get => tedarikci; set => tedarikci = value; }
        public int Fiyat { get => fiyat; set => fiyat = value; }
        public DateTime Stt { get => stt; set => stt = value; }
        public int Alarm { get => alarm; set => alarm = value; }
        public int TedarikciId { get => tedarikciId; set => tedarikciId = value; } 

   
        public product(int id, string ad, string kategori, string tedarikci, int fiyat, DateTime stt, int alarm)
        {
            this.id = id;
            this.ad = ad;
            this.kategori = kategori;
            this.tedarikci = tedarikci;
            this.fiyat = fiyat;
            this.stt = stt;
            this.alarm = alarm;
         
        }

        public product() { } 
    }
}
