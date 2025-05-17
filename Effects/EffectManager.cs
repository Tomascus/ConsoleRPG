using OOD_RPG.Potions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOD_RPG.Models.Effects
{
    internal class EffectManager
    {
        private List<IEffect> activeEffects = new List<IEffect>(); // List to hold active effects of player
        private Player player;

        // Constructor to initialize EffectManager with a player
        public EffectManager(Player player)
        {
            this.player = player;
        }

        public void AddEffect(IEffect effect)
        {
            effect.Apply(player);
            activeEffects.Add(effect);
        }

        public void RemoveEffect(IEffect effect) 
        {
            effect.Remove(player); 
            activeEffects.Remove(effect);
        }

        // This method is called each turn to update the effects 
        public void UpdateEffects()
        {
            // We go through the list of active effects in reverse order to remove expired ones
            for (int i = activeEffects.Count - 1; i >= 0; i--)
            {
                // We remove the effect from the player, update it, and check if it is expired
                // Remove affects the player not UI display 
                activeEffects[i].Remove(player);
                activeEffects[i].Update();

                if (activeEffects[i].IsExpired)
                {
                    activeEffects.RemoveAt(i); // This removes the expired effect from the list
                }
                else
                {
                    // If the effect is not expired, we reapply it to the player 
                    activeEffects[i].Apply(player);
                }
            }
        }

        public List<IEffect> GetActiveEffects()
        {
            return new List<IEffect>(activeEffects);
        }
    }
}
