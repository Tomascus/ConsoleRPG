using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOD_RPG
{
    internal class Enemy
    {
        public string Name { get; private set; }
        public int LifePoints { get; private set; }
        public int AttackValue { get; private set; }
        public int ArmorPoints { get; private set; }
        public char Symbol { get; private set; }
        private EnemyType type;

        // Constructor that takes an enemy type and sets the name and other attributes based on it
        public Enemy(EnemyType type)
        {
            this.type = type;
            SetEnemyDetails();
        }

        // Full enemy constructor
        public Enemy(string name, int lifePoints, int attackValue, int armorPoints, char symbol = 'E')
        {
            Name = name;
            LifePoints = lifePoints;
            AttackValue = attackValue;
            ArmorPoints = armorPoints;
            Symbol = symbol;
        }

        private void SetEnemyDetails()
        {
            switch (type)
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

        // Get the enemy type
        public EnemyType GetEnemyType()
        {
            return type;
        }

        // Take damage method
        public bool TakeDamage(int damage)
        {
            // Calculates actual damage after armor points
            int finalDamage = Math.Max(1, damage - ArmorPoints);
            LifePoints -= finalDamage;

            // Returns true if the enemy is defeated
            return LifePoints <= 0;
        }

        // Get attack damage
        public int GetAttackDamage()
        {
            // constant for now - could be randomized or based on other factors
            return AttackValue;
        }

        public enum EnemyType
        {
            Goblin,
            Orc,
            Troll,
        }
    }
}
