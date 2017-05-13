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

    public class ComplexEnumerableClass
    {
        public IEnumerable<ISimpleClass> SimpleClasses { get; set; }

        public ComplexEnumerableClass(IEnumerable<ISimpleClass> simpleClasses)
        {
            this.SimpleClasses = simpleClasses;
        }
    }
    public class ComplexEnumerableClassWithNotImplementedInterface
    {
        public IEnumerable<INotimplementedInterface> SimpleClasses { get; set; }

        public ComplexEnumerableClassWithNotImplementedInterface(IEnumerable<INotimplementedInterface> simpleClasses)
        {
            this.SimpleClasses = simpleClasses;
        }
    }

    public interface INotimplementedInterface
    {
    }
}