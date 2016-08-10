using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pattern.Core.Ninject
{
    using global::Ninject;

    public class NinjectStandardKernel
    {
        private StandardKernel standardKernel;

        public NinjectStandardKernel()
        {
            this.standardKernel = new StandardKernel();
        }
    }
}
