using Pattern.Core.Tests.Example;

namespace Pattern.Core.Tests.Fakes
{
    using System.Collections.Generic;

    public class ComplexListClass
    {
        public IList<IMotor> SimpleClasses { get; set; }

        public ComplexListClass(IList<IMotor> simpleClasses)
        {
            this.SimpleClasses = simpleClasses;
        }
    }
}