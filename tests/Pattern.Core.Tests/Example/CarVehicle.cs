namespace Pattern.Core.Tests.Example
{
    public class CarVehicle
    {
        public CarVehicle(IMotor motor)
        {
            this.Motor = motor;
        }

        public IMotor Motor { get; set; }
    }
}