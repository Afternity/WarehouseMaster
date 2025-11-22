using System.Windows;
using WarehouseMaster.WPF.Pages;

namespace WarehouseMaster.WPF.Windows
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            MainFrame.Navigate(new OrderPage());
        }

        private void OpenOrderPage_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new OrderPage());
        }

        private void OpenProductPage_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new ProductPage());
        }

        private void OpenSupplierPage_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new SupplierPage());
        }

        private void OpenEmployeePage_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new EmployeePage());
        }

        private void OpenLocationPage_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new LocationPage());
        }

        private void OpenPalletPage_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new PalletPage());
        }

        private void OpenProfileWindow_Click(object sender, RoutedEventArgs e)
        {
            var window = Application.Current.Windows.OfType<ProfileWindow>().FirstOrDefault();

            if (window != null)
            {
                window.Activate();
                return;
            }

            window = new ProfileWindow();
            window.Show();
        }
    }
}
