using System;
using Pattern.Core.Interfaces;

namespace Pattern.Module.Tests
{
    public class FakeModule : IModule
    {
        public void Load(IKernel kernel)
        {
            throw new NotImplementedException();
        }
    }
}