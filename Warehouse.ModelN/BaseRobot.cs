using System;
using System.Drawing;
using System.Linq;
using Warehouse.Interface;

namespace Warehouse.Model
{
    public abstract class Robot : IRobot
    {
        private const char FORWARD = '^';
        private const char RIGHT = '>';
        private const char LEFT = '<';

        /// <summary>
        /// Robot holds own movement instructions, once validated
        /// </summary>
        public string MovementInstructions { get; private set; }

        /// <summary>
        /// Robot tracks own coords
        /// </summary>
        public Point CurrentCoordinates { get; private set; }

        /// <summary>
        /// Facing is private
        /// </summary>
        private CompassDirection Facing { get; set; }

        /// <summary>
        /// Enum can be private currently
        /// </summary>
        private enum CompassDirection
        {
            N, E, S, W
        }
        
        /// <summary>
        /// Instructions loaded by controller, validated by robot
        /// </summary>
        /// <param name="startLocation"></param>
        /// <param name="movementInstructions"></param>
        public void LoadInstructions(string startLocation, string movementInstructions)
        {
            MovementInstructions = movementInstructions;

            SetStartLocation(startLocation);
        }

        private void SetStartLocation(string startLocation)
        {
            try
            {
                var s = startLocation
                    .Split(' ')
                    .ToArray();

                CurrentCoordinates = new Point
                {
                    X = Convert.ToInt32(s[0]),
                    Y = Convert.ToInt32(s[1])
                };

                Facing = (CompassDirection)Enum.Parse(typeof(CompassDirection), s[2], true);

            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Violation of 3rd Law - START DATA - check input", ex);
            }
            
        }

        /// <summary>
        /// Move takes a char instruction - additional move types can easily be added in future
        /// </summary>
        /// <param name="move"></param>
        /// <returns></returns>
        public bool Move(char move)
        {
            try
            {
                switch (move)
                {
                    case FORWARD:
                        GoForward();
                        break;
                    case RIGHT:
                        TurnRight();
                        break;
                    case LEFT:
                        TurnLeft();
                        break;
                        
                    // Ignore invalid instructions for the time being
                }

                return true;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Violation of 3rd Law - MOVE INSTRUCTION - check input", ex);
            }
        }

        /// <summary>
        /// Return the coordinates of the  next location when moving forwards 
        /// </summary>
        /// <param name="instruction"></param>
        /// <returns></returns>
        public Point GetNextCoordinates(char instruction)
        {
            var newCoordinates = CurrentCoordinates;
            
            if (instruction == FORWARD)
            {
                switch (Facing)
                {
                    case CompassDirection.N:
                        newCoordinates.Y++;
                        break;
                    case CompassDirection.E:
                        newCoordinates.X++;
                        break;
                    case CompassDirection.S:
                        newCoordinates.Y--;
                        break;
                    case CompassDirection.W:
                        newCoordinates.X--;
                        break;
                }
            }

            return newCoordinates;
        }

        protected virtual void GoForward()
        {
            CurrentCoordinates = GetNextCoordinates(FORWARD);
        }

        protected virtual void TurnLeft()
        {
            Facing = Facing > 0 ? Facing - 1 : Facing = CompassDirection.W;
        }

        protected virtual void TurnRight()
        {
            Facing = (int) Facing < 3 ? Facing + 1 : Facing = CompassDirection.N;
        }

        public override string ToString()
        {
            return $"{CurrentCoordinates.X} {CurrentCoordinates.Y} {Enum.GetName(typeof(CompassDirection),Facing)}";
        }

    }
}