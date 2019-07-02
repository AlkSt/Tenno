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
//using System.Threading;

/// <summary>
/// Общая логика игры
/// Перемещение карт на игровом поле
/// /// </summary>

namespace Tenno
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
       
        public bool Shanse = false;     // отвечает за возможжность просмотра карты соперника при удачной атаке
        public bool BeforeFight = true; // показывает что сейчас идет расстановка карт
        public bool doubleatac = false; //возможность двойной атаки
        public int DownColumn = 0, DownRow = 0;       // положение карты в грид
        public Player Guy = new Player("Ты"); // создание объекта ирока пользователя
        public Compuer Comp = new Compuer("Компьютер"); // создание объекта ирока компьютера
        public bool PlayerFallenFight = false; //проиграл игрок
        public int attacing; // номер в  листе КУДА атаковать
        public int defender; // номер в листе КОТОРОГО атаковать
        public string ClearCard = "pack://application:,,,/Tenno;component/Images/Clear2.png"; // пустая карта

        

        public MainWindow()
        {
            InitializeComponent();

        }

        private void MuGrid_Loaded(object sender, RoutedEventArgs e)
        {
            Comp.DoStartCardPosition(); // задание начальной позиции карт компьютера
            Guy.DoStartCardPosition(); // задание позиции карт игрока (стандартная растановка)
            RefreshPosition(); // обновление позиций на поле

        }

        /// <summary>
        /// Закрывает игру если игрок в диалоговом окне нажимает окей
        /// </summary>
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
           
            if (MessageBox.Show("Выйти из игры?", "Закрытие", MessageBoxButton.YesNo, MessageBoxImage.Question ) == MessageBoxResult.No)
                e.Cancel = true;

        }

        private void MainWindows_Closed(object sender, EventArgs e)//после закрытия основной формы открыть меню
        {
            Menu menu = new Menu();
            menu.Show();
        }
        /// <summary>
        /// Вычисляет на ячейку в какой колонне или столбце пользователь нажал 
        /// </summary>
        private void MainWindows_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //определеяем положение мыши в Grid
            var element = (UIElement)e.Source;
            DownColumn = Grid.GetColumn(element);
            DownRow = Grid.GetRow(element);
        }

        /// <summary>
        /// Событие нажатия на кливишу "В бой!" 
        /// Генерируется кто ходит первым 
        /// Вызывается первый ход компьютера
        /// </summary>
        private void StartButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            BeforeFight = false; // бой начат
            StartButton.Visibility = Visibility.Collapsed;//убираем кнопку
            //выбрать кто будет ходить епервым
            Inform.Visibility = Visibility.Visible;
            Random rand = new Random();
            if (rand.Next()%2 ==5)//== 0)
            {
                MessageBox.Show(" Рандом решил, что первым ходит компьютер");
                //Inform.Text = "Рандом решил, что первым \nходит компьютер";
                
                Comp.Activity = true;
                Comp.MakeAMove(Guy.AvangardCard, out attacing, out defender); //прошу компьютер сказать чем и куда он ходит[1-3]
                CompCan(); //сравнивает зачения карт расположенных в полученных до этого ячейках
            }
            else
            {
                MessageBox.Show("Рандом решил, что первым ходите Вы");
               // Inform.Text = "Рандом решил, что первым \nходте Вы ";
                Guy.Activity = true;
            }
        }
        /// <summary>
        /// Если перемещение карт возможно то меняются местами их позиции
        /// </summary>
        private void MainWindows_MouseUp(object sender, MouseButtonEventArgs e) 
        {

            ///Варианты когда игде мы можем перемещать карты
            ///менять могу только свои!
            ///1 - до боя если они в одних и тех же строках
            /// 2 - во время боя в своей руке
            /// 3 - если карту захватили или она ушла в сброс тогда перемещени между рукой и авангардом ! 1 карты
            /// (3 событие должна выдаваться подсказка и оно вызывается в другом методе)
            var element = (UIElement)e.Source;
            int UpColumn = Grid.GetColumn(element);
            int UpRow = Grid.GetRow(element);
            if (attacing == UpColumn / 2 - 1 && DownRow == 4 && Guy.VictoriosAttac) { SwapBord(UpColumn/2-1, UpRow); Guy.VictoriosAttac = false; NextMove(); }
            if (UpRow > 3 && DownRow > 3)//я в своей половине поля
            {
                if (BeforeFight && DownRow == UpRow) // нет боя и я меняю в одной строке
                {
                    //передвинуь в соответствующие клетки элементы
                    if (DownRow == 4) //авангад i/2+1
                    {
                        Card newcard = new Card();
                        newcard = Guy.AvangardCard[UpColumn / 2 - 1];
                        Guy.AvangardCard[UpColumn / 2 - 1] = Guy.AvangardCard[DownColumn / 2 - 1];
                        Guy.AvangardCard[DownColumn / 2 - 1] = newcard;
                    }
                    else //рука i-2
                    {
                        Card newcard = new Card();
                        newcard = Guy.HandCard[UpColumn - 2];
                        Guy.HandCard[UpColumn - 2] = Guy.HandCard[DownColumn - 2];
                        Guy.HandCard[DownColumn - 2] = newcard;
                    }
                    RefreshPosition();
                }
                if (!(BeforeFight) && DownRow == 6 && UpRow == 6)//во время боя в руке
                {
                    //Передвинуть соответствующие элементы
                    Card newcard = new Card();
                    newcard = Guy.HandCard[UpColumn - 2];
                    Guy.HandCard[UpColumn - 2] = Guy.HandCard[DownColumn - 2];
                    Guy.HandCard[DownColumn - 2] = newcard;

                    RefreshPosition();
                }
                if (DownRow != UpRow && (PlayerFallenFight || Guy.VictoriosAttac)) // достать карту из руки
                {

                    if (PlayerFallenFight && Guy.AvangardCard[UpColumn / 2 - 1].Picture.Source.ToString() == ClearCard)
                    {//Менять только на то место которое пустое !!!
                        Card newcard = new Card();
                        newcard = Guy.HandCard[DownColumn - 2];
                        Guy.HandCard[DownColumn - 2] = Guy.AvangardCard[UpColumn / 2 - 1];
                        Guy.AvangardCard[UpColumn / 2 - 1] = newcard;
                        //сменить карту в авангарде
                        
                        RefreshPosition();
                        NextMove();
                    }
                    if (Guy.VictoriosAttac && UpColumn / 2 - 1 == attacing)
                    {//смена той карты которая атаковала
                        Guy.VictoriosAttac = false;
                        Card newcard = new Card();
                        newcard = Guy.HandCard[DownColumn - 2];
                        Guy.HandCard[DownColumn - 2] = Guy.AvangardCard[UpColumn / 2 - 1];
                        Guy.AvangardCard[UpColumn / 2 - 1] = newcard;
                        //сменить карту в авангарде
                        SwapBord(UpColumn/2-1, UpRow);
                        RefreshPosition();
                        NextMove();
                    }
                }
            }

        }


        /// <summary>
        ///  Обновляет значения ячеек игрока в Grid
        ///  высчитывает сколько карт осталось у пользователя
        /// </summary>
        public void RefreshPosition() 
        {

            OneHand.Source = Guy.AvangardCard[0].Picture.Source;
            TwoHand.Source = Guy.AvangardCard[1].Picture.Source;
            ThreeHand.Source = Guy.AvangardCard[2].Picture.Source;
            FourHand.Source = Guy.HandCard[0].Picture.Source;
            FiveHand.Source = Guy.HandCard[1].Picture.Source;
            SixHand.Source = Guy.HandCard[2].Picture.Source;
            SevenHand.Source = Guy.HandCard[3].Picture.Source;
            EightHand.Source = Guy.HandCard[4].Picture.Source;
            NineHand.Source = Guy.HandCard[5].Picture.Source;
            TenHand.Source = Guy.HandCard[6].Picture.Source;
            ElevHand.Source = Guy.HandCard[7].Picture.Source;
            PlayerTB.Text = Guy.Points.ToString();
            Guy.CountCard = Guy.AvangardCard.Count(i => i.Picture.Source.ToString() != ClearCard) +
                Guy.HandCard.Count(i => i.Picture.Source.ToString() != ClearCard);

        }
        /// <summary>
        ///  обновить значение ячеек компьютера в Grid
        ///  Высчтывает сколько карт осталось у компьютера 
        /// </summary>
        public void CompRefreshPosition()
        {
            OneComp.Source = Comp.AvangardCard[0].Picture.Source;
            TwoComp.Source = Comp.AvangardCard[1].Picture.Source;
            ThreeComp.Source = Comp.AvangardCard[2].Picture.Source;
            FourComp.Source = Comp.HandCard[0].Picture.Source;
            FiveComp.Source = Comp.HandCard[1].Picture.Source;
            SixComp.Source = Comp.HandCard[2].Picture.Source;
            SevenComp.Source = Comp.HandCard[3].Picture.Source;
            EightComp.Source = Comp.HandCard[4].Picture.Source;
            NineComp.Source = Comp.HandCard[5].Picture.Source;
            TenComp.Source = Comp.HandCard[6].Picture.Source;
            ElevenComp.Source = Comp.HandCard[7].Picture.Source;
            CompTB.Text = Comp.Points.ToString();
           Comp.CountCard = Comp.AvangardCard.Count(i => i.Picture.Source.ToString() != ClearCard) +
                Comp.HandCard.Count(i => i.Picture.Source.ToString() != ClearCard);

        }
        /// <summary>
        /// 
        /// </summary>
        public void CompCan()
        {
            //поменяем картинки на картах обозначив что куда атакуется
            //сначала перевернем карту компьютера
            Comp.SwapPicture(attacing);
            //теперь обозначим куда будем атаковать мсменйо катинки
            Guy.SwapPicture(defender);

            CompRefreshPosition();//Смена позиций
            RefreshPosition();
            CompareCard();//карты сравниваются
            NextMove(); // следующий ход
            
        }
        /// <summary>
        /// Сравниваются значениякарт после хода
        /// Игрокау предлагается использовать активные способности карт 
        /// </summary>
        public void CompareCard()
        {
            Player Assail = new Player();
            Player Defender = new Player();

            if (Guy.Activity) //в зависимости от того кто атакует сопируем ссылки на массивы
            {
                Assail = Guy;
                Defender = Comp;
            }
            else
            {
                Assail = Comp;
                Defender = Guy;
            }
            //проверяем на способнсть карту 5
            MakeFiveSpecil(Defender.AvangardCard);
            //проверяем на исользование второй атаки карту 6
            if (Assail.AvangardCard[attacing].SpecialAbility == Abilitys.Attac2x && Comp.CountCard > 2 && Guy.CountCard > 2)
                doubleatac = true;
            //сравнивам
            if (Defender.AvangardCard[defender].Shield + Defender.Buf < Assail.AvangardCard[attacing].Power) //бой выигран
            {
                if (Assail.AvangardCard[attacing].Power > 10) // если это гейша
                    Assail.AvangardCard[attacing].Picture.Source = new BitmapImage(new Uri("Images/Clear2.png", UriKind.Relative));
                else
                    //перевернуть карту компа обратно
                    Assail.SwapPicture(attacing); // возвращаем  в нормальное состояняие
                Defender.AvangardCard[defender].Picture.Source = new BitmapImage(new Uri("Images/Clear2.png", UriKind.Relative));// убираем проигравшую
              
               MessageBox.Show($"{Assail.Name} в этом бою победил");
               
                CompRefreshPosition();
                RefreshPosition();

                Assail.Points += Defender.AvangardCard[defender].Shield;
                //добавить очки победителю
                //если ходит комп то 
                if (Comp.Activity)
                { 
                    PlayerFallenFight = true;//человек потерял карту

                    if (Assail.AvangardCard[attacing].Power > 10)//если это гейша
                        Comp.GetNewCard();
                    Comp.TwistCardIntoHand(attacing);//просто сменить карту
                    Comp.TwistCardIntoHand(attacing);//комп может сменить карту
                    CompRefreshPosition();
                }
                //Иначеходил человек
                else
                {
                    Comp.GetNewCard();//комп потерял карту
                    Guy.VictoriosAttac = true;//человек может сменить карту
                    if (!doubleatac && Assail.AvangardCard[attacing].Power < 10)//если имеет место
                        MessageBox.Show("Вы можете сменить карту Если вы не хотите этого делать то нажмите на карту еще раз");
                        
                }
                                  
                //убрать карту соперника с поля
                CompRefreshPosition();
                RefreshPosition();
            }
            else if (Defender.AvangardCard[defender].Shield + Defender.Buf > Assail.AvangardCard[attacing].Power) //бой проигран
            {
                // Comp.GetnewpictureInAvangsrd;
                //убрать картиинку у комапьютера
                Assail.AvangardCard[attacing].Picture.Source = new BitmapImage(new Uri("Images/Clear2.png", UriKind.Relative));  // убираем проигравшую
                if (Guy.Activity) { SwapBord(attacing, 4); PlayerFallenFight = true; }
                if (Defender.AvangardCard[defender].Power > 10)//защищалась гейша
                    Defender.AvangardCard[defender].Picture.Source = new BitmapImage(new Uri("Images/Clear2.png", UriKind.Relative));//карта уходит
                else//карта не уходит
                {
                    if (Defender.AvangardCard[defender].Power > 9)//защищался монах
                 Defender.Points += Assail.AvangardCard[attacing].Shield;
               
                    Defender.SwapPicture(defender);//возвращаем карту в небоевое состояние
                }

                MessageBox.Show($"{Defender.Name} отзащищался");
                CompRefreshPosition();

                RefreshPosition();
                //ходит комп - у человека шанс и комп потерял карту
                if (Comp.Activity)
                {
                    Comp.GetNewCard();
                    //MessageBox.Show("Вы можете посмотрет карту соперника");
                    Shanse = true;

                }
                else
                {
                    if (Defender.AvangardCard[defender].Power > 10)
                        Comp.GetNewCard();
                    //SwapBord(DownColumn/2-1, attacing);
                }
                if (-1 != Guy.AvangardCard.FindIndex(j => j.Picture.Source.ToString() == ClearCard))
                    Inform.Text = $"Вставьте три карты из руки";
                CompRefreshPosition();
                RefreshPosition();
            }
            else // ничья
            {
                Assail.AvangardCard[attacing].Picture.Source = new BitmapImage(new Uri("Images/Clear2.png", UriKind.Relative)); // обнуляем
                Defender.AvangardCard[defender].Picture.Source = new BitmapImage(new Uri("Images/Clear2.png", UriKind.Relative));//обнуляем

                MessageBox.Show("Силы равны карты уходят в сброс, выставьте новую карту из руки");
                //обе карты авангарда надо менять
                CompRefreshPosition();
                RefreshPosition();


                if (Guy.Activity) SwapBord(Guy.AvangardCard.FindIndex(j => j.Picture.Source.ToString() == ClearCard), 4);
                PlayerFallenFight = true;

                Comp.GetNewCard();
                CompRefreshPosition();
                RefreshPosition();

            }
            if (doubleatac)//есть возможость торой атаки
            {
                doubleatac = false;
                MakeSixSpecil();
            }
            else
            {
                Guy.Activity = !Guy.Activity; //смена активнсти карт
                Comp.Activity = !Comp.Activity;
                OneHand.IsEnabled = true;//вернуть активность карты которой не мгла ходить второй раз
                TwoHand.IsEnabled = true;
                ThreeHand.IsEnabled = true;
                
            }
            if (Comp.Buf != 0) //обнуляем использование усиление в защите
                { Comp.Buf = 0; Comp.SwapPicture(Comp.AvangardCard.FindIndex(i => i.SpecialAbility == Abilitys.Protect2Plus)); CompRefreshPosition(); }
                if (Guy.Buf != 0)
                { Guy.Buf = 0; Guy.SwapPicture(Guy.AvangardCard.FindIndex(i => i.SpecialAbility == Abilitys.Protect2Plus)); RefreshPosition(); }


        }

        /// <summary>
        /// Действие карты двойного хода
        /// </summary>
        public void MakeSixSpecil()
        {
            if (Comp.CountCard > 2 && Guy.CountCard > 2)//елси игра к этому моменту не кончится
            {
                if (Guy.Activity)//если ходит человек

                {
                    if (MessageBoxResult.No == MessageBox.Show("Вы хотите сходить еще раз?",
                        "Способность", MessageBoxButton.YesNo, MessageBoxImage.Question))
                    {
                        Guy.Activity = !Guy.Activity;
                        Comp.Activity = !Comp.Activity;
                    }
                    else
                    {//не могу ходить той же картой

                        switch (attacing)
                        {
                            case 0:
                                OneHand.IsEnabled = false;
                                break;
                            case 1:
                                TwoHand.IsEnabled = false;
                                break;
                            case 2:
                                ThreeHand.IsEnabled = false;
                                break;
                            default:
                                break;
                        }
                        RefreshPosition();
                    }


                }
                else//второй ход делает компьютер
                {
                    MessageBox.Show("Компютер ходит еще раз");
                    if (-1 != Guy.AvangardCard.FindIndex(j => j.Picture.Source.ToString() == ClearCard))
                        Inform.Text = $"Вставьте три карты из руки";
                    attacing = (attacing + 1) % 3;
                    //CompareCard();

                }

            }
        }
        /// <summary>
        /// Действие карты защиты
        /// </summary>
        /// <param name="Avangard"> массив игрока который защтщается </param>
        public void MakeFiveSpecil( List<Card> Avangard)
        {
            int sp = Avangard.FindIndex(i => i.SpecialAbility == Abilitys.Protect2Plus);
            if (sp!=-1  && sp!=defender ) //есть шестерка и нападают не на нее
            {
                if(Guy.Activity)//комп защищается и проигрывает
                {
                    if(Guy.AvangardCard[attacing].Power- Comp.AvangardCard[defender].Shield>-1 &&
                       Guy.AvangardCard[attacing].Power - Comp.AvangardCard[defender].Shield <3 )
                    {
                        Comp.SwapPicture(sp);//переворачиваем карту защиты
                        Comp.Buf = 2;
                        CompRefreshPosition();
                        //MessageBox.Show("Использована способность карты защиты");
                    }
                }
                else
                {//защищается человек
                    if (MessageBoxResult.Yes == MessageBox.Show("Вы хотите использовать способность карты защиты?",
                   "Способность", MessageBoxButton.YesNo, MessageBoxImage.Question))
                    {
                        Guy.SwapPicture(sp);
                        Guy.Buf = 2;
                        RefreshPosition();

                    }
                }
            }
        }

