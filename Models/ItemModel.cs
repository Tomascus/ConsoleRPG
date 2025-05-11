using OOD_RPG.Items;
using OOD_RPG.Models.Effects;
using System;

namespace OOD_RPG.Models
{
    /// <summary>
    /// Interface for all items in the game
    /// </summary>
    public interface IItem
    {
        string Name { get; }
        char Symbol { get; }
        string Description { get; }
    }

    /// <summary>
    /// Base class for all items
    /// </summary>
    public class GenericItemModel : IItem
    {
        public virtual string Name { get; set; }
        public virtual char Symbol { get; set; }
        public virtual string Description { get; set; }
    }

    /// <summary>
    /// Base class for all weapons
    /// </summary>
    public class WeaponModel : GenericItemModel, IDamageItems
    {
        public virtual int Damage { get; set; }
        public virtual bool IsTwoHanded { get; set; } = false;

        /// <summary>
        /// Calculate the attack value when used with a specific attack type
        /// </summary>
        public virtual int CalculateAttackValue(AttackType attackType, PlayerModel player)
        {
            // Base implementation - should be overridden by specific weapon types
            return Damage;
        }

        /// <summary>
        /// Calculate defense value when used with a specific attack type
        /// </summary>
        public virtual int CalculateDefenseValue(AttackType attackType, PlayerModel player)
        {
            // Base implementation - should be overridden by specific weapon types
            return 0;
        }
    }

    /// <summary>
    /// Heavy weapon (swords, axes, etc.)
    /// </summary>
    internal class HeavyWeaponModel : WeaponModel
    {
        public override int CalculateAttackValue(AttackType attackType, PlayerModel player)
        {
            switch (attackType)
            {
                case AttackType.Normal:
                    return Damage + (player.Strength / 2) + (player.Aggression / 3);
                case AttackType.Stealth:
                    return (Damage + (player.Strength / 2) + (player.Aggression / 3)) / 2;
                case AttackType.Magic:
                    return 1;
                default:
                    return Damage;
            }
        }

        public override int CalculateDefenseValue(AttackType attackType, PlayerModel player)
        {
            switch (attackType)
            {
                case AttackType.Normal:
                    return player.Strength + player.Luck;
                case AttackType.Stealth:
                    return player.Strength;
                case AttackType.Magic:
                    return player.Luck;
                default:
                    return 0;
            }
        }
    }

    /// <summary>
    /// Light weapon (daggers, rapiers, etc.)
    /// </summary>
    internal class LightWeaponModel : WeaponModel
    {
        public override int CalculateAttackValue(AttackType attackType, PlayerModel player)
        {
            switch (attackType)
            {
                case AttackType.Normal:
                    return Damage + (player.Dexterity / 2) + (player.Luck / 3);
                case AttackType.Stealth:
                    return (Damage + (player.Dexterity / 2) + (player.Luck / 3)) * 2;
                case AttackType.Magic:
                    return 1;
                default:
                    return Damage;
            }
        }

        public override int CalculateDefenseValue(AttackType attackType, PlayerModel player)
        {
            switch (attackType)
            {
                case AttackType.Normal:
                    return player.Dexterity + player.Luck;
                case AttackType.Stealth:
                    return player.Dexterity;
                case AttackType.Magic:
                    return player.Luck;
                default:
                    return 0;
            }
        }
    }

    /// <summary>
    /// Magic weapon (wands, staves, etc.)
    /// </summary>
    internal class MagicWeaponModel : WeaponModel
    {
        public override int CalculateAttackValue(AttackType attackType, PlayerModel player)
        {
            switch (attackType)
            {
                case AttackType.Normal:
                    return Damage + player.Wisdom;
                case AttackType.Stealth:
                    return 1;
                case AttackType.Magic:
                    return Damage + (player.Wisdom * 2);
                default:
                    return Damage;
            }
        }

        public override int CalculateDefenseValue(AttackType attackType, PlayerModel player)
        {
            switch (attackType)
            {
                case AttackType.Normal:
                    return player.Dexterity + player.Luck;
                case AttackType.Stealth:
                    return 0;
                case AttackType.Magic:
                    return player.Wisdom * 2;
                default:
                    return 0;
            }
        }
    }

    /// <summary>
    /// Coins (currency)
    /// </summary>
    public class CoinModel : GenericItemModel
    {
        public int Amount { get; set; }

        public CoinModel(int amount)
        {
            Amount = amount;
            Name = $"{Amount} Coins";
            Symbol = 'c';
            Description = $"A pile of {Amount} coins.";
        }
    }

    /// <summary>
    /// Gold (currency)
    /// </summary>
    public class GoldModel : GenericItemModel
    {
        public int Amount { get; set; }

        public GoldModel(int amount)
        {
            Amount = amount;
            Name = $"{Amount} Gold";
            Symbol = 'g';
            Description = $"A pile of {Amount} gold.";
        }
    }

    /// <summary>
    /// Potion
    /// </summary>
    public class PotionModel : GenericItemModel
    {
        public PotionType PotionType { get; set; }

        public PotionModel(PotionType type)
        {
            PotionType = type;
            SetPotionDetails();
        }

        private void SetPotionDetails()
        {
            switch (PotionType)
            {
                case PotionType.Strength:
                    Name = "Strength Potion";
                    Description = "Temporarily increases strength by (2) for 5 turns";
                    break;
                case PotionType.Luck:
                    Name = "Luck Potion";
                    Description = "Temporarily increases luck over 5 turns";
                    break;
                case PotionType.Health:
                    Name = "Health Elixir";
                    Description = "Permanently increases health by (20)";
                    break;
                case PotionType.Antidote:
                    Name = "Antidote";
                    Description = "Removes all temporary effects";
                    break;
                default:
                    Name = "Unknown Potion";
                    Description = "This potion has mysterious effects";
                    break;
            }

            Symbol = 'p';
        }

        /// <summary>
        /// Create an effect for this potion
        /// </summary>
        internal IEffect CreateEffect()
        {
            switch (PotionType)
            {
                case PotionType.Strength:
                    return new StrengthEffect(2, 5); // +2 strength for 5 turns
                case PotionType.Luck:
                    return new LuckEffect(1, 5); // Scales the luck bonus over 5 turns by 1
                case PotionType.Health:
                    return new HealthEffect(20); // +20 permanent health
                case PotionType.Antidote:
                    // Handled in game logic
                    return null;
                default:
                    throw new ArgumentException($"Unknown potion type: {PotionType}");
            }
        }
    }

    /// <summary>
    /// Interface for damage-dealing items
    /// </summary>
    public interface IDamageItems
    {
        int Damage { get; }
        bool IsTwoHanded { get; }
    }
}