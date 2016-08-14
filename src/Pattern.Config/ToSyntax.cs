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
            this.kernel.Bind(typeof(TFrom), typeof(TFrom));
        }

        public void To<TTo>()
            where TTo:TFrom
        {
            this.kernel.Bind(typeof(TFrom), typeof(TTo));
        }

        public void ToMethod<TTo>(Func<TTo> p) where TTo : TFrom
        {
            this.kernel.Bind(typeof(TFrom), new LambdaFactory(()=> p()));
        }
    }
}