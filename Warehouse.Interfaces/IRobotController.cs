namespace Warehouse.Interface
{
    public interface IRobotController<in TRobot> where TRobot : IRobot
    {
        void Start();
    }
}