using OOD_RPG;
using OOD_RPG.Models.Effects;
using OOD_RPG.Potions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOD_RPG
{
    internal class Player
    {
        // Getters and setters for various attributes
        public int X { get; set; }
        public int Y { get; set; }

        public int Strength { get; private set; }
        public int Dexterity { get; private set; }
        public int Health { get; private set; }
        public int Luck { get; private set; }
        public int Aggression { get; private set; }
        public int Wisdom { get; private set; }

        public int Coins { get; set; }
        public int Gold { get; set; }

        public IItem LeftHand { get; set; }
        public IItem RightHand { get; set; }

        public List<IItem> Inventory { get; private set; }
        public const int InventorySize = 8;

        private EffectManager effectManager; 




        // Constructor
        public Player(int x, int y)
        {
            X = x;
            Y = y;

            // Initialize attributes
            Strength = 10;
            Dexterity = 10;
            Health = 100;
            Luck = 10;
            Aggression = 10;
            Wisdom = 10;

            Coins = 0;
            Gold = 0;

            LeftHand = null;
            RightHand = null;

            Inventory = new List<IItem>(); // Use list to store items
            effectManager = new EffectManager(this); 

        }

        public bool AddToInventory(IItem item)
        {
            if (Inventory.Count >= InventorySize)
            {
                // Inventory is full
                return false;
            }

            Inventory.Add(item);
            return true;
        }

        public IItem RemoveFromInventory(int index)
        {
            if (index >= 0 && index < Inventory.Count)
            {
                IItem item = Inventory[index];
                Inventory.RemoveAt(index);
                return item;
            }

            return null;
        }

        public void ModifyAttribute(string attribute, int value)
        {
            switch (attribute.ToLower())
            {
                case "strength":
                    Strength += value;
                    break;
                case "dexterity":
                    Dexterity += value;
                    break;
                case "health":
                    Health += value;
                    break;
                case "luck":
                    Luck += value;
                    break;
                case "aggression":
                    Aggression += value;
                    break;
                case "wisdom":
                    Wisdom += value;
                    break;
            }
        }

        // Use effect manager to work with effects on player
        public void AddEffect(IEffect effect)
        {
            effectManager.AddEffect(effect);
        }

        public void UpdateEffects()
        {
            effectManager.UpdateEffects();
        }

        public List<IEffect> GetActiveEffects()
        {
            return effectManager.GetActiveEffects();
        }

        // This method is called when the player uses an antidote potion - the effect will be removed after next update
        public void RemoveAllTemporaryEffects()
        {
            List<IEffect> effects = GetActiveEffects();

            foreach (IEffect effect in effects)
            {
                if (effect is TemporaryEffect)
                {
                    effectManager.RemoveEffect(effect);
                }
            }
        }


    }
}
