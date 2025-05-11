using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOD_RPG.Visitors
{
    internal class StealthDefenseVisitor : IDefenseVisitor
    {
        public int VisitHeavyWeapon(IHeavyWeapon weapon, Player player)
        {
            return player.Strength;
        }

        public int VisitLightWeapon(ILightWeapon weapon, Player player)
        {
            return player.Dexterity;
        }

        public int VisitMagicWeapon(IMagicWeapon weapon, Player player)
        {
            return 0;
        }

        public int VisitNonWeapon(IItem item, Player player)
        {
            return 0;
        }
    }
}
