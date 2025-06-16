using stok_takip.access;
using stok_takip.assets;
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
namespace stok_takip
{
    public partial class rapor_page : Page
    {
        private readonly RaporAccess dataAccess = new RaporAccess();

        public rapor_page()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LoadRapor();
        }

        private void LoadRapor()
        {
            
            RaporModel rapor = dataAccess.GetGenelIstatistikler();

            
            txtToplamUrunCesidi.Text = rapor.ToplamUrunCesidi.ToString(); 
            txtToplamStokAdedi.Text = rapor.ToplamStokAdedi.ToString();
            txtToplamStokDegeri.Text = rapor.ToplamStokDegeri.ToString();
            txtAlarmdakiUrunSayisi.Text = rapor.AlarmdakiUrunSayisi.ToString();
            txtOrtalamaUrunFiyati.Text = rapor.OrtalamaUrunFiyati.ToString();
            txtSttYaklasanUrunSayisi.Text = rapor.SttYaklasanUrunSayisi.ToString();

           
            if (rapor.ToplamUrunCesidi > 0)
            {
                double alarmOrani = (double)rapor.AlarmdakiUrunSayisi / rapor.ToplamUrunCesidi * 100;
                txtAlarmdakiUrunOrani.Text = $"(Tüm ürünlerin %{alarmOrani:F1}'i)";
            }
            else
            {
                txtAlarmdakiUrunOrani.Text = "(Hiç ürün yok)";
            }
        }
    }
}