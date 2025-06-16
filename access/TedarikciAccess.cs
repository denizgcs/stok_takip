using MySql.Data.MySqlClient;
using stok_takip.assets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using MySql.Data.MySqlClient;
using stok_takip.assets;
using System;
using System.Collections.Generic;
using System.Windows;

namespace stok_takip.access
{
    public class TedarikciAccess
    {
        private readonly string connectionString = "Server=localhost;Database=stok_takip;Uid=root;Pwd=admin;";


        public List<TedarikciModel> GetSuppliers(string searchTerm = "")
        {
            List<TedarikciModel> suppliers = new List<TedarikciModel>();
            string query = "SELECT id, tedarikci_ad, tedarikci_no, tedarikci_email, tedarikci_adres FROM tedarikciler";
            if (!string.IsNullOrWhiteSpace(searchTerm)) { query += " WHERE tedarikci_ad LIKE @searchTerm"; }
            query += " ORDER BY tedarikci_ad;";
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
                                suppliers.Add(new TedarikciModel
                                {
                                    Id = reader.GetInt32("id"),
                                    Ad = reader.IsDBNull(reader.GetOrdinal("tedarikci_ad")) ? "" : reader.GetString("tedarikci_ad"),
                                    Phone = reader.IsDBNull(reader.GetOrdinal("tedarikci_no")) ? "" : reader.GetString("tedarikci_no"),
                                    Email = reader.IsDBNull(reader.GetOrdinal("tedarikci_email")) ? "" : reader.GetString("tedarikci_email"),
                                    Address = reader.IsDBNull(reader.GetOrdinal("tedarikci_adres")) ? "" : reader.GetString("tedarikci_adres"),
                                    ContactPerson = ""
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show("Tedarikçiler yüklenirken hata: " + ex.Message); }
            return suppliers;
        }

        public bool AddSupplier(TedarikciModel supplier)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "INSERT INTO tedarikciler (tedarikci_ad, tedarikci_no, tedarikci_email, tedarikci_adres) VALUES (@ad, @no, @email, @adres);";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@ad", supplier.Ad);
                    cmd.Parameters.AddWithValue("@no", supplier.Phone);
                    cmd.Parameters.AddWithValue("@email", supplier.Email);
                    cmd.Parameters.AddWithValue("@adres", supplier.Address);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
            catch (Exception ex) { MessageBox.Show("Tedarikçi eklenemedi: " + ex.Message); return false; }
        }

        public bool UpdateSupplier(TedarikciModel supplier)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "UPDATE tedarikciler SET tedarikci_ad=@ad, tedarikci_no=@no, tedarikci_email=@email, tedarikci_adres=@adres WHERE id=@id;";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@id", supplier.Id);
                    cmd.Parameters.AddWithValue("@ad", supplier.Ad);
                    cmd.Parameters.AddWithValue("@no", supplier.Phone);
                    cmd.Parameters.AddWithValue("@email", supplier.Email);
                    cmd.Parameters.AddWithValue("@adres", supplier.Address);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
            catch (Exception ex) { MessageBox.Show("Tedarikçi güncellenemedi: " + ex.Message); return false; }
        }

        public bool DeleteSupplier(int supplierId)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    MySqlCommand updateCmd = new MySqlCommand("UPDATE urunler SET tedarikci = NULL WHERE tedarikci = @id", conn);
                    updateCmd.Parameters.AddWithValue("@id", supplierId);
                    updateCmd.ExecuteNonQuery();

                    MySqlCommand deleteCmd = new MySqlCommand("DELETE FROM tedarikciler WHERE id=@id;", conn);
                    deleteCmd.Parameters.AddWithValue("@id", supplierId);
                    return deleteCmd.ExecuteNonQuery() > 0;
                }
            }
            catch (Exception ex) { MessageBox.Show("Tedarikçi silinemedi. Hata: " + ex.Message); return false; }
        }
    }
}