using System;
using System.Collections.Generic;
using System.Drawing;
using Warehouse.Interface;

namespace Warehouse.Model
{
    public class RobotController<TRobot> : IRobotController<IRobot> where TRobot : IRobot, new()
    {
        const string INVALID_INPUT_DATA_MSG = "Unable to process controller INPUT DATA - check input";

        private readonly List<TRobot> _robots = new List<TRobot>();
        private readonly IDisplay _display;
        private readonly IGrid _grid;

        public RobotController(IDisplay display, IGrid grid, string input)
        {
            _display = display;
            _grid = grid;

            try
            {
                var commandLines = input.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                for (int i = 1; i < commandLines.Length; i += 2)
                {
                    AddRobot(commandLines[i], commandLines[i + 1]);
                }
            }
            catch (Exception ex)
            {
                _display.Write(INVALID_INPUT_DATA_MSG);

                throw new InvalidOperationException(INVALID_INPUT_DATA_MSG, ex);
            }
        }

        /// <summary>
        /// Explicitly start operations, as opposed to entire system firing up via constructor injection 
        /// </summary>
        public void Start()
        {
            foreach (var robot in _robots)
            {
                foreach (var instruction in robot.MovementInstructions.ToCharArray())
                {
                    if (CheckBounds(robot.GetNextCoordinates(instruction)))
                    {
                        robot.Move(instruction);
                    }

                    // TODO: Phase 2 Requirement - We can easily add in robot/warehouse collision detection code here
                }

                _display.Write(robot.ToString());
            }
        }

        /// <summary>
        /// Check robot not about to move outside the Grid bounds
        /// </summary>
        /// <param name="nextMove"></param>
        /// <returns></returns>
        private bool CheckBounds(Point nextMove)
        {
            return nextMove.X >= 0
                    && nextMove.Y >= 0 
                    && nextMove.X <= _grid.UpperBounds.X
                    && nextMove.Y <= _grid.UpperBounds.Y;
        }

        /// <summary>
        /// Robot Factory
        /// </summary>
        /// <param name="startLocation"></param>
        /// <param name="moveInstruction"></param>
        void AddRobot(string startLocation, string moveInstruction)
        {
            var robot = new TRobot();

            robot.LoadInstructions(startLocation, moveInstruction);

            _robots.Add(robot);
        }

    }
}