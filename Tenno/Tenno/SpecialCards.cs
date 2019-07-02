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
        }

        public Abilitys Ability { get; set; }
        /// <summary>
        /// Конструктор карты
        /// </summary>
        /// <param name="power">сила</param>
        /// <param name="ability">способнсть</param>
        /// <param name="im">путь к картинке</param>
        /// <param name="bim"> путь к рубашке</param>
        public SpecialCards(int power, Abilitys ability, string im, string bim): 
            base (power, im, bim)
        {
            SpecialAbility = ability;
            if (SpecialAbility == Abilitys.Power2x) { Shield = power; Power = power * 2; } //нинзя
            if (SpecialAbility == Abilitys.Infinity) { Shield = power + 2; Power = power + 2; }//гейша
        }
    }
}
