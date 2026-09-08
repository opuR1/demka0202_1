using pr1.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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
using System.Windows.Threading;

namespace pr1.Pages
{
    /// <summary>
    /// Логика взаимодействия для OrdersList.xaml
    /// </summary>
    public partial class OrdersList : Page
    {
        private DispatcherTimer _timer;
        public OrdersList()
        {
            InitializeComponent();
            LoadData();

            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(30);
            _timer.Tick += Timer_Tick;
            _timer.Start();
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            LoadData();
        }
        private void LoadData()
        {
            using(iv_de1Entities db = iv_de1Entities.GetContext())
            {
                DateTime today = DateTime.Today;
                DateTime tomorrowEnd = today.AddDays(2).AddTicks(-1);

                var allOrders = db.ServiceClient.Include(sc => sc.Services).Include(sc => sc.Clients)
                    .Where(sc => sc.StartDate >= today && sc.StartDate <= tomorrowEnd).OrderBy(sc => sc.StartDate).ToList();

                lbOrders.ItemsSource = allOrders;

            }
        }
    }
}
