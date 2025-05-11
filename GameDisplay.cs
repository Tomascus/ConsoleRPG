
using OOD_RPG;
using OOD_RPG.KeyHandlers;
using OOD_RPG.Models.Effects;
using System;
using System.Collections.Generic;
using System.Text;

namespace OOD_RPG
{
    // Singleton class for game display - sealed is used to prevent inheritance 
    internal sealed class GameDisplay
    {
        // only one instance - due to singleton pattern
        private static GameDisplay instance;

        // This object is used for locking to prevent multiple threads from creating multiple instances
        private static readonly object lockObject = new object();
        private List<string> gameMessages = new List<string>();

        // Private constructor to prevent direct instantiation of the class
        private GameDisplay() { }

        // Public property to access the singleton instance - thread-safe implementation using double-check locking to prevent multiple instances
        public static GameDisplay Instance
        {
            get
            {
                // Double-check locking pattern - check if instance is null first to avoid the overhead of locking every time
                if (instance == null)
                {
                    lock (lockObject) 
                    {
                        if (instance == null)
                        {
                            instance = new GameDisplay();
                        }
                    }
                }
                return instance;
            }
        }

        public void DisplayDungeon(Room room, Player player)
        {
            Console.Clear();
            room.Draw(player);
        }

        // This displays information about the player - stats, currency, equipped items, and inventory
        // I use dollar sign for string interpolation 
        public void DisplayPlayerInfo(Room room, Player player)
        {
            Console.SetCursorPosition(room.Width + 5, 0);
            Console.WriteLine("Player Stats:");
            Console.SetCursorPosition(room.Width + 5, 1);
            Console.WriteLine($"Strength (P): {player.Strength}");
            Console.SetCursorPosition(room.Width + 5, 2);
            Console.WriteLine($"Dexterity (D): {player.Dexterity}");
            Console.SetCursorPosition(room.Width + 5, 3);
            Console.WriteLine($"Health (H): {player.Health}");
            Console.SetCursorPosition(room.Width + 5, 4);
            Console.WriteLine($"Luck (L): {player.Luck}");
            Console.SetCursorPosition(room.Width + 5, 5);
            Console.WriteLine($"Aggression (A): {player.Aggression}");
            Console.SetCursorPosition(room.Width + 5, 6);
            Console.WriteLine($"Wisdom (W): {player.Wisdom}");

            // Display currency
            Console.SetCursorPosition(room.Width + 5, 8);
            Console.WriteLine($"Coins: {player.Coins}");
            Console.SetCursorPosition(room.Width + 5, 9);
            Console.WriteLine($"Gold: {player.Gold}");

            // Display equipped items
            Console.SetCursorPosition(room.Width + 5, 11);
            Console.WriteLine("Equipped Items:");

            Console.SetCursorPosition(room.Width + 5, 12);
            Console.WriteLine($"Left Hand: {(player.LeftHand != null ? player.LeftHand.Name : "Empty")}"); // This checks if the left hand is empty - if not, it displays the item name - uses ternary operator
            if (player.LeftHand is IDamageItems leftDamageItem)
            {
                Console.SetCursorPosition(room.Width + 5, 13);
                Console.WriteLine($"  Damage: {leftDamageItem.Damage}"); 
            }

            Console.SetCursorPosition(room.Width + 5, 14);
            Console.WriteLine($"Right Hand: {(player.RightHand != null ? player.RightHand.Name : "Empty")}");
            if (player.RightHand is IDamageItems rightDamageItem && player.RightHand != player.LeftHand)
            {
                Console.SetCursorPosition(room.Width + 5, 15);
                Console.WriteLine($"  Damage: {rightDamageItem.Damage}");
            }

            // Display inventory
            Console.SetCursorPosition(room.Width + 5, 17);
            Console.WriteLine($"Inventory ({player.Inventory.Count}/{Player.InventorySize}):");

            if (player.Inventory.Count == 0)
            {
                Console.SetCursorPosition(room.Width + 5, 18);
                Console.WriteLine("(empty)");
            }
            else
            {
                int currentLine = 18;
                for (int i = 0; i < player.Inventory.Count; i++)
                {
                    // Display item with its name
                    Console.SetCursorPosition(room.Width + 5, currentLine);
                    Console.WriteLine($"{i + 1}. {player.Inventory[i].Name}");

                    // If the item deals damage, display the damage value on the next line
                    if (player.Inventory[i] is IDamageItems damageItem)
                    {
                        currentLine++;
                        Console.SetCursorPosition(room.Width + 7, currentLine);
                        Console.WriteLine($"Damage: {damageItem.Damage}");
                    }
                    // Move to the next line for the next item
                    currentLine++;
                }
            }
        }

