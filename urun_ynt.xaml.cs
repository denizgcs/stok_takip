using System;
using System.Collections.Generic;
using System.Globalization;
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
using stok_takip.access;
using stok_takip.assets;

namespace stok_takip
{
    public partial class urun_ynt : Page
    {
        private readonly product_access dataAccess = new product_access();
        private product seciliUrun = null;

        public urun_ynt()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LoadUrunler();
            LoadTedarikciler();
        }

        private void LoadUrunler(string aramaTerimi = "") { dgUrunler.ItemsSource = dataAccess.product_read(aramaTerimi); }
        private void LoadTedarikciler() { cmbTedarikciler.ItemsSource = dataAccess.GetTedarikciler(); }

        private void TemizleForm()
        {
            txtUrunAd.Clear();
            txtKategori.Clear();
            txtFiyat.Clear();
            txtAlarmDuzeyi.Clear();
            dpSTT.SelectedDate = null;
            cmbTedarikciler.SelectedIndex = -1;
            dgUrunler.SelectedItem = null;
            seciliUrun = null;
        }

        private void dgUrunler_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgUrunler.SelectedItem is product selected)
            {
                seciliUrun = selected;
                txtUrunAd.Text = seciliUrun.Ad;
                txtKategori.Text = seciliUrun.Kategori;
                txtFiyat.Text = seciliUrun.Fiyat.ToString();
                txtAlarmDuzeyi.Text = seciliUrun.Alarm.ToString();
                cmbTedarikciler.SelectedValue = seciliUrun.TedarikciId;
                if (seciliUrun.Stt != DateTime.MinValue) { dpSTT.SelectedDate = seciliUrun.Stt; } else { dpSTT.SelectedDate = null; }
            }
        }

        private void btnEkle_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUrunAd.Text)) { MessageBox.Show("Ürün adı boş bırakılamaz.", "Uyarı"); return; }
            product yeniUrun = new product
            {
                Ad = txtUrunAd.Text,
                Kategori = txtKategori.Text,
                Fiyat = int.TryParse(txtFiyat.Text, out int fiyat) ? fiyat : 0,
                Alarm = int.TryParse(txtAlarmDuzeyi.Text, out int alarm) ? alarm : 0,
                Stt = dpSTT.SelectedDate ?? DateTime.MinValue,
                TedarikciId = (int)(cmbTedarikciler.SelectedValue ?? 0)
            };
            if (dataAccess.product_add(yeniUrun))
            {
                MessageBox.Show("Ürün başarıyla eklendi.");
                LoadUrunler();
                TemizleForm();
            }
        }

        private void btnGuncelle_Click(object sender, RoutedEventArgs e)
        {
            if (seciliUrun == null) { MessageBox.Show("Lütfen güncellemek için bir ürün seçin.", "Uyarı"); return; }
            seciliUrun.Ad = txtUrunAd.Text;
            seciliUrun.Kategori = txtKategori.Text;
            seciliUrun.Fiyat = int.TryParse(txtFiyat.Text, out int fiyat) ? fiyat : 0;
            seciliUrun.Alarm = int.TryParse(txtAlarmDuzeyi.Text, out int alarm) ? alarm : 0;
            seciliUrun.Stt = dpSTT.SelectedDate ?? DateTime.MinValue;
            seciliUrun.TedarikciId = (int)(cmbTedarikciler.SelectedValue ?? 0);
            if (dataAccess.product_update(seciliUrun))
            {
                MessageBox.Show("Ürün başarıyla güncellendi.");
                LoadUrunler();
                TemizleForm();
            }
        }

        private void btnSil_Click(object sender, RoutedEventArgs e)
        {
            if (seciliUrun == null) { MessageBox.Show("Lütfen silmek için bir ürün seçin.", "Uyarı"); return; }
            if (MessageBox.Show($"'{seciliUrun.Ad}' adlı ürünü silmek istediğinize emin misiniz?", "Silme Onayı", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                if (dataAccess.product_delete(seciliUrun.Id))
                {
                    MessageBox.Show("Ürün başarıyla silindi.");
                    LoadUrunler();
                    TemizleForm();
                }
            }
        }

        private void btnTemizle_Click(object sender, RoutedEventArgs e) { TemizleForm(); }
        private void btnAra_Click(object sender, RoutedEventArgs e) { LoadUrunler(txtArama.Text); }
    }
}