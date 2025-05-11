using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOD_RPG.Items
{
    internal class Staff : Weapon, IMagicWeapon
    {
        public override string Name => "Staff";
        public override char Symbol => 'S';
        public override string Description => "A magical staff.";
        protected override int GetBaseDamage() => 12;
        public bool IsTwoHanded => true;

        public override int AcceptAttack(IAttackVisitor visitor, Player player)
        {
            return visitor.VisitMagicWeapon(this, player);
        }

        public override int AcceptDefense(IDefenseVisitor visitor, Player player)
        {
            return visitor.VisitMagicWeapon(this, player);
        }
    }
}
