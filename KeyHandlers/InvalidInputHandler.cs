using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOD_RPG.KeyHandlers
{
    internal class InvalidInputHandler : KeyHandler
    {
        public InvalidInputHandler(Game game) : base(game) { }

        public override bool IsApplicable()
        {
            return true;
        }

        public override bool Handle(ConsoleKeyInfo keyInfo)
        {
            // This is the end of the chain - display invalid input message
            GameDisplay.Instance.AddMessage("Invalid key... Press a valid key.");
            return true;
        }

        public override ConsoleKey GetActionKey()
        {
            // No specific action key for invalid input
            return ConsoleKey.NoName;
        }

        public override string GetActionDescription()
        {
            // No description for invalid input
            return "";
        }
    }
}
