using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOD_RPG
{
    // This interface is used for items in the game - It uses the decorator pattern to apply effects to the player
    // We can wrap any IItem to apply effects
    internal interface IItemDecorator : IItem
    {
        void ApplyEffectsToPlayer(Player player);
        void RemoveEffectsFromPlayer(Player player);
    }
}
