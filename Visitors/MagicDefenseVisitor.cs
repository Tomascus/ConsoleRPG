using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOD_RPG.Visitors
{
    internal class MagicDefenseVisitor : IDefenseVisitor
    {
        public int VisitHeavyWeapon(IHeavyWeapon weapon, Player player)
        {
            return player.Luck;
        }

        public int VisitLightWeapon(ILightWeapon weapon, Player player)
        {
            return player.Luck;
        }

        public int VisitMagicWeapon(IMagicWeapon weapon, Player player)
        {
            // Defense depends on doubled wisdom
            return player.Wisdom * 2;
        }

        public int VisitNonWeapon(IItem item, Player player)
        {
            return player.Luck;
        }
    }
}
