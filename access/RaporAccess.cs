using MySql.Data.MySqlClient;
using stok_takip.assets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;


namespace stok_takip.access
{
    public class RaporAccess
    {        /// <summary>
             /// buaraya kendi veritabanıozel bilgilerinizi girin.
             /// </summary>
        private readonly string connectionString = "Server=localhost;Database=stok_takip;Uid=;Pwd=;";


        public RaporModel GetGenelIstatistikler()
        {
            RaporModel rapor = new RaporModel();

            
            string query = @"
                SELECT
                    (SELECT COUNT(*) FROM urunler) AS ToplamUrunCesidi,
                    (SELECT SUM(stok_adet) FROM stok) AS ToplamStokAdedi,
                    (SELECT SUM(s.stok_adet * u.urun_fiyat) FROM stok s JOIN urunler u ON s.urun = u.id) AS ToplamStokDegeri,
                    (SELECT COUNT(*) FROM urunler u JOIN stok s ON u.id = s.urun WHERE s.stok_adet <= u.alarm_seviyesi AND u.alarm_seviyesi > 0) AS AlarmdakiUrunSayisi,
                    (SELECT AVG(urun_fiyat) FROM urunler WHERE urun_fiyat > 0) AS OrtalamaUrunFiyati,
                    (SELECT COUNT(*) FROM urunler WHERE urun_STT BETWEEN CURDATE() AND DATE_ADD(CURDATE(), INTERVAL 30 DAY)) AS SttYaklasanUrunSayisi
                FROM DUAL;"; 

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            rapor.ToplamUrunCesidi = reader.IsDBNull(reader.GetOrdinal("ToplamUrunCesidi")) ? 0 : reader.GetInt32("ToplamUrunCesidi");
                            rapor.ToplamStokAdedi = reader.IsDBNull(reader.GetOrdinal("ToplamStokAdedi")) ? 0 : reader.GetInt64("ToplamStokAdedi");
                            rapor.ToplamStokDegeri = reader.IsDBNull(reader.GetOrdinal("ToplamStokDegeri")) ? 0m : reader.GetDecimal("ToplamStokDegeri");
                            rapor.AlarmdakiUrunSayisi = reader.IsDBNull(reader.GetOrdinal("AlarmdakiUrunSayisi")) ? 0 : reader.GetInt32("AlarmdakiUrunSayisi");
                            rapor.OrtalamaUrunFiyati = reader.IsDBNull(reader.GetOrdinal("OrtalamaUrunFiyati")) ? 0m : reader.GetDecimal("OrtalamaUrunFiyati");
                            rapor.SttYaklasanUrunSayisi = reader.IsDBNull(reader.GetOrdinal("SttYaklasanUrunSayisi")) ? 0 : reader.GetInt32("SttYaklasanUrunSayisi");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("İstatistikler yüklenirken bir hata oluştu: " + ex.Message, "Veritabanı Hatası");
            }
            return rapor;
        }
    }
}