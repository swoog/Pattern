using Xamarin.Forms;

namespace Pattern.Mvvm.Forms
{
    public interface INavigationHandler
    {
        void Navigate(string path, Page page);

        void NavigateBack();
    }
}