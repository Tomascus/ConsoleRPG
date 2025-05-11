using OOD_RPG.Decorators;
using OOD_RPG.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static OOD_RPG.Enemy;

namespace OOD_RPG
{
    // This class is responsible for building dungeons based on various strategies
    internal class DungeonBuilder : IDungeonBuilder
    {
        private Room dungeon;
        private Random random = new Random();
        private List<(int, int)> emptySpaces = new List<(int, int)>(); // Track empty spaces for item placement
        private bool[,] visited; // Track visited cells for connectivity of dungeon

        // Initialize dungeon (can be filled or empty)
        public void InitializeDungeon(int height, int width, bool filled)
        {
            // Create a new room
            dungeon = new Room(height, width);
            emptySpaces.Clear();
            visited = new bool[height, width];

            // Fill with walls or empty spaces based on what was chosen
            CellType defaultCell = filled ? CellType.Wall : CellType.Empty;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    dungeon.SetCell(x, y, defaultCell);
                    if (defaultCell == CellType.Empty && x > 0 && y > 0 && x < width - 1 && y < height - 1)
                    {
                        emptySpaces.Add((x, y));
                    }
                }
            }

            // Add borders regardless of filled/empty - for better visuals
            BuildBorders();
        }

        public void BuildBorders()
        {
            if (dungeon == null)
            {
                InitializeDungeon(20, 40, false); 
            }

            int height = dungeon.Height;
            int width = dungeon.Width;

            // Top and bottom borders
            for (int x = 0; x < width; x++)
            {
                dungeon.SetCell(x, 0, CellType.Wall);
                dungeon.SetCell(x, height - 1, CellType.Wall);

                // Set these positions as non-empty for item placement etc
                emptySpaces.Remove((x, 0));
                emptySpaces.Remove((x, height - 1));
            }

            // Left and right borders
            for (int y = 0; y < height; y++)
            {
                dungeon.SetCell(0, y, CellType.Wall);
                dungeon.SetCell(width - 1, y, CellType.Wall);
                emptySpaces.Remove((0, y));
                emptySpaces.Remove((width - 1, y));
            }
        }

        // Additional strategies
        public void BuildPaths(int pathCount, int pathLength)
        {
            if (dungeon == null)
            {
                InitializeDungeon(20, 40, true); 
            }

            int height = dungeon.Height;
            int width = dungeon.Width;

            // Initialize visited array if not already - for connectivity check
            if (visited == null)
            {
                visited = new bool[height, width];
            }

            // This loop will create paths in the dungeon based on the given count
            for (int i = 0; i < pathCount; i++)
            {
                // Start from random valid position - not too close to the border
                int x = random.Next(2, width - 2);
                int y = random.Next(2, height - 2);

                // Here I make it so that the path can change direction but stick to the same direction for a while - have small chance to change direction
                int currentDirection = random.Next(4);
                int directionChangeChance = 20; 

                for (int j = 0; j < pathLength; j++)
                {
                    // Change walls to empty spaces
                    if (x > 0 && x < width - 1 && y > 0 && y < height - 1)
                    {
                        dungeon.SetCell(x, y, CellType.Empty);
                        if (!emptySpaces.Contains((x, y)))
                        {
                            emptySpaces.Add((x, y));
                        }
                        visited[y, x] = true; 
                    }

                    // This makes it possible to randomly change direction of the path for better procedural generation
                    if (random.Next(100) < directionChangeChance)
                    {
                        currentDirection = random.Next(4);
                    }

                    // Calculate next position based on current direction
                    int nextX = x;
                    int nextY = y;

                    switch (currentDirection)
                    {
                        case 0: nextY--; break; // Up
                        case 1: nextY++; break; // Down
                        case 2: nextX--; break; // Left
                        case 3: nextX++; break; // Right
                    }

                    // Ensure we do not go out of bounds or too close to the border
                    if (nextX < 2 || nextX >= width - 2 || nextY < 2 || nextY >= height - 2)
                    {
                        // If we are too close to the border, change direction
                        currentDirection = (currentDirection + 2) % 4; // Reverse direction
                    }
                    else
                    {
                        // Move to next position
                        x = nextX;
                        y = nextY;
                    }
                }
            }
        }

        public void BuildChambers(int chamberCount, int minSize, int maxSize)
        {
            if (dungeon == null)
            {
                InitializeDungeon(20, 40, true); 
            }

            int height = dungeon.Height;
            int width = dungeon.Width;

            // Create list of chambers to avoid overlap of them
            List<(int x, int y, int w, int h)> chambers = new List<(int x, int y, int w, int h)>();

            // Creates chambers that are rectangular rooms
            for (int i = 0; i < chamberCount; i++)
            {
                int roomWidth = random.Next(minSize, maxSize + 1); 
                int roomHeight = random.Next(minSize, maxSize + 1);
                int x = random.Next(2, width - roomWidth - 2); 
                int y = random.Next(2, height - roomHeight - 2);

                // Check if the chamber overlaps with any existing chambers - if so, it breaks the loop and tries again
                bool overlaps = false;
                foreach (var chamber in chambers)
                {
                    if (x + roomWidth + 1 > chamber.x &&
                        chamber.x + chamber.w + 1 > x &&
                        y + roomHeight + 1 > chamber.y &&
                        chamber.y + chamber.h + 1 > y)
                    {
                        overlaps = true;
                        break;
                    }
                }

                // If no overlap, create room
                if (!overlaps)
                {
                    chambers.Add((x, y, roomWidth, roomHeight));

                    // Create the room with distinct walls and floor
                    for (int dy = -1; dy <= roomHeight; dy++)
                    {
                        for (int dx = -1; dx <= roomWidth; dx++)
                        {
                            int cellX = x + dx;
                            int cellY = y + dy;

                            // Skip if out of bounds
                            if (cellX < 0 || cellX >= width || cellY < 0 || cellY >= height)
                                continue;

                            // If the room is on the border, make it a wall
                            if (dx == -1 || dx == roomWidth || dy == -1 || dy == roomHeight)
                            {
                                if (dungeon.GetCellType(cellX, cellY) != CellType.Empty)
                                {
                                    dungeon.SetCell(cellX, cellY, CellType.Wall);
                                }
                            }
                            // Otherwise, make it empty
                            else
                            {
                                dungeon.SetCell(cellX, cellY, CellType.Empty);
                                if (!emptySpaces.Contains((cellX, cellY)))
                                {
                                    emptySpaces.Add((cellX, cellY));
                                }

                                // Mark as visited for connectivity check
                                visited[cellY, cellX] = true;
                            }
                        }
                    }

                    // Add doors to each wall (top, right, bottom, left) - change in future ?
                    int doorX, doorY;

                    // Top door - get random position on top wall
                    doorX = x + random.Next(1, roomWidth);
                    doorY = y - 1;
                    if (doorY >= 0)
                    {
                        dungeon.SetCell(doorX, doorY, CellType.Empty);
                        if (!emptySpaces.Contains((doorX, doorY)))
                        {
                            emptySpaces.Add((doorX, doorY));
                        }
                    }

                    // Right door
                    doorX = x + roomWidth;
                    doorY = y + random.Next(1, roomHeight);
                    if (doorX < width)
                    {
                        dungeon.SetCell(doorX, doorY, CellType.Empty);
                        if (!emptySpaces.Contains((doorX, doorY)))
                        {
                            emptySpaces.Add((doorX, doorY));
                        }
                    }

                    // Bottom door
                    doorX = x + random.Next(1, roomWidth);
                    doorY = y + roomHeight;
                    if (doorY < height)
                    {
                        dungeon.SetCell(doorX, doorY, CellType.Empty);
                        if (!emptySpaces.Contains((doorX, doorY)))
                        {
                            emptySpaces.Add((doorX, doorY));
                        }
                    }

                    // Left door
                    doorX = x - 1;
                    doorY = y + random.Next(1, roomHeight);
                    if (doorX >= 0)
                    {
                        dungeon.SetCell(doorX, doorY, CellType.Empty);
                        if (!emptySpaces.Contains((doorX, doorY)))
                        {
                            emptySpaces.Add((doorX, doorY));
                        }
                    }
                }
                else
                {
                    // Try again if there was an overlap
                    i--;
                }
            }
        }

        public void BuildCentralRoom(int width, int height)
        {
            if (dungeon == null)
            {
                InitializeDungeon(20, 40, true); 
            }

            // Calculate center position
            int centerX = dungeon.Width / 2 - width / 2;
            int centerY = dungeon.Height / 2 - height / 2;

            // Create central room with walls and floor
            for (int dy = -1; dy <= height; dy++)
            {
                for (int dx = -1; dx <= width; dx++)
                {
                    int cellX = centerX + dx;
                    int cellY = centerY + dy;

                    if (cellX >= 0 && cellX < dungeon.Width && cellY >= 0 && cellY < dungeon.Height)
                    {
                        // If by some chance the room is on the border, make it a wall - for smaller dungeons
                        if (dx == -1 || dx == width || dy == -1 || dy == height)
                        {
                            dungeon.SetCell(cellX, cellY, CellType.Wall);
                        }
                        // Interior of the room is empty
                        else
                        {
                            dungeon.SetCell(cellX, cellY, CellType.Empty);
                            if (!emptySpaces.Contains((cellX, cellY)))
                            {
                                emptySpaces.Add((cellX, cellY));
                            }
                            // Mark as visited for connectivity
                            visited[cellY, cellX] = true;
                        }
                    }
                }
            }

            // Add doors in each direction
            // Top door
            dungeon.SetCell(centerX + width / 2, centerY - 1, CellType.Empty);
            emptySpaces.Add((centerX + width / 2, centerY - 1));

            // Right door
            dungeon.SetCell(centerX + width, centerY + height / 2, CellType.Empty);
            emptySpaces.Add((centerX + width, centerY + height / 2));

            // Bottom door
            dungeon.SetCell(centerX + width / 2, centerY + height, CellType.Empty);
            emptySpaces.Add((centerX + width / 2, centerY + height));

            // Left door
            dungeon.SetCell(centerX - 1, centerY + height / 2, CellType.Empty);
            emptySpaces.Add((centerX - 1, centerY + height / 2));
        }

        // Ensure all open areas of the dungeon are connected
        public void connectDungeon()
        {
            if (dungeon == null)
            {
                InitializeDungeon(20, 40, true); 
            }

            int height = dungeon.Height;
            int width = dungeon.Width;

            // Find all disconnected regions - empty spaces that are not visited
            bool[,] visited = new bool[height, width];
            List<List<(int x, int y)>> regions = new List<List<(int x, int y)>>(); // We keep track of these regions to connect them - it is a list of lists 

            // Find all regions
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (dungeon.GetCellType(x, y) == CellType.Empty && !visited[y, x])
                    {
                        List<(int x, int y)> region = new List<(int x, int y)>(); // Create a new region list for local scope
                        FloodFill(x, y, region, visited); // I use flood fill to find all connected empty spaces
                        regions.Add(region); // add the region to the list of regions 
                    }
                }
            }

            // If we have multiple regions, connect them
            if (regions.Count > 1)
            {
                for (int i = 0; i < regions.Count - 1; i++)
                {
                    // Finds closest points between this region and the next
                    ConnectRegions(regions[i], regions[i + 1]);
                }
            }
        }

        // This method works by recursively checking all four directions of a cell to find all connected empty spaces
        private void FloodFill(int x, int y, List<(int x, int y)> region, bool[,] visited)
        {
            // Checks bounds - if out of bounds, return 
            if (x < 0 || x >= dungeon.Width || y < 0 || y >= dungeon.Height)
                return;

            // Check if already visited or not empty 
            if (visited[y, x] || dungeon.GetCellType(x, y) != CellType.Empty)
                return;

            // Mark as visited and add to region
            visited[y, x] = true;
            region.Add((x, y));

            // Recursively check all four directions 
            FloodFill(x + 1, y, region, visited);
            FloodFill(x - 1, y, region, visited);
            FloodFill(x, y + 1, region, visited);
            FloodFill(x, y - 1, region, visited);
        }

        // Connect two regions by creating a path between them
        private void ConnectRegions(List<(int x, int y)> region1, List<(int x, int y)> region2)
        {
            // Find the closest pair of cells between the two regions - using Manhattan distance which is the sum of the absolute values of the differences of the coordinates
            int minDistance = int.MaxValue;
            (int x1, int y1) best1 = (0, 0);
            (int x2, int y2) best2 = (0, 0);

            // Goes through all pairs of cells between the two regions and finds the closest pair
            foreach (var cell1 in region1)
            {
                foreach (var cell2 in region2)
                {
                    int distance = Math.Abs(cell1.x - cell2.x) + Math.Abs(cell1.y - cell2.y); // Manhattan distance
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        best1 = cell1;
                        best2 = cell2;
                    }
                }
            }

            // Create a path between the two points
            int x = best1.x1;
            int y = best1.y1;

            // First move horizontally, then vertically to connect the two points
            while (x != best2.x2)
            {
                x += Math.Sign(best2.x2 - x);
                dungeon.SetCell(x, y, CellType.Empty);
                emptySpaces.Add((x, y));
            }

            while (y != best2.y2)
            {
                y += Math.Sign(best2.y2 - y);
                dungeon.SetCell(x, y, CellType.Empty);
                emptySpaces.Add((x, y));
            }
        }

        public void BuildItems(int itemCount)
        {
            if (dungeon == null)
            {
                InitializeDungeon(20, 40, false); 
            }

            // Update empty spaces list to place items
            UpdateEmptySpaces();

            // Place random items in the dungeon
            for (int i = 0; i < itemCount && emptySpaces.Count > 0; i++)
            {
                // Get a random empty space
                int index = random.Next(emptySpaces.Count);
                (int x, int y) = emptySpaces[index];
                emptySpaces.RemoveAt(index);

                // Create a random item
                IItem item;
                switch (random.Next(3))
                {
                    case 0:
                        item = new Sigil();
                        break;
                    case 1:
                        item = new Book();
                        break;
                    case 2:
                        item = new Bone();
                        break;
                    default:
                        item = new Potion();
                        break;
                }
                dungeon.PlaceItem(item, x, y);
            }
        }

        public void BuildWeapons(int weaponCount)
        {
            if (dungeon == null)
            {
                InitializeDungeon(20, 40, false); 
            }

            // Update empty spaces list to place weapons
            UpdateEmptySpaces();

            // Place random weapons in the dungeon
            for (int i = 0; i < weaponCount && emptySpaces.Count > 0; i++)
            {
                int index = random.Next(emptySpaces.Count);
                (int x, int y) = emptySpaces[index];
                emptySpaces.RemoveAt(index);

                IItem weapon;
                switch (random.Next(3))
                {
                    case 0:
                        weapon = new Sword();
                        break;
                    case 1:
                        weapon = new Axe();
                        break;
                    default:
                        weapon = new TwoHandedSword();
                        break;
                }

                // Place the weapon
                dungeon.PlaceItem(weapon, x, y);
            }
        }

        public void BuildModifiedWeapons(int weaponCount)
        {
            if (dungeon == null)
            {
                InitializeDungeon(20, 40, false); 
            }
            UpdateEmptySpaces();

            // Place random effect based weapons in the dungeon
            for (int i = 0; i < weaponCount && emptySpaces.Count > 0; i++)
            {
                int index = random.Next(emptySpaces.Count);
                (int x, int y) = emptySpaces[index];
                emptySpaces.RemoveAt(index);

                IItem baseWeapon;
                switch (random.Next(3))
                {
                    case 0:
                        baseWeapon = new Sword();
                        break;
                    case 1:
                        baseWeapon = new Axe();
                        break;
                    default:
                        baseWeapon = new TwoHandedSword();
                        break;
                }

                IItem modifiedWeapon = baseWeapon;
                if (random.Next(2) == 0)
                {
                    modifiedWeapon = new PowerfulDecorator(modifiedWeapon);
                }
                if (random.Next(2) == 0)
                {
                    modifiedWeapon = new UnluckyDecorator(modifiedWeapon);
                }
                dungeon.PlaceItem(modifiedWeapon, x, y);
            }
        }

        public void BuildPotions(int potionCount)
        {
            if (dungeon == null)
            {
                InitializeDungeon(20, 40, false); 
            }

            UpdateEmptySpaces();

            for (int i = 0; i < potionCount && emptySpaces.Count > 0; i++)
            {
                int index = random.Next(emptySpaces.Count);
                (int x, int y) = emptySpaces[index];
                emptySpaces.RemoveAt(index);

                IItem potion;
                switch (random.Next(4))
                {
                    case 0:
                        potion = new Potion(PotionType.Strength);
                        break;
                    case 1:
                        potion = new Potion(PotionType.Luck);
                        break;
                    case 2:
                        potion = new Potion(PotionType.Health);
                        break;
                    default:
                        potion = new Potion(PotionType.Antidote);
                        break;
                }

                dungeon.PlaceItem(potion, x, y);
            }
        }

        public void BuildEnemies(int enemyCount)
        {
            if (dungeon == null)
            {
                InitializeDungeon(20, 40, false);
            }

            // Update empty spaces list
            UpdateEmptySpaces();

            // Place random enemies in the dungeon
            for (int i = 0; i < enemyCount && emptySpaces.Count > 0; i++)
            {
                int index = random.Next(emptySpaces.Count);
                (int x, int y) = emptySpaces[index];
                emptySpaces.RemoveAt(index);

                // Creates random enemy type
                EnemyType randomType = GetRandomEnemyType();
                Enemy enemy = new Enemy(randomType);
                dungeon.SetCell(x, y, CellType.Enemy);

                // Also add to enemies dictionary
                dungeon.AddEnemy(enemy, x, y);
            }
        }

        // This chooses the enemy types to be selected randomly
        private EnemyType GetRandomEnemyType()
        {
            int typeIndex = random.Next(3); 
            return (EnemyType)typeIndex;
        }

        // Get the built dungeon - must be called after building the dungeon 
        public Room GetResult()
        {
            if (dungeon == null)
            {
                throw new InvalidOperationException("Use one of the starter strategies first");
            }
            return dungeon;
        }

        // This method updates the empty spaces list for item placement - called after each item placement to ensure no overlap
        private void UpdateEmptySpaces()
        {
            emptySpaces.Clear();
            for (int y = 0; y < dungeon.Height; y++)
            {
                for (int x = 0; x < dungeon.Width; x++)
                {
                    if (dungeon.GetCellType(x, y) == CellType.Empty)
                    {
                        emptySpaces.Add((x, y));
                    }
                }
            }
        }
    }
}
