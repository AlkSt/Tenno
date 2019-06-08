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
    /// Логика взаимодействия для Regularions.xaml
    /// </summary>
    public partial class Regularions : Window
    {
        public Regularions()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Texter.Text = @"Нажмите в меню кнопку играть.
В появившемся окне будет происходить игровой процесс
В нижней части расположены ваши катры, в верхней карты соперника.
Игра происходит следующим образом:
    в начале хода вы выбираете одну из своих карт двойным кликом мышкой и затем
нажимаете на одну из трех карт соперника на которую хотите атаковать.
    Если вы побеждаете в бою то получаете столько же очков 
сколько сила поверженой каты.
    Если силы равны то обе карты отправляются в сброс.
    Если же вы проигрываете бой то ваша карта отправляется в сброс, а соперник
получает возможность осмотреть одну из ваших карт авангарда.
";
        }
    }
}
