using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pattern.Core.Ninject
{
    using global::Ninject;

    using Pattern.Core.Interfaces;
    using Pattern.Core.Interfaces.Factories;

    public class NinjectStandardKernel : Pattern.Core.Interfaces.IKernel
    {
        private readonly global::Ninject.IKernel standardKernel;

        public NinjectStandardKernel()
            : this(new StandardKernel())
        {

        }

        public NinjectStandardKernel(global::Ninject.IKernel kernel)
        {
            this.standardKernel = kernel;
            this.standardKernel.Bind<Pattern.Core.Interfaces.IKernel>().ToConstant(this);
        }

        public void Bind(Type @from, IFactory toFactory)
        {
            switch (toFactory)
            {
                case TypeFactory typeFactory:
                    this.standardKernel.Bind(@from).To(typeFactory.TypeToCreate);
                    break;
                default:
                    this.standardKernel.Bind(@from).ToMethod(c => toFactory.Create(new CallContext(c.Request.ParentContext.Plan.Type, c.Plan.Type, false)));
                    break;
            }
        }

        public bool CanResolve(Type parent, Type @from)
        {
            return (bool)this.standardKernel.CanResolve(@from);
        }

        public object Get(Type parentType, Type @from)
        {
            return this.standardKernel.Get(@from);
        }
    }
}
