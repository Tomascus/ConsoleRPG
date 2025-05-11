using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOD_RPG.KeyHandlers
{
    internal abstract class KeyHandler : IKeyHandler
    {
        public IKeyHandler nextHandler; // Next handler in the chain
        protected Game game;

        // Constructor with game reference to access game state
        public KeyHandler(Game game)
        {
            this.game = game;
        }

        // Sets the next handler in the chain 
        public void SetNext(IKeyHandler handler)
        {
            nextHandler = handler;
        }

        // Passes the key input to the next handler in the chain - virtual because it can be overridden for each handler
        // This method is called when a key is pressed 
        public virtual bool Handle(ConsoleKeyInfo keyInfo)
        {
            if (nextHandler != null)
            {
                return nextHandler.Handle(keyInfo);
            }
            return false;
        }

        // This checks if the handler is relevant and should be active
        public abstract bool IsApplicable();

        // Shows description of actions in UI
        public abstract string GetActionDescription();

        // Shows the key that triggers the action in UI
        public abstract ConsoleKey GetActionKey();
    }
}
