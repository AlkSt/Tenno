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
using System.Windows.Shapes;

namespace Tenno
{
    /// <summary>
    /// Логика взаимодействия для Menu.xaml
    /// </summary>
    public partial class Menu : Window
    {//КАК ЗАКРЫТЬ ВСЕ ОКНА ПРИ ВЫХОДЕ ИЗ ПРИЛОЖенИЯ?
        public Menu()
        {
            InitializeComponent();
        }

        private void RegButton_Click(object sender, RoutedEventArgs e)
        {
            Regularions reg = new Regularions();
            reg.Show();
        }

        private void PlayButton_Click1(object sender, RoutedEventArgs e)
        {
            this.Hide();//anvisual this window becs has't close
            MainWindow window = new MainWindow();
            window.ShowDialog();//create modal window
        }

        private void CloseButton_Click2(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Вы действительно хотите закрыть приложение?", "Закрытие", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                this.Close();
                foreach (Window w in Application.Current.Windows)
                    if (w.Name != "MainWindows")
                        w.Close();
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
           

        }
    }
}
