using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOD_RPG
{
    internal interface IAttackVisitor
    {
        // Visit methods for each weapon type - these methods will be called by the AcceptAttack method in the weapon classes 
        int VisitHeavyWeapon(IHeavyWeapon weapon, Player player);
        int VisitLightWeapon(ILightWeapon weapon, Player player);
        int VisitMagicWeapon(IMagicWeapon weapon, Player player);
        int VisitNonWeapon(IItem item, Player player);
    }
}
