using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOD_RPG.Decorators
{
    internal class PowerfulDecorator : ItemDecorator, IDamageItems
    {
        private const int DMG_INCREASE = 5;

        public PowerfulDecorator(IItem item) : base(item) { }

        protected override string GetEffectName() => "(Powerful)";
        protected override string GetEffectDescription() => "This item is more powerful than usual.";
        public int Damage
        {
            get
            {
                if (wrappedItem is IDamageItems damageItems)
                {
                    return damageItems.Damage + DMG_INCREASE;
                }
                return DMG_INCREASE;
            }
        }

        public bool IsTwoHanded
        {
            get
            {
                if (wrappedItem is IDamageItems damageDealer)
                {
                    return damageDealer.IsTwoHanded;
                }
                return false;
            }
        }

        public override void ApplyEffectsToPlayer(Player player) { }

        public override void RemoveEffectsFromPlayer(Player player) { }
    }
}
