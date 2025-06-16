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
using stok_takip.access;
using stok_takip.assets;

namespace stok_takip
{
    public partial class tedarikci_yönetimi : UserControl
    {
        private readonly TedarikciAccess dataAccess = new TedarikciAccess();
        private TedarikciModel seciliTedarikci = null;

        public tedarikci_yönetimi()
        {
            InitializeComponent();
            LoadSuppliers();
        }

        private void LoadSuppliers(string searchTerm = "")
        {
            var supplierList = dataAccess.GetSuppliers(searchTerm);
            DgSuppliers.ItemsSource = supplierList;
            StatusSupplierCount.Text = $"Toplam {supplierList.Count} tedarikçi bulundu.";
        }

        private void ClearForm()
        {
            seciliTedarikci = null;
            DgSuppliers.SelectedItem = null;
            TxtFormSupplierName.Clear();
            TxtFormContactPerson.Clear();
            TxtFormPhone.Clear();
            TxtFormEmail.Clear();
            TxtFormAddress.Clear();
            FormTitle.Text = "Yeni Tedarikçi Ekle";
        }

        private void DgSuppliers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DgSuppliers.SelectedItem is TedarikciModel selected)
            {
                seciliTedarikci = selected;
                TxtFormSupplierName.Text = seciliTedarikci.Ad;
                TxtFormContactPerson.Text = seciliTedarikci.ContactPerson;
                TxtFormPhone.Text = seciliTedarikci.Phone;
                TxtFormEmail.Text = seciliTedarikci.Email;
                TxtFormAddress.Text = seciliTedarikci.Address;
                FormTitle.Text = "Tedarikçi Detayları / Düzenle";
            }
        }

        private void BtnSearch_Click(object sender, RoutedEventArgs e) { LoadSuppliers(TxtSearchSupplier.Text); }
        private void BtnNewSupplierMode_Click(object sender, RoutedEventArgs e) { ClearForm(); }
        private void BtnClearForm_Click(object sender, RoutedEventArgs e) { ClearForm(); }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtFormSupplierName.Text)) { MessageBox.Show("Tedarikçi adı boş bırakılamaz.", "Uyarı"); return; }
            bool success;
            if (seciliTedarikci == null) 
            {
                TedarikciModel newSupplier = new TedarikciModel
                {
                    Ad = TxtFormSupplierName.Text,
                    ContactPerson = TxtFormContactPerson.Text,
                    Phone = TxtFormPhone.Text,
                    Email = TxtFormEmail.Text,
                    Address = TxtFormAddress.Text
                };
                success = dataAccess.AddSupplier(newSupplier);
                if (success) MessageBox.Show("Tedarikçi başarıyla eklendi.");
            }
            else 
            {
                seciliTedarikci.Ad = TxtFormSupplierName.Text;
                seciliTedarikci.ContactPerson = TxtFormContactPerson.Text;
                seciliTedarikci.Phone = TxtFormPhone.Text;
                seciliTedarikci.Email = TxtFormEmail.Text;
                seciliTedarikci.Address = TxtFormAddress.Text;
                success = dataAccess.UpdateSupplier(seciliTedarikci);
                if (success) MessageBox.Show("Tedarikçi başarıyla güncellendi.");
            }
            if (success) { LoadSuppliers(); ClearForm(); }
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (seciliTedarikci == null) { MessageBox.Show("Lütfen silmek için bir tedarikçi seçin.", "Uyarı"); return; }
            if (MessageBox.Show($"'{seciliTedarikci.Ad}' adlı tedarikçiyi silmek istediğinize emin misiniz?", "Silme Onayı", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                if (dataAccess.DeleteSupplier(seciliTedarikci.Id))
                {
                    MessageBox.Show("Tedarikçi başarıyla silindi.");
                    LoadSuppliers();
                    ClearForm();
                }
            }
        }
    }
}