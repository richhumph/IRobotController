namespace Warehouse.Interface
{
    public interface IWarehouse 
    {
        IRobotController<IRobot> RobotController { get; }
    }
}