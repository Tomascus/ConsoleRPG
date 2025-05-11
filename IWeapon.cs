using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOD_RPG
{
    internal interface IWeapon : IItem
    {
        // The visitor pattern allows us to separate the logic of the attack from the items
        // Accept method for the attack visitor - applies when the weapon is used for attack
        int AcceptAttack(IAttackVisitor visitor, Player player);

        // Accept method for the defense visitor - applies when the weapon is used for defense
        int AcceptDefense(IDefenseVisitor visitor, Player player);
    }

    // Specific weapon type interfaces - these interfaces are used to define the specific types of weapons and are empty because they are used only for type checking
    internal interface IHeavyWeapon : IWeapon { }
    internal interface ILightWeapon : IWeapon { }
    internal interface IMagicWeapon : IWeapon { }
}
