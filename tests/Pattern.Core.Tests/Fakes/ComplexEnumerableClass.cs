using System.Collections.Generic;
using Pattern.Core.Tests.Example;

namespace Pattern.Core.Tests.Fakes
{
    public class ComplexEnumerableClass
    {
        public IEnumerable<IMotor> SimpleClasses { get; set; }

        public ComplexEnumerableClass(IEnumerable<IMotor> simpleClasses)
        {
            this.SimpleClasses = simpleClasses;
        }
    }
}