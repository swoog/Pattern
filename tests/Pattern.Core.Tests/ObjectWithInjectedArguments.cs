using Pattern.Core.Tests.Example;

namespace Pattern.Core.Tests
{
    using Pattern.Core.Tests.Fakes;

    public class ObjectWithInjectedArguments : ObjectWithArguments
    {
        public ElectricMotor ElectricMotor { get; set; }

        public ObjectWithInjectedArguments(int i, ElectricMotor electricMotor, string stringValue)
            : base(i, stringValue)
        {
            this.ElectricMotor = electricMotor;
        }
    }
}