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

        public void ToSelf()
        {
            this.To<TFrom>();
        }

        public void To<TTo>()
            where TTo:TFrom
        {
            this.kernel.Bind(typeof(TFrom), new TypeFactory(typeof(TTo), this.kernel));
        }

        public void ToMethod<TTo>(Func<TTo> p) where TTo : TFrom
        {
            this.kernel.Bind(typeof(TFrom), new LambdaFactory(()=> p()));
        }

        public void ToFactory<T>()
            where T : IFactory
        {
            var factory = this.kernel.Get<T>();

            this.kernel.Bind(typeof(TFrom), factory);
        }
    }
}