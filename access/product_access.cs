using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using stok_takip.assets;
using System.Windows;

namespace stok_takip.access
{
    internal class product_access
    {
        /// <summary>
        /// buaraya kendi veritabanıozel bilgilerinizi girin.
        /// </summary>
        private readonly string connectionString = "Server=localhost;Database=stok_takip;Uid=;Pwd=;";


        public List<product> product_read(string searchTerm = "")
        {
            List<product> urunListesi = new List<product>();
            string query = @"
                SELECT u.id, u.urun_ad, u.kategori, u.urun_fiyat, u.urun_STT, u.alarm_seviyesi,
                       u.tedarikci AS TedarikciId, COALESCE(t.tedarikci_ad, 'Tedarikçi Yok') AS TedarikciAdi
                FROM urunler AS u LEFT JOIN tedarikciler AS t ON u.tedarikci = t.id";

            if (!string.IsNullOrWhiteSpace(searchTerm)) { query += " WHERE u.urun_ad LIKE @searchTerm"; }
            query += " ORDER BY u.urun_ad;";

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        if (!string.IsNullOrWhiteSpace(searchTerm)) { cmd.Parameters.AddWithValue("@searchTerm", $"%{searchTerm}%"); }
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                product yeniUrun = new product
                                {
                                    Id = reader.GetInt32("id"),
                                    Ad = reader.GetString("urun_ad"),
                                    Kategori = reader.IsDBNull(reader.GetOrdinal("kategori")) ? "" : reader.GetString("kategori"),
                                    Fiyat = reader.IsDBNull(reader.GetOrdinal("urun_fiyat")) ? 0 : Convert.ToInt32(reader.GetDecimal("urun_fiyat")),
                                    Stt = reader.IsDBNull(reader.GetOrdinal("urun_STT")) ? DateTime.MinValue : reader.GetDateTime("urun_STT"),
                                    Alarm = reader.IsDBNull(reader.GetOrdinal("alarm_seviyesi")) ? 0 : reader.GetInt32("alarm_seviyesi"),
                                    Tedarikci = reader.GetString("TedarikciAdi"),
                                    TedarikciId = reader.IsDBNull(reader.GetOrdinal("TedarikciId")) ? 0 : reader.GetInt32("TedarikciId")
                                };
                                urunListesi.Add(yeniUrun);
                            }
                        }
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show("Hata: " + ex.Message); }
            return urunListesi;
        }


        public List<TedarikciModel> GetTedarikciler()
        {
            List<TedarikciModel> tedarikciler = new List<TedarikciModel>();
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString)) 
                {
                    conn.Open();
                    using (MySqlCommand cmd = new MySqlCommand("SELECT id, tedarikci_ad FROM tedarikciler ORDER BY tedarikci_ad", conn))
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                      
                            tedarikciler.Add(new TedarikciModel
                            {
                                Id = reader.GetInt32("id"),
                                Ad = reader.GetString("tedarikci_ad")
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Tedarikçiler yüklenemedi: " + ex.Message);
            }
            return tedarikciler;
        }


        public bool product_add(product yeniUrun)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string urunQuery = "INSERT INTO urunler (urun_ad, kategori, urun_fiyat, urun_STT, alarm_seviyesi, tedarikci) VALUES (@ad, @kategori, @fiyat, @stt, @alarm, @tedarikciId); SELECT LAST_INSERT_ID();";
                    MySqlCommand cmdUrun = new MySqlCommand(urunQuery, conn);
                    cmdUrun.Parameters.AddWithValue("@ad", yeniUrun.Ad);
                    cmdUrun.Parameters.AddWithValue("@kategori", yeniUrun.Kategori);
                    cmdUrun.Parameters.AddWithValue("@fiyat", yeniUrun.Fiyat);
                    cmdUrun.Parameters.AddWithValue("@stt", (object)yeniUrun.Stt ?? DBNull.Value);
                    cmdUrun.Parameters.AddWithValue("@alarm", yeniUrun.Alarm);
                    cmdUrun.Parameters.AddWithValue("@tedarikciId", yeniUrun.TedarikciId == 0 ? (object)DBNull.Value : yeniUrun.TedarikciId);
                    long yeniUrunId = Convert.ToInt64(cmdUrun.ExecuteScalar());

                    MySqlCommand cmdStok = new MySqlCommand("INSERT INTO stok (urun, stok_adet) VALUES (@urunId, @stokAdet);", conn);
                    cmdStok.Parameters.AddWithValue("@urunId", yeniUrunId);
                    cmdStok.Parameters.AddWithValue("@stokAdet", 0);
                    cmdStok.ExecuteNonQuery();
                    return true;
                }
            }
            catch (Exception ex) { MessageBox.Show("Ürün eklenemedi: " + ex.Message); return false; }
        }

        public bool product_update(product guncelUrun)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "UPDATE urunler SET urun_ad=@ad, kategori=@kategori, urun_fiyat=@fiyat, urun_STT=@stt, alarm_seviyesi=@alarm, tedarikci=@tedarikciId WHERE id=@id;";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@id", guncelUrun.Id);
                    cmd.Parameters.AddWithValue("@ad", guncelUrun.Ad);
                    cmd.Parameters.AddWithValue("@kategori", guncelUrun.Kategori);
                    cmd.Parameters.AddWithValue("@fiyat", guncelUrun.Fiyat);
                    cmd.Parameters.AddWithValue("@stt", (object)guncelUrun.Stt ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@alarm", guncelUrun.Alarm);
                    cmd.Parameters.AddWithValue("@tedarikciId", guncelUrun.TedarikciId == 0 ? (object)DBNull.Value : guncelUrun.TedarikciId);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
            catch (Exception ex) { MessageBox.Show("Ürün güncellenemedi: " + ex.Message); return false; }
        }

        public bool product_delete(int urunId)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    MySqlCommand cmdStok = new MySqlCommand("DELETE FROM stok WHERE urun = @id", conn);
                    cmdStok.Parameters.AddWithValue("@id", urunId);
                    cmdStok.ExecuteNonQuery();
                    MySqlCommand cmdUrun = new MySqlCommand("DELETE FROM urunler WHERE id = @id", conn);
                    cmdUrun.Parameters.AddWithValue("@id", urunId);
                    return cmdUrun.ExecuteNonQuery() > 0;
                }
            }
            catch (Exception ex) { MessageBox.Show("Ürün silinemedi: " + ex.Message); return false; }
        }
    }
}
