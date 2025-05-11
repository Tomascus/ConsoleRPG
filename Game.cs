using OOD_RPG;
using OOD_RPG.Items;
using OOD_RPG.KeyHandlers;
using OOD_RPG.Models.Effects;
using System;
using System.Collections.Generic;

namespace OOD_RPG
{
    internal class Game
    {
        private Room room;
        private Player player;
        private bool isRunning;
        private GameDisplay display;
        private CombatSystem combatSystem;


        // Chain of responsibility for handling inputs
        private IKeyHandler inputHandler;
        private int turnCounter = 0;

        public Game()
        {
            // Here I get GameDisplay singleton instance
            display = GameDisplay.Instance;
            player = new Player(0, 0);
            isRunning = true;

            // Create the dungeon using the Builder pattern
            BuildDungeonMenu();
            SpawnPlayer();
            SetupInputHandlers();

            combatSystem = new CombatSystem(player, room, display);

        }

        // Here I set up the chain of responsibility for handling inputs - They are all set up in a chain so if one handler cant handle the input, it passes it to the next one
        private void SetupInputHandlers()
        {
            MovementHandler movementHandler = new MovementHandler(this);
            PickupHandler pickupHandler = new PickupHandler(this);
            DropItemHandler dropItemHandler = new DropItemHandler(this);
            DropAllItemsHandler dropAllHandler = new DropAllItemsHandler(this);
            EquipItemHandler equipHandler = new EquipItemHandler(this);
            DrinkPotionHandler potionHandler = new DrinkPotionHandler(this);
            ExitHandler exitHandler = new ExitHandler(this);
            InvalidInputHandler invalidHandler = new InvalidInputHandler(this);

            movementHandler.SetNext(pickupHandler);
            pickupHandler.SetNext(dropItemHandler);
            dropItemHandler.SetNext(dropAllHandler);
            dropAllHandler.SetNext(equipHandler);
            equipHandler.SetNext(potionHandler);
            potionHandler.SetNext(exitHandler);
            exitHandler.SetNext(invalidHandler);

            // Set the first handler as movementHandler - this is the first one to be checked
            inputHandler = movementHandler;
        }

