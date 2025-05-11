using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOD_RPG.Items
{
    internal class Axe : Weapon, IHeavyWeapon
    {
        public override string Name => "Axe";
        public override char Symbol => 'A';
        public override string Description => "A heavy axe.";
        protected override int GetBaseDamage() => 12;

        public override int AcceptAttack(IAttackVisitor visitor, Player player)
        {
            return visitor.VisitHeavyWeapon(this, player);
        }

        public override int AcceptDefense(IDefenseVisitor visitor, Player player)
        {
            return visitor.VisitHeavyWeapon(this, player);
        }
    }
}
