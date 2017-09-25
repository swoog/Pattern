using System.Threading.Tasks;
using Pattern.Tasks;

namespace Pattern.Mvvm.Forms
{
    public abstract class ViewModelBase : Pattern.Mvvm.ViewModelBase, ILoadingHandler
    {
        public abstract Task InitAsync();

        public abstract Task Resume();

        public abstract void StartLoading();

        public abstract void StopLoading();
    }
}