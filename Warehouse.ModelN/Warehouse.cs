using Warehouse.Interface;

namespace Warehouse.Model
{
    public class Warehouse : IWarehouse
    {
        // TODO - Phase 2 will add stock etc. 

        public IRobotController<IRobot> RobotController { get; }
        

        public Warehouse(IRobotController<IRobot> robotController)
        {
            RobotController = robotController;
        }

    }
}