using OOD_RPG;
using System;
using System.Text;

// This interface gives the instructions for the game on how to play
// It is used by the Director to build the instructions
public interface IInstructionsBuilder
{
    void BuildBasicControls();
    void BuildMovementInstructions();
    void BuildItemInstructions();
    void BuildWeaponInstructions();
    void BuildEnemyInstructions();
    void BuildInventoryInstructions();
    string GetResult();
}