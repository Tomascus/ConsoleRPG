using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOD_RPG.KeyHandlers
{
    internal class MovementHandler : KeyHandler
    {
        // This handler is responsible for moving the player character in the game
        public MovementHandler(Game game) : base(game) { } // This constructor initializes base class with the game instance 
        
        public override bool IsApplicable()
        {
            return true;
        }

        // Based on the key pressed, it will move the player in the corresponding direction
        public override bool Handle(ConsoleKeyInfo keyInfo)
        {
            switch (keyInfo.Key)
            {
                case ConsoleKey.W:
                    game.MovePlayer(0, -1);
                    return true;
                case ConsoleKey.S:
                    game.MovePlayer(0, 1);
                    return true;
                case ConsoleKey.A:
                    game.MovePlayer(-1, 0);
                    return true;
                case ConsoleKey.D:
                    game.MovePlayer(1, 0);
                    return true;
                default:
                    // If nothing relevant is pressed, go to the next handler (chains)
                    return nextHandler?.Handle(keyInfo) ?? false;
            }
        }

        // Shows just the first movement key (W) as the action key - more not needed
        public override ConsoleKey GetActionKey()
        {
            return ConsoleKey.W; // Representing all movement keys
        }

        public override string GetActionDescription()
        {
            return "W/A/S/D: Move in four directions";
        }
    }
}
