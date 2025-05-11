using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOD_RPG.Potions
{
    internal class HealthEffect : PermanentEffect
    {
        private int healthBonus;

        public HealthEffect(int bonus)
            : base("Health Elixir", $"Health increased by {bonus}")
        {
            healthBonus = bonus;
        }

        public override void Apply(Player player)
        {
            player.ModifyAttribute("health", healthBonus);
        }

        public override void Remove(Player player)
        {
            // PERMANEMT EFFECT - no need to remove
        }
    }
}
