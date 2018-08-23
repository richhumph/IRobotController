using Ninject.Modules;
using Warehouse.Model;
using Warehouse.Interface;

namespace Warehouse.View
{
    internal class WarehouseModule : NinjectModule
    {
        private readonly string _input;

        public WarehouseModule(string input)
        {
            _input = input;
        }

        public override void Load()
        {
            // Singletons
            Bind<IDisplay>().To<ConsoleDisplay>().InSingletonScope();
            Bind<IGrid>().To<Grid>()
                .InSingletonScope()
                .WithConstructorArgument("input", _input);

            Bind<IWarehouse>().To<Model.Warehouse>()
                .InSingletonScope();

            // Pass the desired input in via constructor injection here. Note we're using Test, not Production robot objects
            Bind<IRobotController<IRobot>>().To<RobotController<TestRobot>>()
                .InSingletonScope()
                .WithConstructorArgument("input", _input);

            
        }
    }
}