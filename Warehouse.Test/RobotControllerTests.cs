using System;
using Ninject;
using NUnit.Framework;
using Warehouse.Interface;
using Warehouse.Model;

namespace Warehouse.Test

{
    [TestFixture]
    public class RobotControllerTests
    {
        private StandardKernel _kernel;
        private UnitTestDisplay _display;

        /// <summary>
        /// Standard input for spec
        /// </summary>
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
        /// Check against expected spec output
        /// </summary>
        [Test]
        public void TestStandardInput()
        {
            BindRobotControllerAndStart(STANDARD_INPUT);

            Assert.That(_display.Message.ToString() == STANDARD_EXPECTED_OUTPUT);
        }


        /// <summary>
        /// Check invalid input is handled in the expected way - Exception thrown & Display updated
        /// </summary>
        [Test]
        public void TestInvalidInputExpectedFail()
        {
            _kernel.Bind<IGrid>().To<Grid>()
                .InSingletonScope()
                .WithConstructorArgument("input", STANDARD_INPUT);

            _kernel.Bind<IRobotController<IRobot>>().To<RobotController<TestRobot>>()
                .InSingletonScope()
                .WithConstructorArgument("input", "***\r\n INVALID \r\n DATA \r\n ****");

            Assert.Throws<InvalidOperationException>(() => _kernel.Get<IRobotController<IRobot>>()
                .Start());

            Assert.That(_display.Message.ToString() == INVALID_INPUT_DATA_MSG);
        }

        /// <summary>
        /// Check robot stays at 0,0
        /// </summary>
        [Test]
        public void TestMinBoundsInput()
        {
            BindRobotControllerAndStart(MIN_BOUNDS_INPUT);

            Assert.That(_display.Message.ToString() == MIN_BOUNDS_EXPECTED_OUTPUT);
        }

        /// <summary>
        /// Check robot stays at Max Grid Size 5,5
        /// </summary>
        [Test]
        public void TestMaxBoundsInput()
        {
            BindRobotControllerAndStart(MAX_BOUNDS_INPUT);

            Assert.That(_display.Message.ToString() == MAX_BOUNDS_EXPECTED_OUTPUT);
        }

        /// <summary>
        /// Helper
        /// </summary>
        /// <param name="input"></param>
        private void BindRobotControllerAndStart(string input)
        {
            _kernel.Bind<IGrid>().To<Grid>()
                .InSingletonScope()
                .WithConstructorArgument("input", input);

            _kernel.Bind<IRobotController<IRobot>>().To<RobotController<TestRobot>>()
                .InSingletonScope()
                .WithConstructorArgument("input", input);

            _kernel.Get<IRobotController<IRobot>>()
                .Start();
        }
    }
}
