using System.Collections.Generic;

namespace Pattern.Core.Tests.Example
{
    public class MotorsChoice
    {
        public IList<IMotor> AllMotors { get; set; }

        public MotorsChoice(IList<IMotor> allMotors)
        {
            this.AllMotors = allMotors;
        }
    }
}