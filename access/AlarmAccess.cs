using MySql.Data.MySqlClient;
using stok_takip.assets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using stok_takip.assets;

namespace stok_takip.access
{
    public class AlarmAccess
    {
        /// <summary>
        /// buaraya kendi veritabanıozel bilgilerinizi girin.
        /// </summary>
        private readonly string connectionString = "Server=localhost;Database=stok_takip;Uid=;Pwd=;";

        public List<AlarmModel> GetAlarmList()
        {
            List<AlarmModel> alarmListesi = new List<AlarmModel>();

            
            string query = @"
                SELECT 
                    u.id AS UrunId,
                    u.urun_ad AS UrunAdi,
                    s.stok_adet AS MevcutStok,
                    u.alarm_seviyesi AS AlarmSeviyesi,
                    COALESCE(t.tedarikci_ad, 'Tedarikçi Atanmamış') AS TedarikciAdi
                FROM urunler u
                JOIN stok s ON u.id = s.urun
                LEFT JOIN tedarikciler t ON u.tedarikci = t.id
                WHERE 
                    s.stok_adet <= u.alarm_seviyesi AND u.alarm_seviyesi > 0
                ORDER BY 
                    (u.alarm_seviyesi - s.stok_adet) DESC;";

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            alarmListesi.Add(new AlarmModel
                            {
                                UrunId = reader.GetInt32("UrunId"),
                                UrunAdi = reader.GetString("UrunAdi"),
                                MevcutStok = reader.GetInt32("MevcutStok"),
                                AlarmSeviyesi = reader.GetInt32("AlarmSeviyesi"),
                                TedarikciAdi = reader.GetString("TedarikciAdi")
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Alarm listesi yüklenirken bir hata oluştu: " + ex.Message, "Veritabanı Hatası");
            }

            return alarmListesi;
        }
    }
}