using System.Collections.Generic;

namespace Pattern.Core.Tests.Example
{
    public class EnumerableMotorChoice
    {
        public IEnumerable<IMotor> AllMotors { get; set; }

        public EnumerableMotorChoice(IEnumerable<IMotor> allMotors)
        {
            this.AllMotors = allMotors;
        }
    }
}