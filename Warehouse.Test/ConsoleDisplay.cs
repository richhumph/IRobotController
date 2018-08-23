using System;
using System.Text;
using Warehouse.Interface;

namespace Warehouse.Test
{
    public class UnitTestDisplay : IDisplay
    {
        public StringBuilder Message = new StringBuilder();

        public void Write(string message)
        {
            Message.AppendLine(message);
        }
    }
}