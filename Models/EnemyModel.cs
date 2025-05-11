using System;

namespace OOD_RPG.Models
{
    /// <summary>
    /// Model class for enemies in the game
    /// </summary>
    public class EnemyModel
    {
        public string Name { get; set; }
        public int LifePoints { get; set; }
        public int AttackValue { get; set; }
        public int ArmorPoints { get; set; }
        public char Symbol { get; set; }
        public EnemyType EnemyType { get; private set; }

        /// <summary>
        /// Create a new enemy of the specified type
        /// </summary>
        public EnemyModel(EnemyType type)
        {
            EnemyType = type;
            SetEnemyDetails();
        }

        /// <summary>
        /// Create a custom enemy with specific attributes
        /// </summary>
        public EnemyModel(string name, int lifePoints, int attackValue, int armorPoints, char symbol)
        {
            Name = name;
            LifePoints = lifePoints;
            AttackValue = attackValue;
            ArmorPoints = armorPoints;
            Symbol = symbol;
            // Set to default type
            EnemyType = EnemyType.Goblin;
        }

        /// <summary>
        /// Set enemy details based on type
        /// </summary>
        private void SetEnemyDetails()
        {
            switch (EnemyType)
            {
                case EnemyType.Goblin:
                    Name = "Goblin";
                    LifePoints = 15;
                    AttackValue = 5;
                    ArmorPoints = 2;
                    Symbol = 'G';
                    break;

                case EnemyType.Orc:
                    Name = "Orc";
                    LifePoints = 25;
                    AttackValue = 8;
                    ArmorPoints = 4;
                    Symbol = 'O';
                    break;

                case EnemyType.Troll:
                    Name = "Troll";
                    LifePoints = 40;
                    AttackValue = 12;
                    ArmorPoints = 6;
                    Symbol = 'T';
                    break;

                default:
                    Name = "Unknown Enemy";
                    LifePoints = 10;
                    AttackValue = 3;
                    ArmorPoints = 1;
                    Symbol = 'E';
                    break;
            }
        }

        /// <summary>
        /// Apply damage to the enemy
        /// </summary>
        /// <param name="damage">Amount of damage to apply</param>
        /// <returns>True if the enemy is defeated</returns>
        public bool TakeDamage(int damage)
        {
            // Calculate actual damage after armor reduction
            int finalDamage = Math.Max(1, damage - ArmorPoints);
            LifePoints -= finalDamage;

            // Return true if enemy is defeated
            return LifePoints <= 0;
        }

        /// <summary>
        /// Get the attack damage of this enemy
        /// </summary>
        public int GetAttackDamage()
        {
            // Could be randomized, but keeping it simple for now
            return AttackValue;
        }

        /// <summary>
        /// Convert an existing Enemy to an EnemyModel
        /// </summary>
        public static EnemyModel FromEnemy(Enemy existingEnemy)
        {
            return new EnemyModel(existingEnemy.GetEnemyType())
            {
                Name = existingEnemy.Name,
                LifePoints = existingEnemy.LifePoints,
                AttackValue = existingEnemy.AttackValue,
                ArmorPoints = existingEnemy.ArmorPoints,
                Symbol = existingEnemy.Symbol
            };
        }
    }

    /// <summary>
    /// Types of enemies in the game
    /// </summary>
    public enum EnemyType
    {
        Goblin,
        Orc,
        Troll
    }

    /// <summary>
    /// Types of attacks
    /// </summary>
    public enum AttackType
    {
        Normal,
        Stealth,
        Magic
    }
}