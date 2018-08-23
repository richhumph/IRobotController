using System;
using System.Drawing;
using System.Linq;
using Warehouse.Interface;

namespace Warehouse.Model
{
    public class Grid : IGrid
    {
        const string INVALID_INPUT_DATA_MSG = "Unable to process grid INPUT DATA - check input";

        public Grid(string input)
        {
            try
            {
                var commandLines = input.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                var gridCommand = commandLines
                    .First()
                    .Split(' ')
                    .Select(c => Convert.ToInt32(c))
                    .ToArray();

                UpperBounds = new Point(gridCommand[0], gridCommand[1]);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(INVALID_INPUT_DATA_MSG, ex);
            }
        }

        public Point UpperBounds { get; private set; }
    }
}