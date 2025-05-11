using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOD_RPG.Visitors
{
    // This class implements the visitor pattern for magic attacks 
    internal class MagicAttackVisitor : IAttackVisitor
    {
        public int VisitHeavyWeapon(IHeavyWeapon weapon, Player player)
        {
            // low damage for heavy weapons
            return 1;
        }

        public int VisitLightWeapon(ILightWeapon weapon, Player player)
        {
            // low damage for light weapons
            return 1;
        }

        public int VisitMagicWeapon(IMagicWeapon weapon, Player player)
        {
            // Full damage for magic weapons - works by adding the player wisdom to the weapon base damage 
            int baseDamage = weapon is IDamageItems ? ((IDamageItems)weapon).Damage : 0; // get base damage from the weapon
            return baseDamage + player.Wisdom;
        }

        public int VisitNonWeapon(IItem item, Player player)
        {
            // non weapons do not do damage
            return 0;
        }
    }
}
