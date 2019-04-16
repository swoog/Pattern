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

        private readonly ConditionalWeakTable<object, Func<object, Task>> callbacks =
            new ConditionalWeakTable<object, Func<object, Task>>();

        private readonly ConditionalWeakTable<object, object> parameters = new ConditionalWeakTable<object, object>();

        public NavigationService(IKernel kernel, NavigationPage navigationPage, INavigationHandler navigationHandler)
        {
            this.kernel = kernel;
            this.navigationPage = navigationPage;
            this.navigationHandler = navigationHandler;
            this.navigationPage.Popped += this.NavigationPage_Popped;
        }

        public Task Navigate(Type pageType)
        {
            return this.Navigate<object>(pageType, null);
        }

        public Task Navigate<T>(Type pageType, T parameterToNextViewModel)
        {
            return this.Navigate<object, T>(pageType, null, parameterToNextViewModel);
        }

        public async Task Navigate<T, TParameter>(
            Type pageType,
            Func<T, Task> callBackWhenViewBack,
            TParameter parameterToNextViewModel)
        {
            var animated = true;
            if (this.navigationPage.CurrentPage is INavigateFrom pageNavigateFrom)
            {
                animated = false;
                await pageNavigateFrom.NavigateFrom(pageType);
            }

            var (page, viewmodel) = this.ResolveView(pageType);
            if (!object.Equals(parameterToNextViewModel, default(T)))
            {
                this.parameters.Add(viewmodel, parameterToNextViewModel);                
            }

            if (callBackWhenViewBack != null)
            {
                this.callbacks.Add(viewmodel, o => callBackWhenViewBack((T) o));                
            }
            viewmodel.InitAsync().Fire(viewmodel);
       
            this.navigationHandler?.Navigate(pageType.Name, page);
            await this.navigationPage.PushAsync(page, animated);
        }

        public Task Navigate<T>(Type pageType, Func<T, Task> callBackWhenViewBack)
        {
            return this.Navigate<T, object>(pageType, callBackWhenViewBack, null);
        }

        private void NavigationPage_Popped(object sender, NavigationEventArgs e)
        {
            if (!this.callbacks.TryGetValue(e.Page.BindingContext, out var func))
                return;
            this.callbacks.Remove(e.Page.BindingContext);
            func(e.Page.BindingContext).Fire();
        }

        public async Task NavigateBack()
        {
            this.navigationHandler?.NavigateBack();
            await this.navigationPage.PopAsync(true);
            (this.navigationPage.CurrentPage.BindingContext as ViewModelBase)?.Resume();
        }

        private (Page, ViewModelBase) ResolveView(Type pageType)
        {
            var page = this.kernel.Get(pageType) as Page;
            return (page, page?.BindingContext as ViewModelBase);
        }

        public async Task NavigateRoot(Type pageType)
        {
            if (this.navigationPage.CurrentPage is INavigateFrom pageNavigateFrom)
            {
                await pageNavigateFrom.NavigateFrom(pageType);
            }

            var (page, viewmodel) = this.ResolveView(pageType);
            viewmodel.InitAsync().Fire(viewmodel);
            this.navigationHandler?.Navigate(pageType.Name, page);
            this.navigationPage.Navigation.InsertPageBefore(page,
                this.navigationPage.Navigation.NavigationStack.First());
            await this.navigationPage.PopToRootAsync(false);
        }

        public Task<T> GetParameter<T>(ViewModelBase viewModelBase)
        {
            if (this.parameters.TryGetValue(viewModelBase, out var parameter))
            {
                return Task.FromResult((T) parameter);
            }
            
            return Task.FromResult(default(T));
        }
    }
}