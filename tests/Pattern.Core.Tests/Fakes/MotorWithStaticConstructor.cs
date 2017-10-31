using Pattern.Core.Tests.Example;

namespace Pattern.Core.Tests.Fakes
{
    public class MotorWithStaticConstructor : IMotor
    {
        public static IMotor Motor;

        static MotorWithStaticConstructor()
        {
            Motor = new MotorWithStaticConstructor();
        }
    }
}