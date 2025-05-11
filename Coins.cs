using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOD_RPG
{
    // Coins class inherits from Item class and implements IItemDecorator interface 
    internal class Coins : Item
    {
        public int Amount { get; }

        // Name, Symbol, and Description properties are overridden
        public override string Name => $"{Amount} Coins";
        public override char Symbol => 'c';
        public override string Description => $"A pile of {Amount} coins.";

        // Constructor 
        public Coins(int amount)
        {
            Amount = amount;
        }
    }
}
