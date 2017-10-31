using System;
using Pattern.Core.Interfaces;

namespace Pattern.Config
{
    public interface IScopeSyntax
    {
        void InSingletonScope();

        void InScope(Func<IFactory, IFactory> factoryScope);
    }
}