using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOD_RPG.Models.Effects
{
    internal class StrengthEffect : TemporaryEffect
    {
        private int strengthBonus;

        public StrengthEffect(int bonus, int duration)
            : base("Strength Potion", $"Strength increased by {bonus}", duration)
        {
            strengthBonus = bonus;
        }

        public override void Apply(Player player)
        {
            player.ModifyAttribute("strength", strengthBonus);
        }

        public override void Remove(Player player)
        {
            player.ModifyAttribute("strength", -strengthBonus);
        }
    }
}
