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
            StartSwap();
        }

        public override void DoStartCardPosition()
        {

            ///вариант1
            //перемешать элементы листа 
            // сначала 1-3 потом 4-11 и расставить их

            Mixer(AvangardCard);

            Mixer(HandCard);

        }

        public Card[,] TwistCardInHand()
        {
            ///вариант два получать уже с расставленными позициями
            /// выченять тыл и авангард
            /// перемешивать авангард
            /// /// та это уже другая функция мы не можем мешать начальные карты после первого хода только менять
            return null;
        }

        public List<Card> Mixer(List<Card> Table) //перемешивает карты в заданном массиве карт произвольныим образом
        {//НОРМАЛЬНО ЧТО У МЕНЯ ТИп КАРТА?
            List<Card> table = new List<Card>();
            table = Table;
            //ГЛЕБ -МЫ ПЕРЕДАЕМ КОПИЮ ИЛИ ССЫЛКУ?
            Card libero = new Card();
            Random rand = new Random();
            int ii = rand.Next(0, table.Count - 1);//первая карта
            int jj = rand.Next(0, table.Count - 1);//вторая карта
            for (int i = 0; i < table.Count; i++)
            {
                ii = rand.Next(0, table.Count - 1);//первая карта
                libero = table[ii];//Как записывать мои карты в массив с общим абстрактным типом?
                table[ii] = table[jj];
                table[jj] = libero;

                jj = rand.Next(0, table.Count - 1);//вторая карта
                                                   ///нужно ли мне вообще столько классов? 
                                                   ///смена в гриде как с GrID рабоать?

            }
            return table;
        }

        public void MakeAMove(List<Card> playerAvgrd, out int atac, out int shld)//возвращает пару атакующая/атакуемая
        {
            atac = 0;//это потом можно убрать
            shld = 0;


            int myMax = AvangardCard.Max(i => i.Power);//ищем максимальную по силе карту
                                                       //Math.Max(Math.Max(AvangardCard[0].Power, AvangardCard[1].Power), AvangardCard[2].Power);
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

                    //if (myMax == playerAvgrd.Max(i => i.Power))
                    //{
                    //    int poz = AvangardCard.FindIndex(i => i.Power == myMax);
                    //    //ходить на +1% 3
                    //    poz = (poz + 1) % 3;
                    //    shld = poz;
                    //}
                    //atac = myMax; 

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
        }


        public void StartSwap()//ихзначальная иниц иализация
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
    }
}