/// <summary>
/// Вызов следующего хода компьютера 
/// информирование игрока о его действиях для подолжени игры
/// </summary>
        public void NextMove()
        {
            if (Comp.CountCard > 2 && Guy.CountCard > 2)//Проверка на кончание игры
            {//атакует комп и человек выставил все карты
                if (Comp.Activity && -1 == Guy.AvangardCard.FindIndex(j => j.Picture.Source.ToString() == ClearCard) && !Guy.VictoriosAttac)
                {
                    Inform.Text = $"";
                    MessageBox.Show($" Ходит {Comp.Name}");
                    Comp.MakeAMove(Guy.AvangardCard, out attacing, out defender); //прошу компьютер сказать чем и куда он ходит[1-3]
                    CompCan();
                }
                else
                {
                    if (Guy.Activity)
                    {
                        //MessageBox.Show($" {Guy.Name} ходишь");
                        Inform.Text = $"  {Guy.Name} ходишь";

                        if(-1 != Guy.AvangardCard.FindIndex(j => j.Picture.Source.ToString() == ClearCard))
                             Inform.Text = $"Выставьте три карты из руки";
                    }
                }
               
            }
            else
            {
                

                MessageBoxResult res;
                if (Comp.Points > Guy.Points)
                    res = MessageBox.Show($" {Comp.Name} выиграл бой, игра окончана");
                else if (Comp.Points < Guy.Points) res =MessageBox.Show($" {Guy.Name}  выиграл бой,игра окончана");
                else  res = MessageBox.Show($"Ничья ,игра окончана");
                Comp.Activity = false;
                Guy.Activity = false;
                if (res == MessageBoxResult.OK)
                    Close();
            }
        }
        /// <summary>
        /// Выбор карты которой будет атаковать игрок
        /// </summary>
        private void MainWindows_MouseDoubleClick(object sender, MouseButtonEventArgs e)
         {
            if (DownRow == 4 && Guy.Activity && Guy.AvangardCard[DownColumn / 2 - 1].Picture.Source.ToString() != ClearCard) // в своей половине выбрал
            {
                if (Guy.AvangardCard[DownColumn / 2 - 1].SpecialAbility == Abilitys.TakeCard) //карта 10
                    MessageBox.Show("Вы не можете атаковать этой картой из-за ее способности");
                else
                {
                    if (Guy.Do && attacing != DownColumn / 2 - 1) //если игрок уже выбрал и нажимает сейчас на другую карту
                    {
                        Guy.SwapPicture(attacing);
                        Guy.Do = !Guy.Do;
                        SwapBord(attacing, DownRow);
                    }
                    attacing = DownColumn / 2 - 1;
                    Guy.Do = !Guy.Do; //выбрал или второй раз по той же карте
                    Guy.SwapPicture(attacing);
                    SwapBord(DownColumn/2-1, DownRow);
                    RefreshPosition();
                }
               
            }

        }
        /// <summary>
        /// Перекращивание контуров грида вокруг карт если карта была активирована 
        /// Необходмо для смены карты после атаки
        /// </summary>
        /// <param name="col"> столбец в котором находится карта </param>
        /// <param name="row"> строка в которой находится карта </param>
        private void SwapBord(int col , int row)
        {
            if (row == 4)
            switch (col)
            {
                    
                case 0:
                    if (OneBord.BorderBrush.ToString() == "#FFFFC0CB")
                        OneBord.BorderBrush = new SolidColorBrush(Colors.Red);
                    else OneBord.BorderBrush = new SolidColorBrush(Colors.Pink);
                    break;
                case 1:
                    if (TwoBord.BorderBrush.ToString() == "#FFFFC0CB")
                        TwoBord.BorderBrush = new SolidColorBrush(Colors.Red);
                    else TwoBord.BorderBrush = new SolidColorBrush(Colors.Pink);
                    break;
                case 2:
                    if (ThreeBord.BorderBrush.ToString() == "#FFFFC0CB")
                        ThreeBord.BorderBrush = new SolidColorBrush(Colors.Red);
                    else ThreeBord.BorderBrush = new SolidColorBrush(Colors.Pink);
                    break;
                default: break;
            }
        }

        /// <summary>
        /// Событие происходит при нажатии на крту аванграда соперника игроком
        /// вычисление карты которую будет атаковать игрок
        /// </summary>
        private void AttacComp_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

            int Column = Grid.GetColumn((UIElement)e.Source);
            if (Guy.Do)
            {
                Guy.Do = !Guy.Do;
                defender = Column/2-1;
                Comp.SwapPicture(defender);
                CompRefreshPosition();
                CompareCard();
                NextMove();

            }
        }

        
        /// <summary>
        /// Просмотр карты соперника визуалиация
        /// </summary>
        /// <param name="Column"> столбец грид в котором находится карта</param>
        public void Leave( int Column)
        {
            Comp.SwapPicture(Column / 2 - 1);
            CompRefreshPosition();
            Shanse = false;
        }
        /// <summary>
        /// При наведении мышуи на карту авангарда которую хочет посомтреть игрок
        /// Вычисляется положение этой карты и дается возможность ее посмотреть
        /// </summary>
        private void OnePiece_MouseEnter(object sender, MouseEventArgs e)
        {
            if (Shanse)
            {
                var element = (UIElement)e.Source;
                int Column = Grid.GetColumn(element);

                Comp.SwapPicture(Column / 2 - 1);
                CompRefreshPosition();
                Leave(Column);
            }
        }

    }
}
