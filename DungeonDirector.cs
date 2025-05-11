using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOD_RPG
{
    // This class is responsible for direction of creation of dungeons - it uses a builder to create the dungeon and then returns the result
    // It also includes presets for dungeons
    internal class DungeonDirector
    {
        private IDungeonBuilder builder;

        public DungeonDirector(IDungeonBuilder builder)
        {
            this.builder = builder;
        }

        // Change the builder used to create the dungeon - this allows for different types of dungeons to be created
        public void ChangeBuilder(IDungeonBuilder builder)
        {
            this.builder = builder;
        }

        // Dungeon presets
        public void BuildEmptyRoom(int height, int width)
        {
            builder.InitializeDungeon(height, width, false);
        }

        public void BuildMazeWithItems(int height, int width)
        {
            builder.InitializeDungeon(height, width, true);
            builder.BuildPaths(40, 30);
            builder.connectDungeon();
            builder.BuildItems(10);
        }

        public void BuildComplexDungeon(int height, int width)
        {
            builder.InitializeDungeon(height, width, true);
            builder.BuildPaths(25, 20);
            builder.BuildChambers(4, 4, 7);
            builder.BuildCentralRoom(10, 8);
            builder.connectDungeon();
            builder.BuildItems(5);
            builder.BuildWeapons(5);
            builder.BuildModifiedWeapons(3);
            builder.BuildPotions(5);
            builder.BuildEnemies(8);
        }

        // Custom dungeon building
        public void BuildCustomDungeon(int height, int width, List<string> strategies)
        {
            // Determine if dungeon should start filled or empty
            bool isFilled = strategies.Contains("filled");

            // Initialize the dungeon - start with a filled or empty room
            builder.InitializeDungeon(height, width, isFilled);

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
        }

        // This method returns the built dungeon - gets the result of the builder
        internal Room GetDungeon()
        {
            return builder.GetResult();
        }
    }
}
