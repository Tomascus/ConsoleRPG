using OOD_RPG.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOD_RPG.KeyHandlers
{
    internal class DrinkPotionHandler : KeyHandler
    {
        public DrinkPotionHandler(Game game) : base(game) { }

        public override bool IsApplicable()
        {
            // Appliable only when the player has potions in inventory
            Player player = game.GetPlayer();
            return player.Inventory.Any(item => item is Potion);
        }

        public override bool Handle(ConsoleKeyInfo keyInfo)
        {
            if (keyInfo.Key == ConsoleKey.P)
            {
                game.DrinkPotion();
                return true;
            }
            return nextHandler?.Handle(keyInfo) ?? false;
        }

        public override ConsoleKey GetActionKey()
        {
            return ConsoleKey.P;
        }

        public override string GetActionDescription()
        {
            return "P: Drink a potion";
        }
    }
}
