using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOD_RPG.Visitors
{
    internal class StealthAttackVisitor : IAttackVisitor
    {
        public int VisitHeavyWeapon(IHeavyWeapon weapon, Player player)
        {
            // Half damage for heavy weapons
            int baseDamage = weapon is IDamageItems ? ((IDamageItems)weapon).Damage : 0;
            return (baseDamage + (player.Strength / 2) + (player.Aggression / 3)) / 2;
        }

        public int VisitLightWeapon(ILightWeapon weapon, Player player)
        {
            // Double damage for light weapons
            int baseDamage = weapon is IDamageItems ? ((IDamageItems)weapon).Damage : 0;
            return (baseDamage + (player.Dexterity / 2) + (player.Luck / 3)) * 2;
        }

        public int VisitMagicWeapon(IMagicWeapon weapon, Player player)
        {
            return 1;
        }

        public int VisitNonWeapon(IItem item, Player player)
        {
            return 0;
        }
    }
}
