using System;

namespace Pattern.Core.Tests.Example
{
    public class CarVehicle
    {
        static CarVehicle()
        {
            // For constructor selection when have static constructor
        }

        internal CarVehicle(IMotor motor1, IMotor motor2)
        {
            // For constructor selection when constructor is internal
        }

        public CarVehicle(bool b, bool b2)
        {
            // For constructor selection when constructor have bool
        }

        public CarVehicle(Func<int> f)
        {
            // For constructor selection when constructor have Func
        }

        public CarVehicle(IntPtr pt1, IntPtr pt2)
        {
            // For constructor selection when constructor have ptr
        }

        public CarVehicle(IMotor motor)
        {
            this.Motor = motor;
        }

        public IMotor Motor { get; set; }
    }
}