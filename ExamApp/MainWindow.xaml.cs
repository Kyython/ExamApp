using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace ExamApp
{
    public partial class MainWindow : Window
    {
        private int _count;
        private int[] _numbers;

        public MainWindow()
        {
            InitializeComponent();
        }

        private async Task<string> DownloadFileAsync(string fromAddress, string toAddress)
        {
            SetLoading(true);
            downloadFileButton.IsEnabled = false;

            string exception = "Файл успешно загружен";

            using (var client = new WebClient())
            {
                await Task.Run(() => {try { client.DownloadFile(fromAddress, toAddress); } catch { exception = "Ошибка загрузки"; } });
            }
         
            using (var context = new DataContext())
            {
                context.FileInformations.Add(new FileInformation { FilePath = toAddress, Url = fromAddress  });
                await Task.Run(() => context.SaveChanges());  
            }

            return await Task.FromResult(exception);
        }

        private async void DownloadFileButtonClick(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(urlAddressTextBox.Text) || string.IsNullOrEmpty(downloadPathTextBox.Text))
            {
                MessageBox.Show("Заполните все поля!");
                return;
            }

            var result = await DownloadFileAsync(urlAddressTextBox.Text, downloadPathTextBox.Text);
            SetLoading(false);
            MessageBox.Show(result);
            downloadFileButton.IsEnabled = true;
        }

        private void SetLoading(bool isLoading)
        {
            if (isLoading)
            {
                progressBar.Visibility = Visibility.Visible;
                statusTextBlock.Text = "Пожалуйста подождите...";
            }
            else
            {
                progressBar.Visibility = Visibility.Collapsed;
                statusTextBlock.Text = "Готово";
            }
        }

        private async void EnterButtonClick(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(numberCountTextBox.Text, out _count))
            {
                MessageBox.Show("Введите цифру!");
                return;
            }

            var result = await Task.Run(() => CreateThread());

            MessageBox.Show(result);
        }

        private string CreateThread()
        {
            try
            {
                Thread[] threads = new Thread[_count];
                _numbers = new int[_count];

                for (int i = 0; i < _numbers.Length; i++)
                {
                    threads[i] = new Thread(new ParameterizedThreadStart(AddNumberInArray));
                    threads[i].Start(i);
                }
            }
            catch(Exception exception)
            {
                return $"Потоки завершили свою работу с ошибкой - {exception.Message}";
            }
         
            return "Потоки завершили свою работу успешно";
        } 

        private void AddNumberInArray(object number)
        {
            _numbers[(int)number] = (int)number;
        }
    }
}
