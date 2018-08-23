using System;
using Warehouse.Interface;

namespace Warehouse.Model
{
    public class ConsoleDisplay : IDisplay
    {
        public void Write(string message)
        {
            Console.WriteLine(message);
        }
    }
}