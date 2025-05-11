using OOD_RPG.Decorators;
using OOD_RPG.Items;
using OOD_RPG;
using System;
using System.Collections.Generic;

namespace OOD_RPG
{
    // This interface defines the properties of the dungeon builder - the director
    internal interface IDungeonBuilder
    {
        void InitializeDungeon(int height, int width, bool filled);

        // Add borders around the dungeon for better visuals
        void BuildBorders();

        // Additional strategies
        void BuildPaths(int pathCount, int pathLength);
        void BuildChambers(int chamberCount, int minSize, int maxSize);
        void BuildCentralRoom(int width, int height);
        void connectDungeon(); 
        void BuildItems(int itemCount);
        void BuildWeapons(int weaponCount);
        void BuildModifiedWeapons(int weaponCount);
        void BuildPotions(int potionCount);
        void BuildEnemies(int enemyCount);

        // Get the built dungeon
        Room GetResult();
    }
}