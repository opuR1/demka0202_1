using pr1.Models;
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

namespace pr1.Pages
{
    /// <summary>
    /// Логика взаимодействия для ServiceList.xaml
    /// </summary>
    public partial class ServiceList : Page
    {
        private bool isAdmin = false;
        private List<Services> _allServices = new List<Services>();
        public ServiceList()
        {
            InitializeComponent();
            LoadData();
        }
        private void LoadData()
        {
            using (var db = iv_de1Entities.GetContext())
            {
                _allServices = db.Services.ToList();
                lbServices.ItemsSource = _allServices;

                cmbFilter.SelectedIndex = 0;
            }

            if(isAdmin == true)
            {
                btnAdd.Visibility = Visibility.Visible;
                btnAddClientService.Visibility = Visibility.Visible;
            }
            else
            {
                btnAdd.Visibility = Visibility.Collapsed;
                btnAddClientService.Visibility = Visibility.Collapsed;
            }

            ApplyFilters();
        }
        public void getAdmin()
        {
            isAdmin = true;
            LoadData();
        }
        public void delAdmin()
        {
            isAdmin = false;
            LoadData();
        }

        private void ApplyFilters()
        {
            if (_allServices == null || _allServices.Count == 0) return;

            var filtered = _allServices.AsEnumerable();

            string searchText = tbSearch.Text.Trim().ToLower();

            if (!string.IsNullOrEmpty(searchText))
            {
                filtered = filtered.Where(s => s.Name.ToLower().Contains(searchText));
            }

            switch(cmbFilter.SelectedIndex)
            {
                case 1:
                    filtered = filtered.Where(s => s.Discount < 5);
                    break;
                case 2:
                    filtered = filtered.Where(s => s.Discount >= 5 && s.Discount < 15);
                    break;
                case 3:
                    filtered = filtered.Where(s => s.Discount >= 15 && s.Discount < 30);
                    break;
                case 4:
                    filtered = filtered.Where(s => s.Discount >= 30 && s.Discount < 70);
                    break;
                case 5:
                    filtered = filtered.Where(s => s.Discount >= 70 && s.Discount < 100);
                    break;
            }

            switch(cmbSort.SelectedIndex)
            {
                case 0:
                    filtered = filtered.OrderBy(s => s.Cost);
                    break;
                case 1:
                    filtered = filtered.OrderByDescending(s => s.Cost);
                    break;
            }

            lbServices.ItemsSource = filtered.ToList();
            txbCount.Text = $"{filtered.Count()} из {_allServices.Count()}";
        }
        private void lbServices_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Services selectedService = lbServices.SelectedItem as Services;
            if (isAdmin)
            {
                
                NavigationService.Navigate(new ServiceEdit(selectedService));
            }
            else
            {
                
                NavigationService.Navigate(new ServiceClients(selectedService));
            }
        }

        private void cmbSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void cmbFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void tbSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ServiceEdit(null));
        }

        private void btnAddClientService_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
