using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Media;
using System.Windows.Controls;

using System.Windows.Media.Imaging;

using System.Windows;

using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;

using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Tenno
{
    public class Compuer : Player
    {
        string ClearCard = "pack://application:,,,/Tenno;component/Images/Clear2.png"; // пустая карта
        /// <summary>
        /// Конструктор
        /// Заполняет имеющиеся карты
        /// </summary>
        /// <param name="name"> компьютер или игрок </param>
        public Compuer(string name) : base(name)
        {
            string s = "Images/TheyBack.png";
            AvangardCard = new List<Card>();
            this.AvangardCard.Add(new Card(1, s, "Images/TheyOne.png"));
            this.AvangardCard.Add(new Card(2, s, "Images/TheyTwo.png"));
            this.AvangardCard.Add(new Card(3, s, "Images/TheyThree.png"));

            HandCard = new List<Card>();
            this.HandCard.Add(new SpecialCards(4, Abilitys.Power2x, s, "Images/TheyFour.png"));
            this.HandCard.Add(new SpecialCards(5, Abilitys.Protect2Plus, s, "Images/TheyFive.png"));
            this.HandCard.Add(new SpecialCards(6, Abilitys.Attac2x, s, "Images/TheySix.png"));
            this.HandCard.Add(new Card(7, s, "Images/TheySeven.png"));
            this.HandCard.Add(new Card(8, s, "Images/TheyEight.png"));
            this.HandCard.Add(new Card(9, s, "Images/TheyNine.png"));
            this.HandCard.Add(new SpecialCards(10, Abilitys.TakeCard, s, "Images/TheyTen.png"));
            this.HandCard.Add(new SpecialCards(11, Abilitys.Infinity, s, "Images/TheyEleven.png"));
            //StartSwap();
        }
        /// <summary>
        /// Расстановка стартовых позиций карт
        /// </summary>
        public override void DoStartCardPosition()
        {
            Mixer(AvangardCard);// перемешивание карт в массиве авангарда

            Mixer(HandCard);// перемешивание карт в массиве руки

        }
        /// <summary>
        /// Смена карты после ее выхода из игры
        /// </summary>
        /// <param name="used">номер карты которую меняем</param>
        public void TwistCardIntoHand( int used)
        {
            
            
            if (HandCard.FindIndex(j => j.Picture.Source.ToString() != ClearCard) != -1 &&
                AvangardCard[used].SpecialAbility != Abilitys.Infinity)//найдется карту которую нужно поменять (не гейша)
            {
                int pos = 0;
                int k = HandCard.FindIndex(i => i.Shield > AvangardCard[used].Shield && i.Picture.Source.ToString() != ClearCard); //не пустая карта по сильнее
                if (k != -1) pos = k;
                else pos = HandCard.FindIndex(i => i.Picture.Source.ToString() != ClearCard);

                Card swap = new Card();
                swap = (Card)AvangardCard[used].Clone();
                AvangardCard[used] = (Card)HandCard[pos].Clone();
                HandCard[pos] = (Card)swap.Clone();
            }
        }
        /// <summary>
        /// //перемешивает карты в заданном массиве карт произвольныим образом
        /// </summary>
        /// <param name="Table"> таблца карт  компьютеа</param>
        /// <returns> таблица с измененными позициями карт </returns>
        public List<Card> Mixer(List<Card> Table) 
        {
            List<Card> table = new List<Card>();
            table = Table;

            Card libero = new Card();
            Random rand = new Random();
            int ii = rand.Next(0, table.Count - 1);//первая карта
            int jj = rand.Next(0, table.Count - 1);//вторая карта
            for (int i = 0; i < table.Count; i++)
            {
                ii = rand.Next(0, table.Count - 1);//первая карта
                libero = table[ii];
                table[ii] = table[jj];
                table[jj] = libero;

                jj = rand.Next(0, table.Count - 1);//вторая карта
                                                  

            }
            return table;
        }
        /// <summary>
        /// Выбирает какой картой в каую будет идти атака
        /// </summary>
        /// <param name="playerAvgrd"> карты авангарда соперника</param>
        /// <param name="atac">атакующая карта</param>
        /// <param name="shld">атакуемася карта</param>
        public void MakeAMove(List<Card> playerAvgrd, out int atac, out int shld)
        {
            atac = 0;//это потом можно убрать
            shld = 0;


            int myMax = AvangardCard.Max(i => i.Power);//ищем максимальную по силе карту
                                                       
            int heMax = playerAvgrd.Max(i => i.Shield);//смотрим защиту соперника
            int heMin = playerAvgrd.Min(i => i.Shield); //смотрим по минимальным 
            int myMin = AvangardCard.Min(i => i.Power);
            try
            {
                if (heMax == 11)
                { // с наименьшими потерями убрать ее в сброс
                    atac = atac = AvangardCard.FindIndex(i => i.Power == myMin);
                    shld = atac = AvangardCard.FindIndex(i => i.Shield == heMax);
                }
                else
                    if (myMax < 10)//если могу ходить максимальной картой 
                {
                    if (myMax > heMax) // если моя карта больше максимальной  карты человека
                    {
                        //перевернуть карту которой я хожу и обозначить карту на которую я хожу
                        //int poz = AvangardCard.FindIndex(i => i.Power == myMax);
                        ///знаю индекс знаю что в авангарде, значит координаты grid = [poz*2+1,4]
                        atac = AvangardCard.FindIndex(i => i.Power == myMax); // моя атакующая карта максимальная
                        shld = playerAvgrd.FindIndex(i => i.Shield == heMax);

                    }
                    else //если максимальные равны или максимальная человека больше
                    {

                        if (myMin > heMin) //если моя минимальная больше минимальной человека
                        {
                            atac = AvangardCard.FindIndex(i => i.Power == myMin); // моя атакующая карта минимальная
                            shld = playerAvgrd.FindIndex(i => i.Shield == heMin);
                        }
                        else // будем атаковать максимальной какую то среднюю карту  
                        {
                            atac = AvangardCard.FindIndex(i => i.Power == myMax); // моя атакующая карта максимальная
                            shld = playerAvgrd.FindIndex(i => i.Shield < myMax && i.Shield > myMin);
                            if (AvangardCard[atac].Power <= playerAvgrd[shld].Shield)//если проиграем атакуем минимальной
                                atac = AvangardCard.FindIndex(i => i.Power == myMin);
                        }
                    }


                }

                else // я не могу ходить своей максимальной
                {
                    if (myMax == 11 && heMax > 7) // забираю карту побольше
                    {
                        atac = AvangardCard.FindIndex(i => i.Power == myMax);
                        shld = playerAvgrd.FindIndex(i => i.Shield == heMax);
                    }

                }

            }
            catch
            {
                Random re = new Random();
                atac = re.Next(2);
                shld = re.Next(1);
            }

            if (AvangardCard[atac].Power ==10 )//что бы ни как не сходить монахом
                atac = (atac + 1) % 3;
              

            if (playerAvgrd[shld].Power ==10)//что бы не делать суицида
                shld = (shld + 1) % 3;

        }

        /// <summary>
        /// Переворот карт нужно для тестирвания
        /// </summary>
        public void StartSwap()
        {
            foreach (var card in AvangardCard)
            {
                card.Picture.Source = card.BacPicture.Source;
                card.BacPicture.Source = new BitmapImage(new Uri("Images/TheyBack.png", UriKind.Relative));
            }
            foreach (var card in HandCard)
            {
                card.Picture.Source = card.BacPicture.Source;
                card.BacPicture.Source = new BitmapImage(new Uri("Images/TheyBack.png", UriKind.Relative));
            }
        }

        /// <summary>
        /// Выбор и перестановка новой карты в место утраченой
        /// </summary>
        public void GetNewCard()
        {
            
            if (HandCard.FindIndex(j => j.Picture.Source.ToString() != ClearCard) != -1)//найдется карту которую нужно поменять
            {
                Random ran = new Random();
                int card = ran.Next(HandCard.Count);//на которую меняем

                int i = AvangardCard.FindIndex(j => j.Picture.Source.ToString() == ClearCard);//Находим карту авангарда которую нужно поменять

                if (HandCard[card].Picture.Source.ToString() == ClearCard)//если случайно выбраная карта не подходит
                {
                    int k = HandCard.FindLastIndex(j => j.Picture.Source.ToString() != ClearCard); //мерем точно подходящую
                    //меняем на точно подходящую
                    card = k;
                }
                    //меняем 
                    AvangardCard[i] = (Card)HandCard[card].Clone();
                    HandCard[card].Picture.Source = new BitmapImage(new Uri("Images/Clear2.png", UriKind.Relative));
                
            }
        }

    }
}
