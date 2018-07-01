using System.Collections.Generic;

namespace Pattern.Core.Tests.Fakes
{
    public class ComplexEnumerableClassWithNotImplementedInterface
    {
        public IEnumerable<INotimplementedInterface> SimpleClasses { get; set; }

        public ComplexEnumerableClassWithNotImplementedInterface(IEnumerable<INotimplementedInterface> simpleClasses)
        {
            this.SimpleClasses = simpleClasses;
        }
    }
}