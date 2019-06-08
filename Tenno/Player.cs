using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Controls;

using System.Windows.Media.Imaging;


//замена полодения карты!!!!!!!!!!!!!!!!!!!!!!!!!1 Работатет не корректно, нельзя менять только изображение нужно так же менять и содержимое карты

namespace Tenno
{
    public class Player
    {/// <summary>
     ///ОТдельНО ВЧЛЕНИЬ каРТЫ в руке и крты в поле
     /// </summary>
        public string Name { get; set; }
        public int Points { get; set; }
        public List<Card> HandCard { get; set; }
        public List<Card> AvangardCard { get; set; }
        
        public bool Activity { get; set; }

        public Player()
        {
            Name = "Unknown";
        }
        public Player(string Name)
        {
            this.Name = Name;
            Points = 0;
           AvangardCard = new List<Card>();
            this.AvangardCard.Add(new Card(1, "Images/One.png", "Images/TheyOne.png"));
            this.AvangardCard.Add(new Card(2, "Images/Two.png", "Images/TheyTwo.png"));
            this.AvangardCard.Add(new Card(3, "Images/Three.png", "Images/TheyThree.png"));

             HandCard = new List<Card>();
            this.HandCard.Add(new SpecialCards(4, Abilitys.Power2x, "Images/Four.png", "Images/TheyFour.png"));
            this.HandCard.Add(new SpecialCards(5, Abilitys.Protect2Plus, "Images/Five.png", "Images/TheyFive.png"));
            this.HandCard.Add(new SpecialCards(6, Abilitys.Attac2x, "Images/Six.png", "Images/TheySix.png"));
            this.HandCard.Add(new Card(7, "Images/Seven.png", "Images/TheySeven.png"));
            this.HandCard.Add(new Card(8, "Images/Eight.png", "Images/TheyEight.png"));
            this.HandCard.Add(new Card(9, "Images/Nine.png", "Images/TheyNine.png"));
            this.HandCard.Add(new SpecialCards(10, Abilitys.TakeCard, "Images/Ten.png", "Images/TheyTen.png"));
            this.HandCard.Add(new SpecialCards(11, Abilitys.Infinity, "Images/Eleven.png", "Images/TheyEleven.png"));
            Activity = false;
        }
        public string Hint(int col, int row)
        {//ПОЧЕМу у меня не оступна Table&&&&&&&&&&&&&&
            switch (col)
            {
                case 2:
                    //вывести текст подсказки карты из ячейки игровой таблицы
                    break;
                case 3:
                    break;
                case 4:
                    break;
                case 5:
                    break;
                case 6:
                    break;
                case 7:
                    break;
                case 8:
                    break;
                case 9:
                    break;
                default: break;

            }
            return null;
        }
        public static bool operator ==(Player a, Player b)
        {
            return a.Points == b.Points;
        }
        public static bool operator !=(Player a, Player b)
        {
            return a.Points != b.Points;
        }



        public virtual void DoStartCardPosition()
        {
            
        }

        public void SwapPicture(int poz)
        {
            Image im = new Image(); ///черз него заменяем картинку карты и заднюю сторону
            im.Source = AvangardCard[poz].Picture.Source;
            AvangardCard[poz].Picture.Source = AvangardCard[poz].BacPicture.Source;
            AvangardCard[poz].BacPicture.Source = im.Source;
        }

    }
}
