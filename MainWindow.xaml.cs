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
    /// <summary>
    /// MainWindow.xaml etkileşim mantığı
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }


        private void MainFrame_Navigated(object sender, NavigationEventArgs e)
        {

        }

        private void MainContentFrame_Navigated(object sender, NavigationEventArgs e)
        {

        }

        private void MainContentFrame_Navigated_1(object sender, NavigationEventArgs e)
        {

        }

        private void urun_button(object sender, RoutedEventArgs e)
        {
            frame_main.Navigate(new urun_ynt());
        }

        private void AlertsButton_Click(object sender, RoutedEventArgs e)
        {
            frame_main.Navigate(new alarm_page());
        }

        private void RaporButton_Click(object sender, RoutedEventArgs e)
        {
            frame_main.Navigate(new rapor_page());
        }

        private void StockButton_Click(object sender, RoutedEventArgs e)
        {
            frame_main.Navigate(new StokDurumuPage()); 

        }

        private void SuppliersButton_Click(object sender, RoutedEventArgs e)
        {
            frame_main.Navigate(new tedarikci_yönetimi()); 
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
