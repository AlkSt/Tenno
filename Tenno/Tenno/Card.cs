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
    public enum Abilitys {NaN,Power2x, Protect2Plus, Attac2x, TakeCard, Infinity }
    public class Card : ICloneable
    {
        public int Power { get; protected set;}
        public int Shield { get; set; }
        public Abilitys SpecialAbility { get; set; }
        public Image Picture { get; set; }
        public  Image BacPicture { get; set; }


        public object Clone() //глубокое копирование 
        {
            //Company company = new Company { Name = this.Work.Name };
            Image picture = new Image { Source = this.Picture.Source };
            Image bacPicture = new Image { Source = this.BacPicture.Source };
            return new Card
            {
                Power = this.Power,
                Shield = this.Shield,
                SpecialAbility = this.SpecialAbility,
                Picture = picture,
                BacPicture = bacPicture

            };
        }


        public Card()
        {

        }
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="power">сила карты</param>
        /// <param name="im">путь к картинке</param>
        /// <param name="bim"> путь к рубашке карты </param>
        public Card(int power, string im, string bim)
        {
            SpecialAbility = Abilitys.NaN;
            Power = power;
            Shield = power;
            BacPicture = new Image();//у обеих карт одинаковые
            BacPicture.Source = new BitmapImage(new Uri(bim, UriKind.Relative));
            Picture = new Image();
            Picture.Source = new BitmapImage(new Uri(im, UriKind.Relative));
        }

      

        public static bool operator >( Card card1, Card card2) => card1.Power > card2.Shield ;
        //больше если силаб ольше щита и меньше если шит меньше силы
        public static bool operator <(Card card1, Card card2) => card1.Shield  < card2.Power;


/// <summary>
/// Информация о карте
/// </summary>
/// <returns> возвращается строка с информацией о карте</returns>
       public string Info()
        {
            switch (SpecialAbility)
            {
                case Abilitys.Attac2x:
                    return "МПосле его атаки есть возможность атакавать другой картой в этот же ход";
                case Abilitys.Infinity:
                    return "Сильнее любой другой, но после использования уходит в сброс";
                case Abilitys.NaN:
                    return "Нет способности";
                case Abilitys.Power2x:
                    return "Удваивание силы карты при атаке";
                case Abilitys.Protect2Plus:
                    return "Может увеличить силу защиты другой атакованной карты на две единицы";
                case Abilitys.TakeCard:
                    return "Не может атковать, но за удачную защиту начисляются очки";
                default:
                    return null;
            }

        }

       
    }
}
