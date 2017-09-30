using Pattern.Core.Tests.Example;

namespace Pattern.Core.Tests.Fakes
{
    public class SimpleClassWithStaticConstructorAndInjectedConstructor
    {
        public static IMotor MotorStatic;
        private readonly IMotor motor;

        public SimpleClassWithStaticConstructorAndInjectedConstructor(IMotor motor)
        {
            this.motor = motor;
        }

        static SimpleClassWithStaticConstructorAndInjectedConstructor()
        {
            MotorStatic = new MotorWithStaticConstructor();
        }
    }
}