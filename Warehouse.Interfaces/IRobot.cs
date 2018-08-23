using System.Drawing;

namespace Warehouse.Interface
{
    public interface IRobot
    {
        void LoadInstructions(string startLocation, string movementInstructions);

        string MovementInstructions { get; }

        bool Move(char movementInstruction);

        Point GetNextCoordinates(char instruction);
    }
}