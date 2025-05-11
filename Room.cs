using OOD_RPG;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOD_RPG
{
    internal class Room
    {
        private CellType[,] grid; // 2D array to represent the room
        private Dictionary<(int, int), IItem> items; // Dictionary to store items at specific coordinates - Changed to random locations later
        private Dictionary<(int, int), Enemy> enemies; // Dictionary to store enemies at specific coordinates


        public int Width { get; }
        public int Height { get; }

        // Constructor
        public Room(int height, int width)
        {
            Height = height;
            Width = width;
            grid = new CellType[height, width];
            items = new Dictionary<(int, int), IItem>();
            enemies = new Dictionary<(int, int), Enemy>();

            // Create an empty room
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    grid[y, x] = CellType.Empty;
                }
            }

            AddBoundaryWalls();
        }

        // This method adds walls around the room to make it better looking
        private void AddBoundaryWalls()
        {
            // Add top and bottom walls
            for (int x = 0; x < Width; x++)
            {
                grid[0, x] = CellType.Wall;           // Top wall - from 0 to width - 1
                grid[Height - 1, x] = CellType.Wall;  // Bottom wall - from height - 1 to width - 1
            }

            // Add left and right walls 
            for (int y = 1; y < Height - 1; y++)
            {
                grid[y, 0] = CellType.Wall;           // Left wall 
                grid[y, Width - 1] = CellType.Wall;   // Right wall 
            }
        }

        // This method sets the cell type at a specific location
        public void SetCell(int x, int y, CellType cellType)
        {
            if (x >= 0 && x < Width && y >= 0 && y < Height)
            {
                grid[y, x] = cellType;
            }
        }

        // This method returns the cell type at a specific location
        public CellType GetCellType(int x, int y)
        {
            if (x >= 0 && x < Width && y >= 0 && y < Height)
            {
                return grid[y, x];
            }

            return CellType.Wall; // Return wall if out of bounds
        }

        // Item methods
        // Place items from the list of items in the room
        public void PlaceItem(IItem item, int x, int y)
        {
            if (x >= 0 && x < Width && y >= 0 && y < Height && grid[y, x] != CellType.Wall)
            {
                items[(x, y)] = item;
                grid[y, x] = CellType.Item;
            }
        }

        public IItem GetItemAt(int x, int y)
        {
            if (items.TryGetValue((x, y), out IItem item))
            {
                return item;
            }

            return null;
        }

        public void RemoveItem(int x, int y)
        {
            if (items.ContainsKey((x, y)))
            {
                items.Remove((x, y));
                grid[y, x] = CellType.Empty;
            }
        }

        //Enemy methods
        public void AddEnemy(Enemy enemy, int x, int y)
        {
            if (x >= 0 && x < Width && y >= 0 && y < Height)
            {
                enemies[(x, y)] = enemy;
                SetCell(x, y, CellType.Enemy);
            }
        }

        // Get enemy at specific coordinates
        public Enemy GetEnemyAt(int x, int y)
        {
            // Check if the coordinates are within bounds - if not, return null
            if (enemies.TryGetValue((x, y), out Enemy enemy))
            {
                return enemy;
            }
            return null;
        }

        public void RemoveEnemy(int x, int y)
        {
            if (enemies.ContainsKey((x, y)))
            {
                enemies.Remove((x, y));
                SetCell(x, y, CellType.Empty);
            }
        }

        // Draw the room and the player
        public void Draw(Player player)
        {
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    if (x == player.X && y == player.Y)
                    {
                        Console.Write('@'); 
                    }
                    else if (GetCellType(x, y) == CellType.Enemy && enemies.TryGetValue((x, y), out Enemy enemy))
                    {
                        Console.Write(enemy.Symbol); // Enemy symbol
                    }
                    else
                    {
                        switch (grid[y, x])
                        {
                            case CellType.Empty:
                                Console.Write(' ');
                                break;
                            case CellType.Wall:
                                Console.Write('█');
                                break;
                            case CellType.Item:
                                // Get symbol for the item - First letter of the name
                                IItem item = GetItemAt(x, y);
                                Console.Write(item.Symbol);
                                break;
                            case CellType.Enemy:
                                Console.Write('E'); 
                                break;
                            default:
                                Console.Write(' ');
                                break;
                        }
                    }
                }
                Console.WriteLine();
            }
        }
    }
}