        // Display information about the items under the player
        public void DisplayItemInfo(Room room, Player player)
        {
            IItem itemUnderPlayer = room.GetItemAt(player.X, player.Y);

            if (itemUnderPlayer != null)
            {
                // Calculate the starting line after the inventory
                // First, I count how many lines the inventory takes including damage lines
                int inventoryLines = 0;
                if (player.Inventory.Count == 0)
                {
                    inventoryLines = 1; // this is for the "(empty)" line at the start
                }
                else
                {
                    // Count lines for each item plus an extra line for each damage item
                    for (int i = 0; i < player.Inventory.Count; i++)
                    {
                        inventoryLines++; // Count the item name line
                        if (player.Inventory[i] is IDamageItems)
                        {
                            inventoryLines++; // Add another line for damage info
                        }
                    }
                }

                // Start 2 lines after the inventory part to give some space
                int startLine = 18 + inventoryLines + 2;

                // Make sure we stay within console buffer - if we exceed, adjust the start line
                if (startLine + 5 >= Console.BufferHeight)
                {
                    startLine = Console.BufferHeight - 6; // Leave space for 5 lines for item info
                }

                Console.SetCursorPosition(room.Width + 5, startLine);
                Console.WriteLine("Item on ground:");

                Console.SetCursorPosition(room.Width + 5, startLine + 1);
                Console.WriteLine(itemUnderPlayer.Name);

                Console.SetCursorPosition(room.Width + 5, startLine + 2);
                Console.WriteLine($"Description: {itemUnderPlayer.Description}");

                // If the item can deal damage, show its damage value
                if (itemUnderPlayer is IDamageItems damageItem)
                {
                    Console.SetCursorPosition(room.Width + 5, startLine + 3);
                    Console.WriteLine($"Damage: {damageItem.Damage}");
                }

                Console.SetCursorPosition(room.Width + 5, startLine + 4);
                Console.WriteLine("Press E to pick up");
            }
        }

        public void DisplayCloseEnemies(Room room, Player player)
        {
            int startX = room.Width + 35; 
            int startY = 0; 

            Console.SetCursorPosition(startX, startY++);
            Console.WriteLine("Close Enemies:");

            bool foundEnemies = false;

            // Check the 8 cells for enemies - directions around the player
            for (int dy = -1; dy <= 1; dy++)
            {
                for (int dx = -1; dx <= 1; dx++)
                {
                    if (dx == 0 && dy == 0) continue; // Skip players position 

                    int checkX = player.X + dx; 
                    int checkY = player.Y + dy;

                    // Check if the position is realistic and has an enemy
                    if (checkX >= 0 && checkX < room.Width &&
                        checkY >= 0 && checkY < room.Height &&
                        room.GetCellType(checkX, checkY) == CellType.Enemy)
                    {
                        // Make sure we don't exceed buffer height - to prevent overflow
                        if (startY < Console.BufferHeight)
                        {
                            Enemy enemy = room.GetEnemyAt(checkX, checkY);
                            string enemyInfo = enemy != null
                                ? $"{enemy.Name} (HP: {enemy.LifePoints})" // Display enemy name and HP 
                                : "Unknown Enemy"; // if enemy is null, show unknown enemy

                            if (startY < Console.BufferHeight)
                            {
                                Console.SetCursorPosition(startX, startY++);
                                Console.WriteLine($"- {enemyInfo} to the {GetDirectionName(dx, dy)}");
                                foundEnemies = true;
                            }
                        }
                    }
                }
            }

            if (!foundEnemies)
            {
                if (startY < Console.BufferHeight)
                {
                    Console.SetCursorPosition(startX, startY);
                    Console.WriteLine("No enemies nearby");
                }
            }
        }

