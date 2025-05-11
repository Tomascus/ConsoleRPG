using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOD_RPG.KeyHandlers
{
    internal class EquipItemHandler : KeyHandler
    {
        public EquipItemHandler(Game game) : base(game) { }

        public override bool IsApplicable()
        {
            // Applicable only when the player has items in inventory
            return game.GetPlayer().Inventory.Count > 0;
        }

        public override bool Handle(ConsoleKeyInfo keyInfo)
        {
            // Handles numeric keys 1-9 for equipping items 
            if (keyInfo.Key >= ConsoleKey.D1 && keyInfo.Key <= ConsoleKey.D9)
            {
                int index = keyInfo.Key - ConsoleKey.D1;
                game.EquipItem(index);
                game.EndTurn();
                return true;
            }
            return nextHandler?.Handle(keyInfo) ?? false;
        }

        // Shows the action key for equipping items 
        public override ConsoleKey GetActionKey()
        {
            return ConsoleKey.D1; 
        }

        public override string GetActionDescription()
        {
            return "1-9: Equip/use item from inventory";
        }
    }
}
