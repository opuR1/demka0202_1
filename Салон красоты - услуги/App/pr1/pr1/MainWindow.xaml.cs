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
using pr1.Models;
using pr1.Pages;

namespace pr1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string _aPass = "0000";
        public MainWindow()
        {
            InitializeComponent();
            MainFrame.Navigate(new ServiceList());
        }

        private void MainFrame_Navigated(object sender, NavigationEventArgs e)
        {
            if (e.Content is ServiceList)
            {
                btnAdmin.Visibility = Visibility.Visible;
                btnBack.Visibility = Visibility.Collapsed;
                btnClient.Visibility = Visibility.Collapsed;
            }
            else
            {
                btnAdmin.Visibility = Visibility.Collapsed;
                btnBack.Visibility = Visibility.Visible;
                btnClient.Visibility = Visibility.Collapsed;
            }
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.GoBack();
        }

        private void btnAdmin_Click(object sender, RoutedEventArgs e)
        {
            tbPassword.Visibility = Visibility.Visible;
        }

        private void btnClient_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Вы уверены, что хотите выйти из админ панели?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                btnAdmin.Visibility = Visibility.Visible;
                btnClient.Visibility = Visibility.Collapsed;
                tbPassword.Visibility = Visibility.Collapsed;
                if (MainFrame.Content is ServiceList currentServiceList)
                {
                    currentServiceList.delAdmin();
                }
            }
        }

        private void tbPassword_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (tbPassword.Text == _aPass)
            {
                tbPassword.Text = "";
                btnAdmin.Visibility = Visibility.Collapsed;
                btnClient.Visibility = Visibility.Visible;
                tbPassword.Visibility= Visibility.Collapsed;
                if (MainFrame.Content is ServiceList currentServiceList)
                {
                    currentServiceList.getAdmin();
                }
                MessageBox.Show("Вы вошли в панель администратора!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}
