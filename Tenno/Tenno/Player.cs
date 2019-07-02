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
    {

        public bool Do = false;
        public bool VictoriosAttac = false; // победа в атаке
        public int CountCard = 11; // количесто карт

        public string Name { get; set; } // имя
        public int Points { get; set; } // очки
        public List<Card> HandCard { get; set; }//карты в руке
        public List<Card> AvangardCard { get; set; } // карты в авангарде
        
        public bool Activity { get; set; } // акивность

        public int Buf { get; set; } // усиление защиты
        public Player()
        {
            Name = "Unknown";
        }
        /// <summary>
        /// Констроктор 
        /// Зполняются листы карт в руке и в вангрде
        /// </summary>
        /// <param name="Name">имя</param>
        public Player(string Name)
        {
            this.Name = Name;
            Points = 0;
            Buf = 0;
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
        /// <summary>
        /// Смена кртинок
        /// </summary>
        /// <param name="poz">номер кары в авангарде</param>
        public void SwapPicture(int poz)
        {
            Image im = new Image(); ///черз него заменяем картинку карты и заднюю сторону
            im.Source = AvangardCard[poz].Picture.Source;
            AvangardCard[poz].Picture.Source = AvangardCard[poz].BacPicture.Source;
            AvangardCard[poz].BacPicture.Source = im.Source;
        }

    }
}