        private void BuildDungeonMenu()
        {
            // Create builder and director
            IDungeonBuilder builder = new DungeonBuilder();
            DungeonDirector director = new DungeonDirector(builder);

            display.Clear();
            Console.WriteLine("=== DUNGEON BUILDER ===");
            Console.WriteLine();
            Console.WriteLine("Choose a dungeon type:");
            Console.WriteLine("1. Empty Dungeon");
            Console.WriteLine("2. Filled Dungeon");
            Console.WriteLine("3. Preset Simple Dungeon");
            Console.WriteLine("4. Preset Complex Dungeon");

            int choice = GetUserChoice(1, 4);

            // Dungeon size
            int height = 20;
            int width = 40;

            // If the user chooses an empty or filled dungeon, we start asking for other strategies
            if (choice == 1 || choice == 2)
            {
                bool isFilled = (choice == 2);
                List<string> strategies = new List<string>(); // Store strategies to apply

                if (isFilled)
                {
                    display.Clear();
                    Console.WriteLine("Select from the following options to build the dungeon:");
                    Console.WriteLine();

                    // Keep asking until we have at least one open space strategy - so the player can move around
                    bool hasPath = false;
                    while (!hasPath)
                    {
                        Console.WriteLine("Do you want to add random paths? (y/n)");
                        if (Console.ReadKey(true).Key == ConsoleKey.Y)
                        {
                            strategies.Add("paths");
                            hasPath = true;
                        }

                        Console.WriteLine("Do you want to add random chambers/rooms? (y/n)");
                        if (Console.ReadKey(true).Key == ConsoleKey.Y)
                        {
                            strategies.Add("chambers");
                            hasPath = true;
                        }

                        Console.WriteLine("Do you want to add a central room? (y/n)");
                        if (Console.ReadKey(true).Key == ConsoleKey.Y)
                        {
                            strategies.Add("central");
                            hasPath = true;
                        }

                        if (!hasPath)
                        {
                            Console.WriteLine("You must select at least one option to create a playable dungeon");
                        }
                    }
                }

                // Ask for additional features for both empty and filled dungeons
                display.Clear();
                Console.WriteLine("Select additional features for your dungeon:");
                Console.WriteLine();

                // For empty dungeons, we only ask about contents, not structure
                if (!isFilled)
                {
                    Console.WriteLine("Do you want to add regular items? (y/n)");
                    if (Console.ReadKey(true).Key == ConsoleKey.Y)
                    {
                        strategies.Add("items");
                    }

                    Console.WriteLine("Do you want to add weapons? (y/n)");
                    if (Console.ReadKey(true).Key == ConsoleKey.Y)
                    {
                        strategies.Add("weapons");
                    }

                    Console.WriteLine("Do you want to add effect-based weapons? (y/n)");
                    if (Console.ReadKey(true).Key == ConsoleKey.Y)
                    {
                        strategies.Add("modweapons");
                    }

                    Console.WriteLine("Do you want to add potions? (y/n)");
                    if (Console.ReadKey(true).Key == ConsoleKey.Y)
                    {
                        strategies.Add("potions");
                    }

                    Console.WriteLine("Do you want to add enemies? (y/n)");
                    if (Console.ReadKey(true).Key == ConsoleKey.Y)
                    {
                        strategies.Add("enemies");
                    }
                }
                else
                {
                    // For filled dungeons give options to change dungeon structure
                    if (!strategies.Contains("chambers"))
                    {
                        Console.WriteLine("Do you want to add random chambers/rooms? (y/n)");
                        if (Console.ReadKey(true).Key == ConsoleKey.Y)
                        {
                            strategies.Add("chambers");
                        }
                    }

                    if (!strategies.Contains("central"))
                    {
                        Console.WriteLine("Do you want to add a central room? (y/n)");
                        if (Console.ReadKey(true).Key == ConsoleKey.Y)
                        {
                            strategies.Add("central");
                        }
                    }

                    if (!strategies.Contains("paths"))
                    {
                        Console.WriteLine("Do you want to add random paths? (y/n)");
                        if (Console.ReadKey(true).Key == ConsoleKey.Y)
                        {
                            strategies.Add("paths");
                        }
                    }

                    // Then ask about items and entities
                    Console.WriteLine("Do you want to add regular items? (y/n)");
                    if (Console.ReadKey(true).Key == ConsoleKey.Y)
                    {
                        strategies.Add("items");
                    }

                    Console.WriteLine("Do you want to add weapons? (y/n)");
                    if (Console.ReadKey(true).Key == ConsoleKey.Y)
                    {
                        strategies.Add("weapons");
                    }

                    Console.WriteLine("Do you want to add effect-based weapons? (y/n)");
                    if (Console.ReadKey(true).Key == ConsoleKey.Y)
                    {
                        strategies.Add("modweapons");
                    }

                    Console.WriteLine("Do you want to add potions? (y/n)");
                    if (Console.ReadKey(true).Key == ConsoleKey.Y)
                    {
                        strategies.Add("potions");
                    }

                    Console.WriteLine("Do you want to add enemies? (y/n)");
                    if (Console.ReadKey(true).Key == ConsoleKey.Y)
                    {
                        strategies.Add("enemies");
                    }
                }

                // Initialize dungeon
                builder.InitializeDungeon(height, width, isFilled);

                // Apply selected strategies
                foreach (string strategy in strategies)
                {
                    switch (strategy)
                    {
                        case "paths":
                            builder.BuildPaths(30, 20);
                            break;
                        case "chambers":
                            builder.BuildChambers(5, 3, 6);
                            break;
                        case "central":
                            builder.BuildCentralRoom(8, 6);
                            break;
                        case "items":
                            builder.BuildItems(8);
                            break;
                        case "weapons":
                            builder.BuildWeapons(4);
                            break;
                        case "modweapons":
                            builder.BuildModifiedWeapons(3);
                            break;
                        case "potions":
                            builder.BuildPotions(5);
                            break;
                        case "enemies":
                            builder.BuildEnemies(6);
                            break;
                    }
                }

                // Ensure connectivity for complex dungeons
                if (strategies.Contains("paths") || strategies.Contains("chambers") || strategies.Contains("central"))
                {
                    builder.connectDungeon();
                }

                // Get the built dungeon
                room = builder.GetResult();
            }
            // Predefined dungeon layout
            else if (choice == 3)
            {
                builder.InitializeDungeon(height, width, true);
                builder.BuildPaths(40, 30);
                builder.connectDungeon();
                builder.BuildItems(10);

                // Get the built dungeon
                room = builder.GetResult();
            }
            // Predefined complex dungeon layout - has all strategies
            else if (choice == 4)
            {
                builder.InitializeDungeon(height, width, true);
                builder.BuildPaths(25, 20);
                builder.BuildChambers(4, 4, 7);
                builder.BuildCentralRoom(10, 8);

                // Ensure connectivity
                builder.connectDungeon();
                builder.BuildItems(5);
                builder.BuildWeapons(5);
                builder.BuildModifiedWeapons(3);
                builder.BuildPotions(5);
                builder.BuildEnemies(8);

                room = builder.GetResult(); 
            }

            display.Clear();
            Console.WriteLine("Dungeon created successfully.");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey(true);
        }

