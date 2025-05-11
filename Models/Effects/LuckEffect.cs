using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace OOD_RPG.Models.Effects
{
    internal class LuckEffect : TemporaryEffect
    {
        private int baseBonus;

        public LuckEffect(int baseBonus, int duration)
            : base("Luck Potion", "Increases luck", duration)
        {
            this.baseBonus = baseBonus;
        }

        public override void Apply(Player player)
        {
            int scaledBonus = baseBonus * RemainingTurns; // The effect scales with the number of turns remaining 
            player.ModifyAttribute("luck", scaledBonus);
        }

        public override void Remove(Player player)
        {
            // Calculate and remove the current bonus
            int scaledBonus = baseBonus * RemainingTurns;
            player.ModifyAttribute("luck", -scaledBonus);
        }

        public override string GetStatusText()
        {
            int currentBonus = baseBonus * RemainingTurns;
            return $"{Name}: +{currentBonus} luck ({RemainingTurns} turns remaining)";
        }
    }
}
