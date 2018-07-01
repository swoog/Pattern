using Pattern.Config;
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
        
        public static NavigationPage LoadNaviationPage(this IKernel kernel, NavigationConfig config)
        {
            kernel.Bind<NavigationConfig>().ToMethod(() => config);
            kernel.Bind<INavigationService>().To<NavigationService>().InSingletonScope();
            kernel.Bind<NavigationPage>().ToSelf().InSingletonScope();

            return kernel.Get<NavigationPage>();
        }
    }
}