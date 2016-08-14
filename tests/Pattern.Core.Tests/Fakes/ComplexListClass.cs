namespace Pattern.Core.Tests.Fakes
{
    using System.Collections.Generic;

    public class ComplexListClass
    {
        public IList<ISimpleClass> SimpleClasses { get; set; }

        public ComplexListClass(IList<ISimpleClass> simpleClasses)
        {
            this.SimpleClasses = simpleClasses;
        }
    }
}