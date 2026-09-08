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
using System.Data.Entity;
using pr1.Models;

namespace pr1.Pages
{
    /// <summary>
    /// Логика взаимодействия для ServiceClients.xaml
    /// </summary>
    public partial class ServiceClients : Page
    {
        private Services _service;
        private iv_de1Entities db = iv_de1Entities.GetContext();

        public ServiceClients(Services service)
        {
            InitializeComponent();
            if (service == null)
            {
                MessageBox.Show("Выберите услугу!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                NavigationService.GoBack();
            }
            _service = service;
            LoadCMB();
            LoadData();

            tbStartTime.TextChanged += UpdateEndTime;
            clDate.SelectedDatesChanged += UpdateEndTime;
        }
        private DateTime? GetStartDateTime()
        {
            if (clDate.SelectedDate == null) return null;

            if (TimeSpan.TryParse(tbStartTime.Text, out TimeSpan startTime))
            {
                return clDate.SelectedDate.Value.Add(startTime);
            }

            return null;
        }
        private void UpdateEndTime(object sender, EventArgs e)
        {
            DateTime? startDateTime = GetStartDateTime();

            if (startDateTime.HasValue)
            {
                DateTime endDateTime = startDateTime.Value.AddMinutes(_service.Duration);

                tblEndTime.Text = endDateTime.ToString("HH:mm dd.MM.yyyy");
            }
            else
            {
                tblEndTime.Text = "Некорректное время старта";
            }
        }
        private void LoadCMB()
        {
            var ClientsList = db.Clients.ToList();
            cmbClients.ItemsSource = ClientsList;
            cmbClients.DisplayMemberPath = "FullName";
            cmbClients.SelectedValuePath = "Id";
        }
        private void LoadData()
        {
            tblName.Text = _service.Name;
            tblCost.Text = $"{_service.Cost.ToString()} руб.";
            tblDur.Text = $"{_service.Duration.ToString()} мин.";
            clDate.SelectedDate = DateTime.Today;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (cmbClients.SelectedValue == null)
            {
                MessageBox.Show("Пожалуйста, выберите клиента из списка!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            DateTime? startDateTime = GetStartDateTime();
            if (!startDateTime.HasValue)
            {
                MessageBox.Show("Введите корректное время начала (например, 15:30)!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                ServiceClient newRecord = new ServiceClient
                {
                    ServiceId = _service.Id,
                    ClientId = (int)cmbClients.SelectedValue,
                    StartDate = startDateTime.Value
                };

                db.ServiceClient.Add(newRecord);
                db.SaveChanges();

                MessageBox.Show("Запись успешно сохранена!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                NavigationService.GoBack();
            }
            catch(Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении данных: {ex.Message}", "Ошибка БД", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
