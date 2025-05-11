using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOD_RPG.KeyHandlers
{
    internal class ExitHandler : KeyHandler
    {
        public ExitHandler(Game game) : base(game) { }

        public override bool IsApplicable()
        {
            return true;
        }

        public override bool Handle(ConsoleKeyInfo keyInfo)
        {
            if (keyInfo.Key == ConsoleKey.Escape)
            {    
                game.ExitGame();
                return true;
            }
            return nextHandler?.Handle(keyInfo) ?? false;
        }

        public override ConsoleKey GetActionKey()
        {
            return ConsoleKey.Escape;
        }

        public override string GetActionDescription()
        {
            return "ESC: Exit game";
        }
    }
}
