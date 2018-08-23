using System;
using Ninject;
using NUnit.Framework;
using Rhino.Mocks;
using Warehouse.Interface;
using Warehouse.Model;

namespace Warehouse.Test

{
    [TestFixture]
    public class RobotTests
    {
        private StandardKernel _kernel;
        private UnitTestDisplay _display;

        const string STANDARD_INPUT = "5 5\r\n1 2 N\r\n<^<^<^<^^\r\n3 3 E\r\n^^>^^>^>>^";
        private const string STANDARD_EXPECTED_OUTPUT = "1 3 N\r\n5 1 E\r\n";

        const string MAX_BOUNDS_INPUT = "5 5\r\n1 2 N\r\n^^^^^^^^^^^^>^^^^^^^^^^^^^";
        private const string MAX_BOUNDS_EXPECTED_OUTPUT = "5 5 E\r\n";

        const string MIN_BOUNDS_INPUT = "5 5\r\n1 2 N\r\n^^^^^^^^^^^^>^^^^^^^^^^^^^>^^^^^^^^^^^^^^>^^^^^^^^^^^^^^^^^^";
        private const string MIN_BOUNDS_EXPECTED_OUTPUT = "0 0 W\r\n";

        const string INVALID_INPUT_DATA_MSG = "Unable to process controller INPUT DATA - check input\r\n";


        [OneTimeSetUp]
        public void OneTimeSetUp()
        {

        }

        /// <summary>
        /// Per-Test Setup
        /// </summary>
        [SetUp]
        public void Setup()
        {
            _kernel = new StandardKernel();
            _display = new UnitTestDisplay();
            _kernel.Bind<IDisplay>().ToMethod(x => _display).InThreadScope();
        }

        /// <summary>
        /// Per-Test TearDown 
        /// </summary>
        [TearDown]
        public void TearDown()
        {
            // Ensure we dispose of the entire object graph for each test - simplest way is to Dispose
            _kernel.Dispose();
        }

        /// <summary>
        /// Individual robot test - move with no bounds
        /// </summary>
        [Test]
        public void TestCheckTestRobotMovement()
        {
            var robot = new TestRobot();

            robot.LoadInstructions("1 2 N", "^^^^^^^^^^^^>^^^^^^^^^^^^^");

            foreach (var instruction in robot.MovementInstructions.ToCharArray())
            {
                robot.Move(instruction);
            }

            Assert.That(robot.ToString() == "14 14 E");
        }

        /// <summary>
        /// Individual robot test fails - Can't use a Production Robot as the API is yet to be implemented
        /// </summary>
        [Test]
        public void TestCheckProductionRobotExpectedFail()
        {
            var robot = new ProductionRobot();

            robot.LoadInstructions("1 2 N", "^");

            Assert.Throws<InvalidOperationException>((() =>
                    {
                        foreach (var instruction in robot.MovementInstructions.ToCharArray())
                        {
                            robot.Move(instruction);
                        }
                    }
                ));
        }

        /// <summary>
        /// An example of how mocking would work to test Production Robot (which requires outside API call, not yet implemented)
        /// </summary>
        [Test]
        public void TestCheckProductionRobotMock()
        {
            var mockRobot = MockRepository.GeneratePartialMock<ProductionRobot>();

            // Tell the mock Production robot not to fail on ApiRobotMove
            mockRobot.Stub(r => r.ApiRobotMove())
                .Return(true);

            mockRobot.LoadInstructions("1 2 N", "^^^^^^^^^^^^>^^^^^^^^^^^^^");

            foreach (var instruction in mockRobot.MovementInstructions.ToCharArray())
            {
                mockRobot.Move(instruction);
            }

            Assert.That(mockRobot.ToString() == "14 14 E");
        }

    }
}
