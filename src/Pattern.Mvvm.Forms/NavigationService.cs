using Pattern.Core.Interfaces;
using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Pattern.Tasks;
using Xamarin.Forms;

namespace Pattern.Mvvm.Forms
{
    public class NavigationService : INavigationService
    {
        private readonly IKernel kernel;

        private readonly NavigationPage navigationPage;
        private readonly INavigationHandler navigationHandler;
        
        public NavigationService(IKernel kernel, NavigationPage navigationPage, INavigationHandler navigationHandler)
        {
            this.kernel = kernel;
            this.navigationPage = navigationPage;
            this.navigationHandler = navigationHandler;
            this.navigationPage.Popped += this.NavigationPage_Popped;
        }

        public Task Navigate(Type pageType,
            bool animated = true)
        {
            return this.Navigate<object>(pageType, null, animated);
        }

        public Task Navigate<T>(Type pageType, T parameterToNextViewModel,
            bool animated = true)
        {
            return this.Navigate<object, T>(pageType, null, parameterToNextViewModel, animated);
        }

        public async Task Navigate<T, TParameter>(
            Type pageType,
            Func<T, Task> callBackWhenViewBack,
            TParameter parameterToNextViewModel,
            bool animated = true)
        {
            if (this.navigationPage.CurrentPage is INavigateFrom pageNavigateFrom)
            {
                animated = false;
                await pageNavigateFrom.NavigateFrom(pageType);
            }

            var (page, viewmodel) = this.ResolveView(pageType);
            if (!object.Equals(parameterToNextViewModel, default(T)))
            {
                viewmodel.Parameter = parameterToNextViewModel;
            }

            if (callBackWhenViewBack != null)
            {
                viewmodel.Callback = o => callBackWhenViewBack((T) o);
            }
            viewmodel.InitAsync().Fire(viewmodel);
       
            this.navigationHandler?.Navigate(pageType.Name, page);
            await this.navigationPage.PushAsync(page, animated);
            await viewmodel.AfterNavigationAsync();
        }

        public Task Navigate<T>(Type pageType, Func<T, Task> callBackWhenViewBack,
            bool animated = true)
        {
            return this.Navigate<T, object>(pageType, callBackWhenViewBack, null, animated);
        }

        private void NavigationPage_Popped(object sender, NavigationEventArgs e)
        {
            var viewModel = e.Page.BindingContext as ViewModelBase;
            var func = viewModel?.Callback;
            func?.Invoke(e.Page.BindingContext)?.Fire();
        }

        public async Task NavigateBack(bool animated = true)
        {
            this.navigationHandler?.NavigateBack();
            await this.navigationPage.PopAsync(animated);
            (this.navigationPage.CurrentPage.BindingContext as ViewModelBase)?.Resume();
        }

        private (Page, ViewModelBase) ResolveView(Type pageType)
        {
            var page = this.kernel.Get(pageType) as Page;
            return (page, page?.BindingContext as ViewModelBase);
        }
 
        public Task NavigateRoot(Type pageType, bool animated = false)
        {
            return NavigateRoot<object>(pageType, null, animated);
        }
        
        public async Task NavigateRoot<TParameter>(Type pageType, TParameter parameterToNextViewModel, bool animated = false)
        {
            if (this.navigationPage.CurrentPage is INavigateFrom pageNavigateFrom)
            {
                animated = false;
                await pageNavigateFrom.NavigateFrom(pageType);
            }

            var (page, viewmodel) = this.ResolveView(pageType);
            if (!object.Equals(parameterToNextViewModel, default(TParameter)))
            {
                viewmodel.Parameter = parameterToNextViewModel;
            }

            viewmodel.InitAsync().Fire(viewmodel);
       
            this.navigationHandler?.Navigate(pageType.Name, page);
            this.navigationPage.Navigation.InsertPageBefore(page,
                this.navigationPage.Navigation.NavigationStack.First());
            await this.navigationPage.PopToRootAsync(animated);
            await viewmodel.AfterNavigationAsync();
        }

        public Task<T> GetParameter<T>(ViewModelBase viewModelBase)
        {
            if (viewModelBase.Parameter != null)
            {
                return Task.FromResult((T)viewModelBase.Parameter);
            }
            
            return Task.FromResult(default(T));
        }
    }
}