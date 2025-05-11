using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOD_RPG.Items
{
    internal class Wand : Weapon, IMagicWeapon
    {
        public override string Name => "Wand";
        public override char Symbol => 'W';
        public override string Description => "A wand that casts spells.";
        protected override int GetBaseDamage() => 8;
        public bool IsTwoHanded => false;

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
