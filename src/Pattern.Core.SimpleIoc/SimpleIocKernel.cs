using System;

namespace Pattern.Core.SimpleIoc
{
    using System.Linq;
    using System.Reflection;
    using System.Runtime.Remoting.Messaging;

    using GalaSoft.MvvmLight.Ioc;

    using Pattern.Core.Interfaces;
    using Pattern.Core.Interfaces.Factories;

    public class SimpleIocKernel : Pattern.Core.Interfaces.IKernel
    {
        private readonly SimpleIoc simpleIoc;

        private readonly MethodInfo registerMethod;

        private readonly MethodInfo isRegisterMethod;

        public SimpleIocKernel()
            : this(new SimpleIoc())
        {
            this.registerMethod = this.simpleIoc.GetType().GetMethods()
                .Where(m => m.Name == "Register")
                .First(m => m.GetGenericArguments().Length == 2 && m.GetParameters().Length == 0);
            this.isRegisterMethod = this.simpleIoc.GetType().GetMethods()
                .Where(m => m.Name == "IsRegistered")
                .First(m => m.GetGenericArguments().Length == 1 && m.GetParameters().Length == 0);
        }

        public SimpleIocKernel(SimpleIoc simpleIoc)
        {
            this.simpleIoc = simpleIoc;
        }

        public void Bind(Type @from, IFactory toFactory)
        {
            switch (toFactory)
            {
                case TypeFactory typeFactory:
                    registerMethod.MakeGenericMethod(@from, typeFactory.TypeToCreate).Invoke(this.simpleIoc, new Object[0]);
                    break;
            }
        }

        public object Get(Type parentType, Type @from, params object[] parameters)
        {
            return this.simpleIoc.GetInstance(@from);
        }

        public bool CanResolve(Type parentType, Type @from)
        {
            return (bool)isRegisterMethod.MakeGenericMethod(@from).Invoke(this.simpleIoc, new Object[0]);
        }
    }
}
