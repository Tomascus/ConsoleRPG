using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOD_RPG.KeyHandlers
{
    internal class DropAllItemsHandler : KeyHandler
    {
        public DropAllItemsHandler(Game game) : base(game) { }

        public override bool IsApplicable()
        {
            Player player = game.GetPlayer();
            return player.Inventory.Count > 0;
        }

        public override bool Handle(ConsoleKeyInfo keyInfo)
        {
            if (keyInfo.Key == ConsoleKey.X)
            {
                game.DropAllItems();
                game.EndTurn(); 
                return true;
            }
            return nextHandler?.Handle(keyInfo) ?? false;
        }

        public override ConsoleKey GetActionKey()
        {
            return ConsoleKey.X;
        }

        public override string GetActionDescription()
        {
            return "X: Drop all items";
        }
    }
}
