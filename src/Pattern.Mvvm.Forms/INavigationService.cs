using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pattern.Mvvm.Forms
{
    public interface INavigationService
    {
        Task Navigate(Type pageType);

        Task Navigate<T>(Type pageType, T parameterToNextViewModel);

        Task Navigate<T>(Type pageType, Action<T> callBackWhenViewBack);

        Task NavigateBack();

        Task NavigateRoot(Type pageType);

        Task<T> GetParameter<T>();
    }
}