        // Convert dx,dy to a direction name - used for enemy display 
        private string GetDirectionName(int dx, int dy)
        {
            if (dx == -1 && dy == -1) return "northwest";
            if (dx == 0 && dy == -1) return "north";
            if (dx == 1 && dy == -1) return "northeast";
            if (dx == -1 && dy == 0) return "west";
            if (dx == 1 && dy == 0) return "east";
            if (dx == -1 && dy == 1) return "southwest";
            if (dx == 0 && dy == 1) return "south";
            if (dx == 1 && dy == 1) return "southeast";
            return "unknown direction";
        }

        // Add a message to game log
        public void AddMessage(string message)
        {
            gameMessages.Add(message);

            // Keep only last 5 messages 
            if (gameMessages.Count > 5)
            {
                gameMessages.RemoveAt(0);
            }
        }

        public void DisplayMessages(Room room, int startY)
        {
            // I use the startY parameter instead of calculating based on room height
            // This prevents exceeding console buffer size in case we have a room that is larger 
            Console.SetCursorPosition(0, startY);
            Console.WriteLine("Recent messages:");

            for (int i = 0; i < gameMessages.Count; i++)
            {
                if (startY + 1 + i < Console.BufferHeight)
                {
                    Console.SetCursorPosition(0, startY + 1 + i);
                    Console.WriteLine(gameMessages[i]);
                }
            }
        }

        // This method displays the game state - dungeon, player info, items, and close enemies, basically everything
        public void DisplayGameState(Room room, Player player)
        {
            DisplayDungeon(room, player);
            DisplayPlayerInfo(room, player);
            DisplayItemInfo(room, player);
            DisplayCloseEnemies(room, player);
            DisplayActiveEffects(room, player);

            // I display messages at the bottom of the screen 
            int messageY = Math.Min(room.Height + 2, Console.BufferHeight - 7);
            DisplayMessages(room, messageY);
        }

        // Clear all console output - used to clear the screen before showing new content
        public void Clear()
        {
            Console.Clear();
        }

        // Display a confirmation prompt and get yes/no response
        public bool DisplayDecision(string message)
        {
            Console.WriteLine($"{message} (y/n)");

            while (true)
            {
                ConsoleKeyInfo key = Console.ReadKey(true);

                if (key.Key == ConsoleKey.Y)
                {
                    return true;
                }
                else if (key.Key == ConsoleKey.N)
                {
                    return false;
                }
            }
        }

        // This method displays text  
        public void DisplayText(string text)
        {
            Console.WriteLine(text);
        }

        // Method to display instructions with proper formatting
        public void DisplayInstructions(string instructions)
        {
            Clear();

            // Display the instructions
            Console.WriteLine(instructions);

            // Prompt for continuation
            Console.WriteLine();
            Console.WriteLine("Press any key to continue...");
            WaitForKeyPress();
        }

        public void WaitForKeyPress()
        {
            Console.ReadKey(true);
        }

        public void DisplayInstructions(Room room)
        {
            // We create a builder for instructions 
            IInstructionsBuilder builder = new InstructionsBuilder(room);
            InstructionsDirector director = new InstructionsDirector(builder);
            director.BuildCompleteInstructions(); // Build all instructions 
            string instructions = director.GetInstructions();

            // Display the instructions using the GameDisplay singleton so it is centralized in one place
            DisplayInstructions(instructions);
        }

