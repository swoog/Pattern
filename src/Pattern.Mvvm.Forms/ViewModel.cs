using Pattern.Core.Interfaces;
using Xamarin.Forms;

namespace Pattern.Mvvm.Forms
{
    public static class ViewModel
    {
        public static void BindViewModel<TView, TViewModel>(this IKernel syntax)
            where TView : Page
            where TViewModel : ViewModelBase
        {
            syntax.Bind(typeof(TView), new PageFactory(typeof(TView), typeof(TViewModel), syntax));
        }
    }
}