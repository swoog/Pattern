using Pattern.Core.Interfaces;

namespace Pattern.Module
{
    public interface IModule
    {
        void Load(IKernel kernel);
    }
}
