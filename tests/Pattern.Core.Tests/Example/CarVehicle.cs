using System;

namespace Pattern.Core.Tests.Example
{
    public class CarVehicle
    {
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