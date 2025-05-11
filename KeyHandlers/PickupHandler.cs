using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOD_RPG.KeyHandlers
{
    internal class PickupHandler : KeyHandler
    {
        public PickupHandler(Game game) : base(game) { }

        // Relevant only if there is an item under the player
        public override bool IsApplicable()
        {
            Room room = game.GetRoom();
            Player player = game.GetPlayer();
            return room.GetItemAt(player.X, player.Y) != null;
        }

        public override bool Handle(ConsoleKeyInfo keyInfo)
        {
            if (keyInfo.Key == ConsoleKey.E)
            {
                game.PickUpItem();
                game.EndTurn();
                return true;
            }
            return nextHandler?.Handle(keyInfo) ?? false; // pass to next handler
        }

        public override ConsoleKey GetActionKey()
        {
            return ConsoleKey.E;
        }

        public override string GetActionDescription()
        {
            return "E: Pick up item";
        }
    }
}
