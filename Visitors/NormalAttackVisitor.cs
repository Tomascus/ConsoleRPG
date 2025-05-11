using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOD_RPG.Visitors
{
    internal class NormalAttackVisitor : IAttackVisitor
    {
        public int VisitHeavyWeapon(IHeavyWeapon weapon, Player player)
        {
            // Damage depends on strength and aggression
            int baseDamage = weapon is IDamageItems ? ((IDamageItems)weapon).Damage : 0;
            return baseDamage + (player.Strength / 2) + (player.Aggression / 3); 
        }

        public int VisitLightWeapon(ILightWeapon weapon, Player player)
        {
            // Damage depends on dexterity and luck
            int baseDamage = weapon is IDamageItems ? ((IDamageItems)weapon).Damage : 0;
            return baseDamage + (player.Dexterity / 2) + (player.Luck / 3);
        }

        public int VisitMagicWeapon(IMagicWeapon weapon, Player player)
        {
            // Damage depends on wisdom
            int baseDamage = weapon is IDamageItems ? ((IDamageItems)weapon).Damage : 0;
            return baseDamage + player.Wisdom;
        }

        public int VisitNonWeapon(IItem item, Player player)
        {
            // Non-weapons do 0 damage
            return 0;
        }

    }
}
