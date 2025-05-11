using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOD_RPG.KeyHandlers
{
    internal class DropItemHandler : KeyHandler
    {
        public DropItemHandler(Game game) : base(game) { }

        public override bool IsApplicable()
        {
            // Applicable only when the player has items in inventory
            return game.GetPlayer().Inventory.Count > 0;
        }

        public override bool Handle(ConsoleKeyInfo keyInfo)
        {
            if (keyInfo.Key == ConsoleKey.Q)
            {
                game.DropItem();
                game.EndTurn();
                return true;
            }
            return nextHandler?.Handle(keyInfo) ?? false;
        }

        public override ConsoleKey GetActionKey()
        {
            return ConsoleKey.Q;
        }

        public override string GetActionDescription()
        {
            return "Q: Drop item";
        }
    }
}
