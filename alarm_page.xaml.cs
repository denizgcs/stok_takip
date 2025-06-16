using stok_takip.access;
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
using stok_takip.assets;
using stok_takip.access;

namespace stok_takip
{
    public partial class alarm_page : Page
    {
        private readonly AlarmAccess dataAccess = new AlarmAccess();

        public alarm_page()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            LoadAlarmList();
        }

        private void LoadAlarmList()
        {
           
            var alarmListesi = dataAccess.GetAlarmList();

            
            DgAlarms.ItemsSource = alarmListesi;
        }
    }
}