using OOD_RPG.Visitors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOD_RPG
{
    internal class CombatSystem
    {
        private Player player;
        private GameDisplay display;
        private Room room;

        // Dictionary to map attack types to visitors 
        private Dictionary<AttackType, IAttackVisitor> attackVisitors;
        private Dictionary<AttackType, IDefenseVisitor> defenseVisitors;

        public CombatSystem(Player player, Room room, GameDisplay display)
        {
            this.player = player;
            this.room = room;
            this.display = display;

            InitializeVisitors(); 
        }

        private void InitializeVisitors()
        {
            attackVisitors = new Dictionary<AttackType, IAttackVisitor>
            {
                { AttackType.Normal, new NormalAttackVisitor() },
                { AttackType.Stealth, new StealthAttackVisitor() },
                { AttackType.Magic, new MagicAttackVisitor() }
            };

            defenseVisitors = new Dictionary<AttackType, IDefenseVisitor>
            {
                { AttackType.Normal, new NormalDefenseVisitor() },
                { AttackType.Stealth, new StealthDefenseVisitor() },
                { AttackType.Magic, new MagicDefenseVisitor() }
            };
        }

        // This method works by calculating, applying and displaying the damage done to the enemy and the player
        public bool FightEnemy(Enemy enemy, int enemyX, int enemyY)
        {
            // Use enhanced display methods
            display.DisplayCombatScreen(player, enemy);
            display.DisplayAttackOptions();

            AttackType attackType = GetAttackChoice();
            int playerDamage = CalculatePlayerDamage(attackType);
            bool enemyDefeated = enemy.TakeDamage(playerDamage);

            // Create result message
            string resultMessage = $"You hit the {enemy.Name} for {playerDamage} damage.";

            if (enemyDefeated)
            {
                resultMessage += $"\nYou defeated the {enemy.Name}!";
                this.room.RemoveEnemy(enemyX, enemyY); 
            }
            else
            {
                // Enemy attacks back
                int enemyDamage = enemy.GetAttackDamage();
                int playerDefense = CalculatePlayerDefense(attackType);

                // Calculate actual damage to player - use player defense to reduce enemy damage
                int finalDamage = Math.Max(1, enemyDamage - playerDefense);
                player.ModifyAttribute("Health", -finalDamage); // I use ModifyAttribute to change the health of the player

                resultMessage += $"\nThe {enemy.Name} hits you for {finalDamage} damage.";

                if (player.Health <= 0)
                {
                    resultMessage += "\nYou have been defeated! Game Over.";
                    display.DisplayCombatResult(resultMessage);
                    return false;
                }
            }

            display.DisplayCombatResult(resultMessage);
            return true;
        }

        private AttackType GetAttackChoice()
        {
            while (true)
            {
                ConsoleKeyInfo key = Console.ReadKey(true);

                switch (key.Key)
                {
                    case ConsoleKey.D1:
                        display.DisplayText("You chose Normal Attack!");
                        return AttackType.Normal;
                    case ConsoleKey.D2:
                        display.DisplayText("You chose Stealth Attack!");
                        return AttackType.Stealth;
                    case ConsoleKey.D3:
                        display.DisplayText("You chose Magic Attack!");
                        return AttackType.Magic;
                }
            }
        }

        private int CalculatePlayerDamage(AttackType attackType)
        {
            IAttackVisitor visitor = attackVisitors[attackType]; // get the visitor for the attack type 

            int totalDamage = 0;

            // Left hand
            if (player.LeftHand is IWeapon leftWeapon)
            {
                totalDamage += leftWeapon.AcceptAttack(visitor, player);
            }

            // Right hand (only if its different from left)
            if (player.RightHand is IWeapon rightWeapon && rightWeapon != player.LeftHand)
            {
                totalDamage += rightWeapon.AcceptAttack(visitor, player);
            }

            return totalDamage;
        }

        private int CalculatePlayerDefense(AttackType attackType)
        {
            IDefenseVisitor visitor = defenseVisitors[attackType];

            int totalDefense = 0;

            // Calculate defense from weapons
            if (player.LeftHand is IWeapon leftWeapon)
            {
                totalDefense += leftWeapon.AcceptDefense(visitor, player);
            }

            if (player.RightHand is IWeapon rightWeapon && rightWeapon != player.LeftHand)
            {
                totalDefense += rightWeapon.AcceptDefense(visitor, player);
            }

            // If no weapons equipped, add base defense based on attack type
            if (player.LeftHand == null && player.RightHand == null)
            {
                switch (attackType)
                {
                    case AttackType.Normal:
                        totalDefense += player.Dexterity;
                        break;
                    case AttackType.Stealth:
                        totalDefense += 0; 
                        break;
                    case AttackType.Magic:
                        totalDefense += player.Luck;
                        break;
                }
            }

            return totalDefense;
        }
    }

    // Enum for attack types
    public enum AttackType
    {
        Normal,
        Stealth,
        Magic
    }
}