        // This method gets a valid user choice within a range - uses a while loop to keep asking until valid input
        private int GetUserChoice(int min, int max)
        {
            int choice;
            while (true)
            {
                Console.WriteLine($"Enter your choice ({min}-{max}):");
                if (int.TryParse(Console.ReadLine(), out choice) && choice >= min && choice <= max) // We use parsing and range check
                {
                    return choice;
                }
                Console.WriteLine("Invalid input. Please try again.");
            }
        }

        private void SpawnPlayer()
        {
            // Here I find an open space in the dungeon and place the player there
            for (int y = 0; y < room.Height; y++)
            {
                for (int x = 0; x < room.Width; x++)
                {
                    if (room.GetCellType(x, y) == CellType.Empty)
                    {
                        player.X = x;
                        player.Y = y;
                        return;
                    }
                }
            }

            // If no empty space was found, I try to create one - prevents player from being stuck
            if (room.Width > 0 && room.Height > 0)
            {
                room.SetCell(0, 0, CellType.Empty);
                player.X = 0;
                player.Y = 0;
            }
        }

        public void Start()
        {
            // Display instructions using GameDisplay singleton 
            display.DisplayInstructions(room);

            // Add a welcome message
            display.AddMessage("Welcome to the Game! Use WASD to move.");

            while (isRunning)
            {
                // Display game state with available actions
                DisplayGameState();

                // Handle player input using Chain of Responsibility
                HandleInput();
            }
        }

        private void HandleInput()
        {
            ConsoleKeyInfo keyInfo = Console.ReadKey(true);
            inputHandler.Handle(keyInfo);
        }


        private void DisplayGameState()
        {
            // Display the dungeon and player information
            display.DisplayGameState(room, player);
            // Display available actions
            display.DisplayAvailableActions(room, AvailableHandlers());
        }

        // This list returns all available handlers in the chain of responsibility
        private List<IKeyHandler> AvailableHandlers()
        {
            List<IKeyHandler> handlers = new List<IKeyHandler>();

            IKeyHandler current = inputHandler;
            while (current != null)
            {
                if (current.IsApplicable() && !(current is InvalidInputHandler))
                {
                    handlers.Add(current);
                }

                // Move to next handler
                if (current is KeyHandler keyHandler)
                {
                    current = keyHandler.nextHandler;
                }
                else
                {
                    break;
                }
            }

            return handlers;
        }

        // These methods are used to get the room and player objects - they are used in the handlers
        public Room GetRoom() => room;
        public Player GetPlayer() => player;

