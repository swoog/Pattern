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
            var componentFactory = new ComponentFactory(typeFactory);

            this.kernel.Bind(typeof(TFrom), componentFactory);

            return new ScopeSyntax(componentFactory);
        }

        public IScopeSyntax ToMethod<TTo>(Func<TTo> p) where TTo : TFrom
        {
            var typeFactory = new LambdaFactory(c => p());
            var componentFactory = new ComponentFactory(typeFactory);

            this.kernel.Bind(typeof(TFrom), componentFactory);
            return new ScopeSyntax(componentFactory);
        }

        public void ToFactory<T>()
            where T : IFactory
        {
            var factory = this.kernel.Get<T>();

            this.kernel.Bind(typeof(TFrom), factory);
        }
    }
}