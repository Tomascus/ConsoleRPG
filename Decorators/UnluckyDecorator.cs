using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOD_RPG.Decorators
{
    internal class UnluckyDecorator : ItemDecorator, IDamageItems
    {
        private const int LUCK_PENALTY = -5;

        public UnluckyDecorator(IItem item) : base(item) { }

        protected override string GetEffectName() => "(Unlucky)";
        protected override string GetEffectDescription() => "This item brings bad luck.";

        // Implement IDamageDealer interface
        public int Damage
        {
            get
            {
                if (wrappedItem is IDamageItems damageDealer)
                {
                    return damageDealer.Damage;
                }
                return 0;
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

        public override void ApplyEffectsToPlayer(Player player)
        {
            player.ModifyAttribute("luck", LUCK_PENALTY);
        }

        public override void RemoveEffectsFromPlayer(Player player)
        {
            player.ModifyAttribute("luck", -LUCK_PENALTY); 
        }
    }
}
