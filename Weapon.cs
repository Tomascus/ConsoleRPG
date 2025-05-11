using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOD_RPG
{
    // Abstract class that provides basic implementation for weapons
    internal abstract class Weapon : Item, IDamageItems
    {
        public virtual int Damage
        {
            get
            {
                return GetBaseDamage();
            }
        }
        protected virtual int GetBaseDamage() => 0;
        public virtual bool IsTwoHanded { get; } = false;

        // Attack and defense methods for the visitor pattern - these methods will be overridden in the concrete weapon classes
        public virtual int AcceptAttack(IAttackVisitor visitor, Player player)
        {
            return 0;
        }

        public virtual int AcceptDefense(IDefenseVisitor visitor, Player player)
        {
            return 0;
        }
    }
}
