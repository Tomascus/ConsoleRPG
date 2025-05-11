using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOD_RPG
{
    // This interface defines the visitor pattern for defense 
    internal interface IDefenseVisitor
    {
        int VisitHeavyWeapon(IHeavyWeapon weapon, Player player);
        int VisitLightWeapon(ILightWeapon weapon, Player player);
        int VisitMagicWeapon(IMagicWeapon weapon, Player player);
        int VisitNonWeapon(IItem item, Player player);
    }
}
