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
    public class StokAccess
    {
        /// <summary>
        /// buaraya kendi veritabanıozel bilgilerinizi girin.
        /// </summary>
        private readonly string connectionString = "Server=localhost;Database=stok_takip;Uid=;Pwd=;";

        public List<StokDurumuModel> GetStokDurumuListesi(string searchTerm = "")
        {
            List<StokDurumuModel> stokListesi = new List<StokDurumuModel>();
           
            string query = @"
                SELECT 
                    u.id, u.urun_ad, u.kategori, s.stok_adet,
                    COALESCE(t.tedarikci_ad, 'Tedarikçi Yok') AS TedarikciAdi
                FROM urunler u
                LEFT JOIN stok s ON u.id = s.urun
                LEFT JOIN tedarikciler t ON u.tedarikci = t.id";

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query += " WHERE u.urun_ad LIKE @searchTerm OR u.kategori LIKE @searchTerm";
            }
            query += " ORDER BY u.urun_ad;";

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        if (!string.IsNullOrWhiteSpace(searchTerm))
                        {
                            cmd.Parameters.AddWithValue("@searchTerm", $"%{searchTerm}%");
                        }
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                stokListesi.Add(new StokDurumuModel
                                {
                                    UrunId = reader.GetInt32("id"),
                                    UrunAdi = reader.GetString("urun_ad"),
                                    Kategori = reader.IsDBNull(reader.GetOrdinal("kategori")) ? "" : reader.GetString("kategori"),
                                    StokAdedi = reader.IsDBNull(reader.GetOrdinal("stok_adet")) ? 0 : reader.GetInt32("stok_adet"),
                                    TedarikciAdi = reader.GetString("TedarikciAdi")
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show("Stok durumu yüklenirken bir hata oluştu: " + ex.Message); }
            return stokListesi;
        }

        public bool UpdateStokAdedi(int urunId, int yeniAdet)
        {
            if (yeniAdet < 0)
            {
                MessageBox.Show("Stok adedi 0'dan küçük olamaz.", "Hata");
                return false;
            }
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "UPDATE stok SET stok_adet = @yeniAdet WHERE urun = @urunId;";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@yeniAdet", yeniAdet);
                    cmd.Parameters.AddWithValue("@urunId", urunId);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Stok adedi güncellenirken bir hata oluştu: " + ex.Message);
                return false;
            }
        }
    }
}