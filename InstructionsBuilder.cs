using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOD_RPG
{
    // This class is used to build the instructions for the game based on the dungeon layout
    internal class InstructionsBuilder : IInstructionsBuilder
    {
        private StringBuilder instructions = new StringBuilder();
        private Room dungeon;

        public InstructionsBuilder(Room dungeon)
        {
            this.dungeon = dungeon;

            // Add a title for the instructions
            instructions.AppendLine("=== GAME INSTRUCTIONS ===");
            instructions.AppendLine();
        }

        public void BuildBasicControls()
        {
            instructions.AppendLine("Basic Controls:");
            instructions.AppendLine("- ESC: Exit the game");
            instructions.AppendLine();
        }

        public void BuildMovementInstructions()
        {
            instructions.AppendLine("Movement Controls:");
            instructions.AppendLine("- W: Move up");
            instructions.AppendLine("- S: Move down");
            instructions.AppendLine("- A: Move left");
            instructions.AppendLine("- D: Move right");
            instructions.AppendLine();
        }

        public void BuildItemInstructions()
        {
            // Check if dungeon has items
            if (DungeonHasItems())
            {
                instructions.AppendLine("Item Controls:");
                instructions.AppendLine("- E: Pick up item from the ground");
                instructions.AppendLine("- Q: Drop item from inventory");
                instructions.AppendLine();
            }
        }

        public void BuildWeaponInstructions()
        {
            // Check if dungeon has weapons
            if (DungeonHasWeapons())
            {
                instructions.AppendLine("Weapon Controls:");
                instructions.AppendLine("- 1-9: Equip item from inventory");
                instructions.AppendLine("- When equipping a weapon, you will be asked to choose left (L) or right (R) hand");
                instructions.AppendLine("- Two-handed weapons require both hands to be free");
                instructions.AppendLine();
            }
        }

        public void BuildEnemyInstructions()
        {
            // Check if dungeon has enemies
            if (DungeonHasEnemies())
            {
                instructions.AppendLine("Enemy Interaction:");
                instructions.AppendLine("- Enemies can be found throughout the dungeon");
                instructions.AppendLine("- Combat system will be implemented in future updates");
                instructions.AppendLine();
            }
        }

        public void BuildInventoryInstructions()
        {
            // Always include inventory instructions
            instructions.AppendLine("Inventory Information:");
            instructions.AppendLine("- Your inventory displays items you are carrying");
            instructions.AppendLine("- You can equip items in your left and right hands");
            instructions.AppendLine("- Some items have special effects when equipped");
            instructions.AppendLine();
        }

        private bool DungeonHasItems()
        {
            // Check if the dungeon has any items
            for (int y = 0; y < dungeon.Height; y++)
            {
                for (int x = 0; x < dungeon.Width; x++)
                {
                    if (dungeon.GetCellType(x, y) == CellType.Item)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private bool DungeonHasWeapons()
        {
            for (int y = 0; y < dungeon.Height; y++)
            {
                for (int x = 0; x < dungeon.Width; x++)
                {
                    if (dungeon.GetCellType(x, y) == CellType.Item)
                    {
                        IItem item = dungeon.GetItemAt(x, y);
                        if (item is Weapon || item is IDamageItems)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        private bool DungeonHasEnemies()
        {
            // PLACEHOLDER FOR NOW - ENEMIES ARE STATIC WITHOUT ATTRIBUTES OR INTERACTIONS
            return false;
        }

        // Return the instructions as a string
        public string GetResult()
        {
            return instructions.ToString();
        }
    }
}
