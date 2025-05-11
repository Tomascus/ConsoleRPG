using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOD_RPG.KeyHandlers
{
    //Interface for handling key inputs in the game - provides methods for setting up a chain of responsibility pattern
    internal interface IKeyHandler
    {
        void SetNext(IKeyHandler handler);
        bool Handle(ConsoleKeyInfo keyInfo);
        bool IsApplicable();
        string GetActionDescription();
        ConsoleKey GetActionKey();
    }
}