        public void DisplayActiveEffects(Room room, Player player)
        {
            List<IEffect> effects = player.GetActiveEffects();

            if (effects.Count > 0)
            {
                int startX = room.Width + 35;
                int startY = 15; 

                Console.SetCursorPosition(startX, startY++);
                Console.WriteLine("Active Effects:");

                foreach (IEffect effect in effects)
                {
                    // This checks for buffer overflow and prints the effect text
                    if (startY < Console.BufferHeight)
                    {
                        Console.SetCursorPosition(startX, startY++);
                        Console.WriteLine($"- {effect.GetStatusText()}");
                    }
                }
            }
        }

        // Displays available actions based on relevant handlers
        public void DisplayAvailableActions(Room room, List<IKeyHandler> handlers)
        {
            
            if (handlers == null || handlers.Count == 0)
                return;

            int startX = room.Width + 35;
            int startY = 5; 

            Console.SetCursorPosition(startX, startY++);
            Console.WriteLine("Available Actions:");

            foreach (IKeyHandler handler in handlers)
            {
                // Check if we are within the console buffer height
                if (startY < Console.BufferHeight)
                {
                    Console.SetCursorPosition(startX, startY++);
                    Console.WriteLine($"- {handler.GetActionDescription()}");
                }
            }
        }

        // Display a menu and get user selection
        public int DisplayMenu(string title, string[] options, int min, int max)
        {
            Clear();
            Console.WriteLine(title);
            Console.WriteLine();

            for (int i = 0; i < options.Length; i++)
            {
                Console.WriteLine(options[i]);
            }

            return GetUserChoice(min, max);
        }

        // Get user choice within a range
        public int GetUserChoice(int min, int max)
        {
            int choice;
            while (true)
            {
                Console.WriteLine($"Enter your choice ({min}-{max}):");
                if (int.TryParse(Console.ReadLine(), out choice) && choice >= min && choice <= max)
                {
                    return choice;
                }
                Console.WriteLine("Invalid input. Please try again.");
            }
        }

        // Display a list of items for selection
        public int DisplayItemSelection(string prompt, List<string> items)
        {
            Clear();
            Console.WriteLine(prompt);

            for (int i = 0; i < items.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {items[i]}");
            }

            ConsoleKeyInfo key = Console.ReadKey(true);

            if (key.Key >= ConsoleKey.D1 && key.Key <= ConsoleKey.D9)
            {
                return key.Key - ConsoleKey.D1;
            }

            return -1; // Cancelled or invalid selection
        }

        public void DisplayCombatScreen(Player player, Enemy enemy)
        {
            Clear();

            Console.WriteLine("=== COMBAT ===");
            Console.WriteLine();

            // Display player information
            Console.WriteLine("Player:");
            Console.WriteLine($"Health: {player.Health}");
            Console.WriteLine($"Strength: {player.Strength}, Dexterity: {player.Dexterity}, Wisdom: {player.Wisdom}");
            Console.WriteLine($"Left Hand: {(player.LeftHand != null ? player.LeftHand.Name : "Empty")}");
            Console.WriteLine($"Right Hand: {(player.RightHand != null ? player.RightHand.Name : "Empty")}");
            Console.WriteLine();

            // Display enemy information
            Console.WriteLine($"Enemy: {enemy.Name}");
            Console.WriteLine($"Life Points: {enemy.LifePoints}");
            Console.WriteLine($"Attack: {enemy.AttackValue}");
            Console.WriteLine($"Armor: {enemy.ArmorPoints}");
            Console.WriteLine();
        }

        public void DisplayAttackOptions()
        {
            Console.WriteLine("Choose your attack:");
            Console.WriteLine("1. Normal Attack - Balanced approach");
            Console.WriteLine("2. Stealth Attack - Better with light weapons");
            Console.WriteLine("3. Magic Attack - Better with magic weapons");
            Console.WriteLine();
        }

        public void DisplayCombatResult(string resultMessage)
        {
            Console.WriteLine();
            Console.WriteLine("Combat Result:");
            Console.WriteLine(resultMessage);
            Console.WriteLine();
            Console.WriteLine("Press any key to continue...");
            WaitForKeyPress();
        }

    }
}