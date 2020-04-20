using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pattern.Mvvm.Forms
{
    public interface INavigationService
    {
        Task Navigate(Type pageType, bool animated = true);

        Task Navigate<T>(Type pageType, T parameterToNextViewModel, bool animated = true);

        Task Navigate<T, TParameter>(Type pageType, Func<T, Task> callBackWhenViewBack, TParameter parameterToNextViewModel, bool animated = true);
        
        Task Navigate<T>(Type pageType, Func<T, Task> callBackWhenViewBack, bool animated = true);

        Task NavigateBack(bool animated = true);

        Task NavigateRoot(Type pageType, bool animated = false);
        
        Task NavigateRoot<TParameter>(Type pageType, TParameter parameterToNextViewModel, bool animated = false);

        Task<T> GetParameter<T>(ViewModelBase viewModelBase);
    }
}