        internal void MovePlayer(int dx, int dy)
        {
            int newX = player.X + dx;
            int newY = player.Y + dy;

            // Check if the new position is valid
            if (newX >= 0 && newX < room.Width && newY >= 0 && newY < room.Height)
            {
                // Check if the new position is an enemy
                if (room.GetCellType(newX, newY) == CellType.Enemy)
                {
                    // Get the enemy at this position
                    Enemy enemy = room.GetEnemyAt(newX, newY);
                    if (enemy != null)
                    {
                        // Initiate combat
                        bool playerSurvived = combatSystem.FightEnemy(enemy, newX, newY);

                        if (!playerSurvived)
                        {
                            // Player died - game over
                            isRunning = false;
                            return;
                        }
                        else if (enemy.LifePoints <= 0)
                        {
                            // Enemy was defeated, move to that position
                            player.X = newX;
                            player.Y = newY;
                            display.AddMessage($"Moved to position ({player.X}, {player.Y}) after defeating enemy.");
                            EndTurn();
                        }
                        else
                        {
                            // Combat ended but enemy survived
                            display.AddMessage($"Try fighting again...");
                            EndTurn();
                        }
                    }
                }
                else if (room.GetCellType(newX, newY) != CellType.Wall)
                {
                    // Original movement code
                    player.X = newX;
                    player.Y = newY;
                    display.AddMessage($"Moved to position ({player.X}, {player.Y})");
                    EndTurn();
                }
                else
                {
                    display.AddMessage("You can't move through walls!");
                }
            }
            else
            {
                display.AddMessage("You can't leave the dungeon!");
            }
        }

        internal void PickUpItem()
        {
            IItem item = room.GetItemAt(player.X, player.Y);
            if (item != null)
            {
                // If the item is currency, add it to the player wallet
                if (item is Coins coins)
                {
                    player.Coins += coins.Amount;
                    room.RemoveItem(player.X, player.Y);
                    display.AddMessage($"Picked up {coins.Amount} coins.");
                }
                else if (item is Gold gold)
                {
                    player.Gold += gold.Amount;
                    room.RemoveItem(player.X, player.Y);
                    display.AddMessage($"Picked up {gold.Amount} gold.");
                }
                else
                {
                    // Try to add item to inventory - if not full
                    if (player.AddToInventory(item))
                    {
                        room.RemoveItem(player.X, player.Y);
                        display.AddMessage($"Picked up {item.Name}.");
                    }
                    else
                    {
                        display.AddMessage("Your inventory is full!");
                    }
                }
            }
            else
            {
                display.AddMessage("There is nothing here to pick up.");
            }
        }

        internal void DropItem()
        {
            if (player.Inventory.Count == 0)
            {
                display.AddMessage("Your inventory is empty.");
                return;
            }

            // Show inventory and give player an option to select item to drop
            display.Clear();
            Console.WriteLine("Select item to drop (1-9, 0 to cancel):");

            for (int i = 0; i < player.Inventory.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {player.Inventory[i].Name}");
            }

            
            ConsoleKeyInfo keyInfo = Console.ReadKey(true);
            int index = -1; // Default to invalid index 

            // Here I check if the key is a number key and convert it to an index - then check if its in range valid ones
            if (keyInfo.Key >= ConsoleKey.D1 && keyInfo.Key <= ConsoleKey.D9)
            {
                index = keyInfo.Key - ConsoleKey.D1; 
            }

            if (index >= 0 && index < player.Inventory.Count)
            {
                // Check if there's already an item at this location
                if (room.GetItemAt(player.X, player.Y) != null)
                {
                    display.AddMessage("There is already an item here. Move to an empty space to drop your item.");
                    return;
                }

                IItem item = player.RemoveFromInventory(index);
                if (item != null)
                {
                    room.PlaceItem(item, player.X, player.Y);
                    display.AddMessage($"Dropped {item.Name} on the ground.");
                }
            }
            else
            {
                display.AddMessage("Dropping item cancelled.");
            }
        }

        internal void DropAllItems()
        {

            if (player.Inventory.Count == 0)
            {
                display.AddMessage("Your inventory is empty.");
                return;
            }

            display.Clear();
            bool confirmed = display.DisplayDecision("Are you sure you want to drop all items?");

            if (!confirmed)
            {
                display.AddMessage("Drop all items cancelled.");
                return;
            }

            // Drop all items from inventory
            while (player.Inventory.Count > 0)
            {
                IItem item = player.RemoveFromInventory(0);

                // Try to find a nearby empty space to drop the item
                bool dropped = false;
                for (int dy = 0; dy <= 1; dy++)
                {
                    for (int dx = 0; dx <= 1; dx++)
                    {
                        int dropX = player.X + dx;
                        int dropY = player.Y + dy;

                        if (dropX < room.Width && dropY < room.Height &&
                            room.GetCellType(dropX, dropY) != CellType.Wall &&
                            room.GetItemAt(dropX, dropY) == null)
                        {
                            room.PlaceItem(item, dropX, dropY);
                            dropped = true;
                            break;
                        }
                    }
                    if (dropped) break;
                }

                if (!dropped)
                {
                    // Couldn't find space to drop, stop the process
                    display.AddMessage("Not enough space to drop all items...");
                    player.AddToInventory(item);
                    break;
                }
            }

            display.AddMessage("Dropped all items...");
        }

