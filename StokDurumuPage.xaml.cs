using stok_takip.access;
using stok_takip.assets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace stok_takip
{
    public partial class StokDurumuPage : Page
    {
        private readonly StokAccess dataAccess = new StokAccess();
        private StokDurumuModel seciliUrun = null;

        public StokDurumuPage()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LoadStokDurumu();
        }

        private void LoadStokDurumu(string searchTerm = "")
        {
            DgStokDurumu.ItemsSource = dataAccess.GetStokDurumuListesi(searchTerm);
        }

        private void DgStokDurumu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DgStokDurumu.SelectedItem is StokDurumuModel selected)
            {
                seciliUrun = selected;
                FormPanel.IsEnabled = true;
                txtUrunAdi.Text = seciliUrun.UrunAdi;
                txtMevcutStok.Text = seciliUrun.StokAdedi.ToString();
                txtYeniStokAdedi.Clear();
                txtYeniStokAdedi.Focus(); 
            }
            else
            {
                FormPanel.IsEnabled = false;
                seciliUrun = null;
                txtUrunAdi.Clear();
                txtMevcutStok.Clear();
                txtYeniStokAdedi.Clear();
            }
        }

        private void btnAra_Click(object sender, RoutedEventArgs e)
        {
            LoadStokDurumu(txtArama.Text);
        }

        private void btnGuncelle_Click(object sender, RoutedEventArgs e)
        {
            if (seciliUrun == null)
            {
                MessageBox.Show("Lütfen stok miktarını güncellemek için bir ürün seçin.", "Uyarı");
                return;
            }

            if (int.TryParse(txtYeniStokAdedi.Text, out int yeniAdet))
            {
                if (dataAccess.UpdateStokAdedi(seciliUrun.UrunId, yeniAdet))
                {
                    MessageBox.Show("Stok başarıyla güncellendi.", "Başarılı");
                   
                    int seciliIndex = DgStokDurumu.SelectedIndex; 
                    LoadStokDurumu(txtArama.Text);
                    DgStokDurumu.SelectedIndex = seciliIndex;
                }
            }
            else
            {
                MessageBox.Show("Lütfen geçerli bir stok adedi girin (sadece rakam).", "Geçersiz Giriş");
            }
        }
    }
}