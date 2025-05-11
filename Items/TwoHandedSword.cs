using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOD_RPG.Items
{
    internal class TwoHandedSword : Weapon, IHeavyWeapon 
    {
        public override string Name => "Two-Handed Sword";
        public override char Symbol => 'T';
        public override string Description => "A massive two-handed sword.";
        protected override int GetBaseDamage() => 20;
        public override bool IsTwoHanded => true;

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