        internal void EquipItem(int index)
        {
            if (index >= 0 && index < player.Inventory.Count)
            {
                IItem item = player.Inventory[index];

                // Check if the item is a damage item (which includes decorated weapons)
                if (item is IDamageItems damageItem)
                {
                    // Use the IsTwoHanded check directly from the IDamageItems interface
                    bool isTwoHanded = damageItem.IsTwoHanded;

                    // Check if the item is two-handed using the interface property
                    if (isTwoHanded)
                    {
                        // Unequip both hands if they have items
                        if (player.LeftHand != null)
                        {
                            // Remove effects from the left hand item
                            if (player.LeftHand is IItemDecorator leftDecorator)
                            {
                                leftDecorator.RemoveEffectsFromPlayer(player);
                            }

                            player.AddToInventory(player.LeftHand);
                            player.LeftHand = null;
                        }

                        if (player.RightHand != null && player.RightHand != player.LeftHand)
                        {
                            // Remove effects from the right hand item
                            if (player.RightHand is IItemDecorator rightDecorator)
                            {
                                rightDecorator.RemoveEffectsFromPlayer(player);
                            }

                            player.AddToInventory(player.RightHand);
                            player.RightHand = null;
                        }

                        // Equip the two-handed weapon
                        player.LeftHand = item;
                        player.RightHand = item; // Same reference for both hands
                        player.RemoveFromInventory(index);

                        // Apply effects from the weapon
                        if (item is IItemDecorator decorator)
                        {
                            decorator.ApplyEffectsToPlayer(player);
                        }

                        display.AddMessage($"Equipped two-handed {item.Name} in both hands.");
                    }
                    else
                    {
                        // Ask which hand to equip to 
                        display.Clear();
                        Console.WriteLine("Equip to which hand? (L/R):");

                        ConsoleKeyInfo handKey = Console.ReadKey(true);

                        if (handKey.Key == ConsoleKey.L)
                        {
                            // Check if right hand has a two-handed weapon
                            if (player.RightHand != null && player.RightHand is IDamageItems rightHandWeapon && rightHandWeapon.IsTwoHanded)
                            {
                                // Remove effects from the two-handed weapon
                                if (player.RightHand is IItemDecorator twoHandedDecorator)
                                {
                                    twoHandedDecorator.RemoveEffectsFromPlayer(player);
                                }

                                player.AddToInventory(player.RightHand);
                                player.RightHand = null;
                                player.LeftHand = null;
                            }

                            // If left hand is not empty, put the item back in inventory
                            if (player.LeftHand != null)
                            {
                                // Remove effects from the left hand item
                                if (player.LeftHand is IItemDecorator leftDecorator)
                                {
                                    leftDecorator.RemoveEffectsFromPlayer(player);
                                }

                                player.AddToInventory(player.LeftHand);
                            }

                            player.LeftHand = item; // Use item instead of weapon
                            player.RemoveFromInventory(index);

                            // Apply effects from the weapon
                            if (item is IItemDecorator decorator)
                            {
                                decorator.ApplyEffectsToPlayer(player);
                            }

                            display.AddMessage($"Equipped {item.Name} in left hand.");
                        }
                        else if (handKey.Key == ConsoleKey.R)
                        {
                            // Check if left hand has a two-handed weapon
                            if (player.LeftHand != null && player.LeftHand is IDamageItems leftHandWeapon && leftHandWeapon.IsTwoHanded)
                            {
                                // Remove effects from the two-handed weapon
                                if (player.LeftHand is IItemDecorator twoHandedDecorator)
                                {
                                    twoHandedDecorator.RemoveEffectsFromPlayer(player);
                                }

                                player.AddToInventory(player.LeftHand);
                                player.LeftHand = null;
                                player.RightHand = null;
                            }

                            // If right hand is not empty, put the item back in inventory
                            if (player.RightHand != null)
                            {
                                // Remove effects from the right hand item
                                if (player.RightHand is IItemDecorator rightDecorator)
                                {
                                    rightDecorator.RemoveEffectsFromPlayer(player);
                                }

                                player.AddToInventory(player.RightHand);
                            }

                            player.RightHand = item; // Use item instead of weapon so we can equip decorated weapons
                            player.RemoveFromInventory(index);

                            // Apply effects from the weapon
                            if (item is IItemDecorator decorator)
                            {
                                decorator.ApplyEffectsToPlayer(player);
                            }

                            display.AddMessage($"Equipped {item.Name} in right hand.");
                        }
                        else
                        {
                            display.AddMessage("Equipping cancelled.");
                        }
                    }
                }
                else
                {
                    display.AddMessage($"{item.Name} cannot be equipped.");
                }
            }
        }

