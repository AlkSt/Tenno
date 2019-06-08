using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Controls;


using System.Windows.Media.Imaging;
namespace Tenno
{

    class SpecialCards: Card
    {
        public SpecialCards()
        {
           //BacPicture.Source = new BitmapImage(new Uri("Images/TheyBack.png", UriKind.Relative));
           // if (Power == 4) { Shield = 4; Power = 8; }
        }

        public Abilitys Ability { get; set; }
        public SpecialCards(int power, Abilitys ability, string im, string bim): 
            base (power, im, bim)
        {
            SpecialAbility = ability;
            if (SpecialAbility == Abilitys.Power2x) { Shield = power; Power = power * 2; } //нинзя
            if (SpecialAbility == Abilitys.Infinity) { Shield = power + 1; Power = power + 1; }//гейша
        }
    }
}
