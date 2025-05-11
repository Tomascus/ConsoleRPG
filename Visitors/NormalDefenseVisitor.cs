using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOD_RPG.Visitors
{
    internal class NormalDefenseVisitor : IDefenseVisitor
    {
        public int VisitHeavyWeapon(IHeavyWeapon weapon, Player player)
        {
            // Defense depends on strength and luck
            return player.Strength + player.Luck;
        }

        public int VisitLightWeapon(ILightWeapon weapon, Player player)
        {
            return player.Dexterity + player.Luck;
        }

        public int VisitMagicWeapon(IMagicWeapon weapon, Player player)
        {
            return player.Dexterity + player.Luck;
        }

        public int VisitNonWeapon(IItem item, Player player)
        {
            return player.Dexterity;
        }
    }
}
