using System;
using System.Drawing;
using System.Linq;
using Warehouse.Interface;

namespace Warehouse.Model
{
    public class ProductionRobot : Robot
    {
        public ProductionRobot()
        {

        }

        protected override void GoForward()
        {
            if (ApiRobotMove())
            {
                base.GoForward();
            }
        }

        protected override void TurnLeft()
        {
            if (ApiRobotMove())
            {
                base.TurnLeft();
            }
        }

        protected override void TurnRight()
        {
            if (ApiRobotMove())
            {
                base.TurnRight();
            }
        }

        /// <summary>
        /// Production Robot would make an API call to activate actuators etc. 
        /// </summary>
        public virtual bool ApiRobotMove()
        {
            // TODO: Integrate with real world robot actuator, eg via a Repository

            throw new NotImplementedException();
        }

    }
}