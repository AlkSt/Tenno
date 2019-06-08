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


namespace Tenno
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //  СОЗДАТЬ ЛИСТ КАРТИНОК НА КОТОРЫЕ БУДЕТ МЕНЯТЬСЯ ПРИ АТАКЕ ЛИБО ДОБАВИТЬ ЭТОТ ЖЕ ЛИСТ В САМУ КАРТУ!!!!
        public bool PlayerDo = false;
             
        public bool BeforeFight = true;
        public int DownColumn = 0, DownRow = 0;       
        public static string StrName = "Ты";
        public Player Guy = new Player(StrName);
        public Compuer Comp = new Compuer("Компьютер");
        public bool PlayerFallenFight = false; //проиграл игрок
        public int attacing; // номер в  листе КУДА атаковать
        public int defender; // номер в листе КОТОРОГО атаковать
        public string ClearCard = "pack://application:,,,/Tenno;component/Images/Clear2.png";
        public int CountCompCard = 11;
        public int CountPlayerCard = 11;

        public MainWindow()
        {
            InitializeComponent();

        }

        private void MuGrid_Loaded(object sender, RoutedEventArgs e)
        {

            #region //заполнение картинками
            //SimpleCards OneCard = new SimpleCards(1, Abilitys.NaN, "Images/onr.jpg");
            //OneCard.Picture.Source = new BitmapImage(new Uri("Images/onr.jpg", UriKind.Relative));

            //for (int i = 2; i < 9; i++)
            //{
            //Image OneCard = new Image();
            //OneCard.Source = new BitmapImage(new Uri("Images/onr.jpg", UriKind.Relative));
            //MuGrid.Children.Add(OneCard);
            //Grid.SetRow(OneCard, 0);
            //Grid.SetColumn(OneCard, i);}
            #endregion


            Comp.DoStartCardPosition();
            Guy.DoStartCardPosition();
            RefreshPosition();
            // тту должна быть таблица?

        }

        //private void OneHand_MouseEnter(object sender, MouseEventArgs e)
        //{
        //    //нет смысла разворачивать свои карты, нужна найти способ менять карты местами

        //    if (OneHand.Source.ToString() == "pack://application:,,,/Tenno;component/Images/One.png")// указывать полный путь

        //        OneHand.Source = new BitmapImage(new Uri("Images/Back.png", UriKind.Relative));
        //    else OneHand.Source = new BitmapImage(new Uri("Images/One.png", UriKind.Relative));
        //}

        //private void ImageThey_MouseEnter(object sender, MouseEventArgs e)
        //{
        //    ///если (удачная защита) тогда 
        //    ///если карта (находится не в руке)
        //    ///тогда можно ее посмотреть
        //    ///иначе сообщение - вы не можете смотреть карты соперника
        //}

        //private void FourComp_MouseEnter(object sender, MouseEventArgs e)
        //{
        //    int t = MuGrid.Children.IndexOf(FourComp);

        //    if (FourComp.ActualHeight < 54)
        //        if (FourComp.Source.ToString() == "pack://application:,,,/Tenno;component/Images/TheyBack.png")// указывать полный путь

        //            FourComp.Source = new BitmapImage(new Uri("Images/TheyFour.png", UriKind.Relative));

        //}

        //private void FourComp_MouseLeave(object sender, MouseEventArgs e)
        //{

        //    FourComp.Source = new BitmapImage(new Uri("Images/TheyBack.png", UriKind.Relative));
        //}

        //private void OneComp_MouseEnter(object sender, MouseEventArgs e)
        //{
        //    //OneComp.PointFromScreen.
        //    //if (OneComp.ActualHeight < 54)
        //    //    if (OneComp.Source.ToString() == "pack://application:,,,/Tenno;component/Images/TheyBack.png")// указывать полный путь

        //    //        OneComp.Source = new BitmapImage(new Uri("Images/TheyFour.png", UriKind.Relative

        //    //if (PlayerShanse)
        //    //{
               
        //    //    Comp.SwapPicture(defender);
        //    //    CompRefreshPosition();
        //    //    //
        //    //}

        //}

        //private void OneComp_MouseLeave(object sender, MouseEventArgs e)
        //{
        //    if (1==0)
        //    OneComp.Source = new BitmapImage(new Uri("Images/TheyBack.png", UriKind.Relative));

        //}

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)//закрыть игру?
        {
            MessageBoxResult result = MessageBox.Show("Вы действительно хотите выйти из игры?", "Закрытие", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.No)
                e.Cancel = true;

        }

        private void MainWindows_Closed(object sender, EventArgs e)//после закрытия основной формы открыть меню
        {
            Menu menu = new Menu();
            menu.Show();
        }

        private void MainWindows_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //определеяем положение мыши в Grid
            var element = (UIElement)e.Source;
            DownColumn = Grid.GetColumn(element);
            DownRow = Grid.GetRow(element);

            //MessageBox.Show(Column.ToString());
        }


        private void StartButton_MouseDown(object sender, MouseButtonEventArgs e)//игнорируется левая клавиша
        {
            BeforeFight = false; // бой начат
            StartButton.Visibility = Visibility.Collapsed;//убираем кнопку
            //выбрать кто будет ходить епервым
            Random rand = new Random();
            if (rand.Next()%2 == 0)
            {
                MessageBox.Show(" Рандом решил, что первым ходит компьютер");
                Comp.Activity = true;
                Comp.MakeAMove(Guy.AvangardCard, out attacing, out defender); //прошу компьютер сказать чем и куда он ходит[1-3]
                CompCan(); //сравнивает зачения карт расположенных в полученных до этого ячейках
            }
            else { MessageBox.Show("Рандом решил, что первым ходте Вы"); Guy.Activity = true; }
        }

        private void MainWindows_MouseUp(object sender, MouseButtonEventArgs e) //здесь прописаны изменения полжений карт из Grid
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

            //НУЖНО ЛИ СОЗДАВАТЬ CLONE();?????????????
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
                if (DownRow != UpRow && PlayerFallenFight) // && бой проигран
                {//Менять только на то место которое пустое !!!

                    if (Guy.AvangardCard[UpColumn / 2 - 1].Picture.Source.ToString() == "pack://application:,,,/Tenno;component/Images/Clear2.png") {
                        Card newcard = new Card();
                        newcard = Guy.HandCard[DownColumn - 2];
                        Guy.HandCard[DownColumn - 2] = Guy.AvangardCard[UpColumn / 2 - 1];
                        Guy.AvangardCard[UpColumn / 2 - 1] = newcard;
                        //сменить карту в авангарде
                        RefreshPosition();
                        NextMove();
                    }
                }
            }

        }



        public void RefreshPosition() // Обновляет значения ячеек игрока в Grid
        {//OneHand.Source = null; //если нулить не саму карту а ресурс из которого он берется то карта исчезнет без багов
            #region
            //MuGrid.Children.Insert(14, Guy.AvangardCard[0].Picture);
            //MuGrid.Children.Insert(15, Guy.AvangardCard[1].Picture);
            //MuGrid.Children.Insert(16, Guy.AvangardCard[2].Picture);

            //MuGrid.Children.Add(Guy.AvangardCard[0].Picture);
            //Grid.SetRow(Guy.AvangardCard[0].Picture, 2);
            //Grid.SetColumn(Guy.AvangardCard[0].Picture, 3);
            #endregion
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
            CountPlayerCard = Guy.AvangardCard.Count(i => i.Picture.Source.ToString() != "pack://application:,,,/Tenno;component/Images/Clear2.png") +
                Guy.HandCard.Count(i => i.Picture.Source.ToString() != "pack://application:,,,/Tenno;component/Images/Clear2.png");

        }

        public void CompRefreshPosition()// обновить значение ячеек компьютера в Grid
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
            CountCompCard = Comp.AvangardCard.Count(i => i.Picture.Source.ToString() != "pack://application:,,,/Tenno;component/Images/Clear2.png") +
                Comp.HandCard.Count(i => i.Picture.Source.ToString() != "pack://application:,,,/Tenno;component/Images/Clear2.png");

        }

        public void CompCan()
        {
            //поменяем картинки на картах обозначив что куда атакуется
            //сначала перевернем карту компьютера
            Comp.SwapPicture(attacing);

            //теперь обозначим куда будем атаковать мсменйо катинки
            Guy.SwapPicture(defender);

            CompRefreshPosition();
            RefreshPosition();
            CompareCard();
            NextMove();
            
        }

        public void CompareCard()// Атакует компьютер
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


            //сравнивам

            if (Defender.AvangardCard[defender] < Assail.AvangardCard[attacing]) //бой выигран
            {
                if (Assail.AvangardCard[attacing].Power > 10) // если это гейша
                    Assail.AvangardCard[attacing].Picture.Source = new BitmapImage(new Uri("Images/Clear2.png", UriKind.Relative));
                //перевернуть карту компа обратно
                Assail.SwapPicture(attacing); // возвращаем  в нормальное состояняие
                Defender.AvangardCard[defender].Picture.Source = new BitmapImage(new Uri("Images/Clear2.png", UriKind.Relative));// убираем проигравшую
                MessageBox.Show($"{Assail.Name} победил");
                CompRefreshPosition();
                RefreshPosition();

                Assail.Points += Defender.AvangardCard[defender].Shield;
                //добавить очки победителю
                //если ходит комп то человек потерял карту
                if (Comp.Activity) PlayerFallenFight = true;
                //Иначе комп потерял карту и ее надо поставить
                else GetNewCard();
                if (Assail.AvangardCard[attacing].Power > 10)//если это гейша
                    GetNewCard();
                //убрать карту соперника с поля
                CompRefreshPosition();
                RefreshPosition();
            }
            else if (Defender.AvangardCard[defender] > Assail.AvangardCard[attacing]) //бой проигран
            {
                // Comp.GetnewpictureInAvangsrd;
                //убрать картиинку у комапьютера
                Assail.AvangardCard[attacing].Picture.Source = new BitmapImage(new Uri("Images/Clear2.png", UriKind.Relative));  // убираем проигравшую
                if (Defender.AvangardCard[defender].Power > 10)//защищалась гейша
                    Defender.AvangardCard[defender].Picture.Source = new BitmapImage(new Uri("Images/Clear2.png", UriKind.Relative));
                else
                    if (Defender.AvangardCard[defender].Power > 9)//защищался монах
                    Defender.Points += Assail.AvangardCard[attacing].Shield;
                else
                    Defender.SwapPicture(defender);//возвращаем карту в небоевое состояние
                MessageBox.Show($"{Defender.Name} победил");
                CompRefreshPosition();
                RefreshPosition();
                //ходит комп - у человека шанс и комп потерял карту
                if (Comp.Activity)
                {
                    GetNewCard();
                    OnePiece();
                }

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

                PlayerFallenFight = true;
                GetNewCard();
                CompRefreshPosition();
                RefreshPosition();

            }
            if (Assail.AvangardCard[attacing].SpecialAbility == Abilitys.Attac2x)
            {
                // if (Guy.Activity)
                // { MessageBox.Show("Some Message", "DEBUG", MessageBoxButton.YesNo, MessageBoxImage.Question);
                //     if ()}
                //else { }
            }
            else
            {
                Guy.Activity = !Guy.Activity;
                Comp.Activity = !Comp.Activity;
            }
            //смена активнсти карт



        }

        public void NextMove()
        {
            if (CountCompCard > 2 && CountPlayerCard > 2)
            {
                if (Comp.Activity && -1==Guy.AvangardCard.FindIndex (j => j.Picture.Source.ToString() == "pack://application:,,,/Tenno;component/Images/Clear2.png") )
                {
                    MessageBox.Show($" Ходит {Comp.Name}");
                    Comp.MakeAMove(Guy.AvangardCard, out attacing, out defender); //прошу компьютер сказать чем и куда он ходит[1-3]
                    CompCan();
                }
               
            }
            else
            {
                if (Comp.Points > Guy.Points)
                    MessageBox.Show($" Победил {Comp.Name}");
                else MessageBox.Show($" Победил {Guy.Name}");
            }
        }

        private void MainWindows_MouseDoubleClick(object sender, MouseButtonEventArgs e)
         {
            if (DownRow == 4  && Guy.Activity)
            {
                if (Guy.AvangardCard[DownColumn / 2 - 1].SpecialAbility == Abilitys.TakeCard)
                    MessageBox.Show("Вы не можете атаковать этой картой из-за ее способности");
                else
                {
                    attacing = DownColumn / 2 - 1;
                    PlayerDo = !PlayerDo; //выбрал или второй раз по той же карте
                    Guy.SwapPicture(attacing);
                    RefreshPosition();
                }
               
            }

        }

        private void OneComp_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if(PlayerDo)
            {
                PlayerDo = !PlayerDo;
                defender = 0;
                Comp.SwapPicture(defender);
                CompRefreshPosition();
                CompareCard();
                NextMove();
                //вызвать программу подсчитывающую кто победил !!! (мб преобразвать то чт оуже есть)
            }
        }

        private void TwoComp_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (PlayerDo)
            {
                PlayerDo = !PlayerDo;
                defender = 1;
                Comp.SwapPicture(defender);
                CompRefreshPosition();
                CompareCard();
                NextMove();
                //вызвать программу подсчитывающую кто победил !!! (мб преобразвать то чт оуже есть)
            }

        }

        private void ThreeComp_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (PlayerDo)
            {
                PlayerDo = !PlayerDo;
                defender = 2;
                Comp.SwapPicture(defender);
                CompRefreshPosition();
                CompareCard();
                NextMove();
                //вызвать программу подсчитывающую кто победил !!! (мб преобразвать то чт оуже есть)

            }
            }

        public void OnePiece()//передавать сюда параметр от компа, какую карту хочет посмотреть переводить ее на сек в активнй режим
            //для игрока этот парпаметр 0  тогда находим координаты мыши?

        {
            ///тут показывается одна из катрт авангарда на которую наведена мышка
            /// как это реализовать?
            /// можеть быть отправлять в move где будут вычисляться координаты 
            ///  или свойстав маусэнтер у первых трех карт сослать сюда... ?
            ///  пока что незнаю
            ///  но надо не забывать переворачивать карту обратно по 
            ///  mausleave и делать так только один раз
           
        }

        /// <summary>
        ///       если удалось поменять на рандомнуюто меняем на рандомную или же на самую сильную
        ///       
        /// перенести это в класс компьютер!*?
        /// </summary>
        public void GetNewCard()
        {
            Random ran = new Random();
            int card = ran.Next(Comp.HandCard.Count);
      
            int i = Comp.AvangardCard.FindIndex(j => j.Picture.Source.ToString() == "pack://application:,,,/Tenno;component/Images/Clear2.png");

            if (Comp.HandCard[card].Picture.Source.ToString() == "pack://application:,,,/Tenno;component/Images/Clear2.png")
            {
                int k = Comp.HandCard.FindLastIndex(j => j.Picture.Source.ToString() != "pack://application:,,,/Tenno;component/Images/Clear2.png");
                //будет ли нормально работать при приравнивании карты к карте?
                Comp.AvangardCard[i] = (Card)Comp.HandCard[k].Clone();
                Comp.HandCard[k].Picture.Source = new BitmapImage(new Uri("Images/Clear2.png", UriKind.Relative));
            }
            else
            {
                Comp.AvangardCard[i] = (Card)Comp.HandCard[card].Clone();
                Comp.HandCard[card].Picture.Source = new BitmapImage(new Uri("Images/Clear2.png", UriKind.Relative));
            }

        }

    }
}
