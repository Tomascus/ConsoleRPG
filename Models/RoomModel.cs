using System;
using System.Collections.Generic;

namespace OOD_RPG.Models
{
    /// <summary>
    /// Model class representing the game map
    /// </summary>
    public class RoomModel
    {
        // Grid dimensions
        public int Width { get; }
        public int Height { get; }

        // Cell types for each position
        private CellType[,] grid;

        /// <summary>
        /// Create a new empty room with the specified dimensions
        /// </summary>
        public RoomModel(int height, int width)
        {
            Height = height;
            Width = width;
            grid = new CellType[height, width];

            // Initialize with empty cells
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    grid[y, x] = CellType.Empty;
                }
            }

            // Add boundaries
            AddBoundaries();
        }

        /// <summary>
        /// Add wall boundaries around the room
        /// </summary>
        private void AddBoundaries()
        {
            // Top and bottom walls
            for (int x = 0; x < Width; x++)
            {
                grid[0, x] = CellType.Wall;
                grid[Height - 1, x] = CellType.Wall;
            }

            // Left and right walls
            for (int y = 1; y < Height - 1; y++)
            {
                grid[y, 0] = CellType.Wall;
                grid[y, Width - 1] = CellType.Wall;
            }
        }

        /// <summary>
        /// Set the cell type at a specific location
        /// </summary>
        public void SetCell(int x, int y, CellType cellType)
        {
            if (IsValidPosition(x, y))
            {
                grid[y, x] = cellType;
            }
        }

        /// <summary>
        /// Get the cell type at a specific location
        /// </summary>
        public CellType GetCellType(int x, int y)
        {
            if (IsValidPosition(x, y))
            {
                return grid[y, x];
            }

            return CellType.Wall; // Default to wall for invalid positions
        }

        /// <summary>
        /// Check if a position is within the room boundaries
        /// </summary>
        public bool IsValidPosition(int x, int y)
        {
            return x >= 0 && x < Width && y >= 0 && y < Height;
        }

        /// <summary>
        /// Convert an existing Room to a RoomModel
        /// </summary>
        public static RoomModel FromRoom(Room existingRoom)
        {
            var model = new RoomModel(existingRoom.Height, existingRoom.Width);

            // Copy cell types from existing room
            for (int y = 0; y < existingRoom.Height; y++)
            {
                for (int x = 0; x < existingRoom.Width; x++)
                {
                    model.SetCell(x, y, existingRoom.GetCellType(x, y));
                }
            }

            return model;
        }
    }

    /// <summary>
    /// Types of cells in the room grid
    /// </summary>
    public enum CellType
    {
        Empty,
        Wall,
        Player,
        Item,
        Enemy
    }
}