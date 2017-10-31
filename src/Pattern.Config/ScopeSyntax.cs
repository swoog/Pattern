using System;
using Pattern.Core.Interfaces;
using Pattern.Core.Interfaces.Factories;

namespace Pattern.Config
{
    public class ScopeSyntax : IScopeSyntax
    {
        private readonly ComponentFactory componentFactory;

        public ScopeSyntax(ComponentFactory componentFactory)
        {
            this.componentFactory = componentFactory;
        }

        public void InSingletonScope()
        {
            this.InScope(f => new SingletonFactory(f));
        }

        public void InScope(Func<IFactory, IFactory> factoryScope)
        {
            this.componentFactory.Factory = factoryScope(this.componentFactory.Factory);
        }
    }
}