using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOD_RPG.Items
{
    internal class Dagger : Weapon, ILightWeapon
    {
        public override string Name => "Dagger";
        public override char Symbol => 'D';
        public override string Description => "A small, sharp blade.";
        protected override int GetBaseDamage() => 6;
        public bool IsTwoHanded => false;

        // Override the AcceptAttack and AcceptDefense methods to use the visitor pattern 
        public int AcceptAttack(IAttackVisitor visitor, Player player)
        {
            return visitor.VisitLightWeapon(this, player);
        }

        public int AcceptDefense(IDefenseVisitor visitor, Player player)
        {
            return visitor.VisitLightWeapon(this, player);
        }
    }
}

