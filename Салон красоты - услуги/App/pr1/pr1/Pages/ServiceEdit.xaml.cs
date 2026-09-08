using pr1.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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

namespace pr1.Pages
{
    /// <summary>
    /// Логика взаимодействия для ServiceEdit.xaml
    /// </summary>
    public partial class ServiceEdit : Page
    {
        private Services _service;
        private bool isNew;
        private iv_de1Entities db = iv_de1Entities.GetContext();
        private List<Services> _deletedServices = new List<Services>();
        private string _sourceFilePath = null;
        public ServiceEdit(Services service)
        {
            InitializeComponent();
            _service = service;
            isNew = _service == null;
            if (isNew)
            {
                _service = new Services();
            }
            else
            {
                LoadData();
            }
        }
        private void LoadData()
        {
            tbName.Text = _service.Name;
            tbCost.Text = _service.Cost.ToString();
            tbDuration.Text = _service.Duration.ToString();
            tbDiscount.Text = _service.Discount.ToString();
            if (!string.IsNullOrEmpty(_service.ImagePath))
            {
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(_service.ImagePath, UriKind.RelativeOrAbsolute);
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.EndInit();
                imgService.Source = bitmap;
            }
        }
        private bool Validate(out decimal Cost, out int Duration, out int Discount)
        {
            string error = "";
            Cost = 0; Duration = 0; Discount = 0;
            if (string.IsNullOrWhiteSpace(tbName.Text)) error += "Введите название услуги.\n";
            if (!decimal.TryParse(tbCost.Text, out Cost) || Cost <= 0) error += "Введите корректную цену > 0.\n";
            if (!int.TryParse(tbDiscount.Text, out Discount) || Discount < 0 || Discount > 100) error += "Скидка должна быть числом от 0 до 100.\n";
            if (!int.TryParse(tbDuration.Text, out Duration) || Duration < 0 || Duration > 240) error += "Длительность не может быть меньше 0 и больше 4 часов.\n";

            if (!string.IsNullOrWhiteSpace(error))
            {
                tblError.Text = error;
                return false;
            }
            tblError.Text = "";
            return true;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Validate(out decimal validCost, out int validDuration, out int validDiscount))
                {
                    _service.Name = tbName.Text;
                    _service.Cost = validCost;
                    _service.Duration = validDuration;
                    _service.Discount = validDiscount;
                    if (!string.IsNullOrEmpty(_sourceFilePath))
                    {
                        string fileName = System.IO.Path.GetFileName(_sourceFilePath);
                        string targetFolder = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images");

                        System.IO.Directory.CreateDirectory(targetFolder);
                        string targetPath = System.IO.Path.Combine(targetFolder, fileName);

                        System.IO.File.Copy(_sourceFilePath, targetPath, true);

                        _service.Image = fileName;
                        _sourceFilePath = null;
                    }

                    if (isNew)
                    {
                        db.Services.Add(_service);
                        MessageBox.Show("Услуга успешно добавлена!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        db.Entry(_service).State = EntityState.Modified;
                        MessageBox.Show("Услуга успешно обновлена!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    }

                    db.SaveChanges();
                    NavigationService.Navigate(new ServiceList());
                }

                
            }
            catch(Exception ex)
            {
                MessageBox.Show($"{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnDel_Click(object sender, RoutedEventArgs e)
        {
            if (isNew)
            {
                NavigationService.GoBack();
                return;
            }

            var result = MessageBox.Show($"Вы уверены, что хотите удалить услугу {_service.Name}?", "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Question);
            
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    var serviceDB = db.Services.FirstOrDefault(s => s.Id == _service.Id);

                    if (serviceDB != null)
                    {
                        db.Services.Remove(serviceDB);
                        db.SaveChanges();
                        MessageBox.Show("Услуга успешно удалена!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("Услуга уже удалена или не найдена в базе данных.", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                    NavigationService.Navigate(new ServiceList());
                }
                catch(Exception ex)
                {
                    MessageBox.Show($"Ошибка при удалении услуги: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void btnimgEdit_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new Microsoft.Win32.OpenFileDialog();

            if (openFileDialog.ShowDialog() == true)
            {
                _sourceFilePath = openFileDialog.FileName;

                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(_sourceFilePath);
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.EndInit();

                imgService.Source = bitmap;
            }
        }
    }
}
