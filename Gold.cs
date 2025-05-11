using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOD_RPG
{
    internal class Gold : Item
    {
        public int Amount { get; }

        public override string Name => $"{Amount} Gold";
        public override char Symbol => 'g';
        public override string Description => $"A pile of {Amount} gold.";

        public Gold(int amount)
        {
            Amount = amount;
        }
    }
}
