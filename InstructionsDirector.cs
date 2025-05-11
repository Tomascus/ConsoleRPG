using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOD_RPG
{
    // This class is used to direct the building of instructions 
    public class InstructionsDirector
    {
        private IInstructionsBuilder builder;

        public InstructionsDirector(IInstructionsBuilder builder)
        {
            this.builder = builder;
        }

        public void ChangeBuilder(IInstructionsBuilder builder)
        {
            this.builder = builder;
        }

        public void BuildCompleteInstructions()
        {
            builder.BuildBasicControls();
            builder.BuildMovementInstructions();
            builder.BuildItemInstructions();
            builder.BuildWeaponInstructions();
            builder.BuildEnemyInstructions();
            builder.BuildInventoryInstructions();
        }

        public void BuildBasicInstructions()
        {
            builder.BuildBasicControls();
            builder.BuildMovementInstructions();
        }

        public string GetInstructions()
        {
            return builder.GetResult();
        }

        // This method uses the GameDisplay to display the instructions
        public void DisplayInstructions()
        {
            // Get the instructions string to display
            string instructions = GetInstructions();
            // Use the GameDisplay singleton for display 
            GameDisplay.Instance.DisplayInstructions(instructions);
        }
    }
}