        // This method is used to drink potions - it finds all potions in the inventory and lets the player choose one
        internal void DrinkPotion()
        {
            Player player = GetPlayer();
            List<int> potionIndexes = new List<int>();
            List<string> potionNames = new List<string>();

            for (int i = 0; i < player.Inventory.Count; i++)
            {
                if (player.Inventory[i] is Potion)
                {
                    potionIndexes.Add(i); // Store index of the potion
                    potionNames.Add(player.Inventory[i].Name); // Store name of the potion
                }
            }

            // Check if there are any potions
            if (potionIndexes.Count == 0)
            {
                display.AddMessage("You do not have any potions in your inventory.");
                return;
            }

            // Display potions to choose from
            display.Clear();
            display.DisplayText("Select a potion to drink (0 to cancel):");

            for (int i = 0; i < potionIndexes.Count; i++)
            {
                display.DisplayText($"{i + 1}. {potionNames[i]}");
            }

            // Get players choice
            ConsoleKeyInfo choiceKey = Console.ReadKey(true);
            int choice = -1;

            if (choiceKey.Key >= ConsoleKey.D1 && choiceKey.Key <= ConsoleKey.D9)
            {
                choice = choiceKey.Key - ConsoleKey.D1;
            }

            if (choice >= 0 && choice < potionIndexes.Count)
            {
                DrinkChosenPotion(potionIndexes[choice]);
            }
            else
            {
                display.AddMessage("Drinking potion cancelled...");
            }
        }

        // This method uses index of the potion in the inventory to find it and apply its effects to player
        private void DrinkChosenPotion(int inventoryIndex)
        {
            if (inventoryIndex >= 0 && inventoryIndex < player.Inventory.Count)
            {
                IItem item = player.Inventory[inventoryIndex];

                if (item is Potion potion)
                {
                    // Removes the potion from inventory first
                    player.RemoveFromInventory(inventoryIndex);

                    // Here I check if its an antidote and call remove effects of all potions if so
                    if (potion.GetPotionType() == PotionType.Antidote)
                    {
                        player.RemoveAllTemporaryEffects();
                        display.AddMessage("You drink the antidote. All temporary effect were removed.");
                    }
                    else
                    {
                        // Create and apply the effect from the potion
                        IEffect effect = potion.CreateEffect();
                        player.AddEffect(effect);
                        display.AddMessage($"You drink the {potion.Name}. {effect.Description}");
                    }

                    EndTurn();
                }
                else
                {
                    display.AddMessage("That item is not a potion...");
                }
            }
        }

        internal void ExitGame()
        {
            display.Clear();
            bool confirmed = display.DisplayDecision("Are you sure you want to exit the game?");

            if (!confirmed)
            {
                display.AddMessage("Exit cancelled.");
                return;
            }

            isRunning = false;
        }

        // Currenlty turns are incremented only when player picks up an item or uses potions
        internal void EndTurn()
        {
            
            turnCounter++;
            player.UpdateEffects();

            display.AddMessage($"Turn {turnCounter} completed.");
        }


    }
}