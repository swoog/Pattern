namespace Pattern.Config
{
    using System;

    using Pattern.Core.Interfaces;
    using Pattern.Core.Interfaces.Factories;

    public class ToSyntax<TFrom> : IToSyntax<TFrom>
    {
        private readonly IKernel kernel;

        public ToSyntax(IKernel kernel)
        {
            this.kernel = kernel;
        }

        public IScopeSyntax ToSelf()
        {
            return this.To<TFrom>();
        }

        public IScopeSyntax To<TTo>()
            where TTo : TFrom
        {
            var typeFactory = new TypeFactory(typeof(TTo), this.kernel);
            var componentFactory = new ComponentFactory
                                       {
                                           Factory = typeFactory
                                       };

            this.kernel.Bind(typeof(TFrom), componentFactory);

            return new ScopeSyntax(componentFactory);
        }

        public void ToMethod<TTo>(Func<TTo> p) where TTo : TFrom
        {
            this.kernel.Bind(typeof(TFrom), new LambdaFactory(c => p()));
        }

        public void ToFactory<T>()
            where T : IFactory
        {
            var factory = this.kernel.Get<T>();

            this.kernel.Bind(typeof(TFrom), factory);
        }
    }

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