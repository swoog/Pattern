namespace Pattern.Core.Tests.Example
{
    public class ElectricCarVehicle
    {
        public ElectricCarVehicle(ElectricMotor motor)
        {
            this.Motor = motor;
        }

        public ElectricMotor Motor { get; set; }
    }
}