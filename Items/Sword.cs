using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOD_RPG.Items
{
    internal class Sword : Weapon, ILightWeapon
    {
        public override string Name => "Sword";
        public override char Symbol => 'S';
        public override string Description => "A sharp sword.";
        protected override int GetBaseDamage() => 10;

        public override int AcceptAttack(IAttackVisitor visitor, Player player)
        {
            return visitor.VisitLightWeapon(this, player);
        }

        public override int AcceptDefense(IDefenseVisitor visitor, Player player)
        {
            return visitor.VisitLightWeapon(this, player);
        }
    }
}
