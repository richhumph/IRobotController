using System;
using Ninject;
using Ninject.Modules;
using Warehouse.Interface;

namespace Warehouse.View
{
    class Program
    {
        static void Main(string[] args)
        {
            // Supply input data
            var kernel = new StandardKernel(new NinjectModule[] 
                {new WarehouseModule("5 5\r\n1 2 N\r\n<^<^<^<^^\r\n3 3 E\r\n^^>^^>^>>^") });

            var warehouse = kernel.Get<IWarehouse>();

            // Explicit start mechanism 
            warehouse.RobotController.Start();

            Console.ReadLine();
        }
    }
}
