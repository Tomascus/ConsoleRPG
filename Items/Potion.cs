using OOD_RPG.Models.Effects;
using OOD_RPG.Potions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOD_RPG.Items 
{
    internal class Potion : Item
    {
        // Variables that hold values for each type of potion
        private PotionType type;
        private string potionName;
        private string potionDescription;

        public Potion() { }

        // Constructor that takes potion type and sets the name and description based on it
        public Potion(PotionType type)
        {
            this.type = type;
            SetPotionDetails();
        }

        // Properties override the base class to use the instance variables
        public override string Name => potionName;
        public override char Symbol => 'p'; 
        public override string Description => potionDescription;

        // Set potion details based on type
        private void SetPotionDetails()
        {
            switch (type)
            {
                case PotionType.Strength:
                    potionName = "Strength Potion";
                    potionDescription = "Temporarily increases strength by (2) for 5 turns";
                    break;

                case PotionType.Luck:
                    potionName = "Luck Potion";
                    potionDescription = "Temporarily increases luck over 5 turns";
                    break;

                case PotionType.Health:
                    potionName = "Health Elixir";
                    potionDescription = "Permanently increases health by (20)";
                    break;

                case PotionType.Antidote:
                    potionName = "Antidote";
                    potionDescription = "Removes all temporary effects";
                    break;

                default:
                    potionName = "Unknown Potion";
                    potionDescription = "This potion has mysterious effects";
                    break;
            }
        }

        // This uses the interface IEffect to create the effect of the potion 
        public IEffect CreateEffect()
        {
            switch (type)
            {
                case PotionType.Strength:
                    return new StrengthEffect(2, 5); // +2 strength for 5 turns

                case PotionType.Luck:
                    return new LuckEffect(1, 5); // Scales the luck bonus over 5 turns by 1

                case PotionType.Health:
                    return new HealthEffect(20); // +20 permanent health

                case PotionType.Antidote:
                    // This is handled in game class
                    return null;

                default:
                    throw new ArgumentException($"Unknown potion type: {type}"); 
            }
        }

        public PotionType GetPotionType()
        {
            return type;
        }
    }
    public enum PotionType
    {
        Strength,
        Luck,
        Health,
        Antidote
    }

}