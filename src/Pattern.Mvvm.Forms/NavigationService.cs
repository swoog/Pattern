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

        private object parameter;

        public NavigationService(IKernel kernel, NavigationPage navigationPage, INavigationHandler navigationHandler)
        {
            this.kernel = kernel;
            this.navigationPage = navigationPage;
            this.navigationHandler = navigationHandler;
            this.navigationPage.Popped += this.NavigationPage_Popped;
        }

        public async Task Navigate(Type pageType)
        {
            var animated = true;
            if (this.navigationPage.CurrentPage is INavigateFrom pageNavigateFrom)
            {
                animated = false;
                await pageNavigateFrom.NavigateFrom(pageType);
            }

            var page = this.ResolveView(pageType);
            this.navigationHandler?.Navigate(pageType.Name, page);

            await this.navigationPage.PushAsync(page, animated);
        }

        public async Task Navigate<T>(Type pageType, T parameterToNextViewModel)
        {
            var animated = true;
            if (this.navigationPage.CurrentPage is INavigateFrom pageNavigateFrom)
            {
                animated = false;
                await pageNavigateFrom.NavigateFrom(pageType);
            }

            this.parameter = parameterToNextViewModel;
            var page = this.ResolveView(pageType);
            this.navigationHandler?.Navigate(pageType.Name, page);
            await this.navigationPage.PushAsync(page, animated);
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

            this.parameter = parameterToNextViewModel;
            var page = this.ResolveView(pageType);
            this.navigationHandler?.Navigate(pageType.Name, page);
            this.callbacks.Add(page.BindingContext, o => callBackWhenViewBack((T) o));
            await this.navigationPage.PushAsync(page, animated);
        }

        public async Task Navigate<T>(Type pageType, Func<T, Task> callBackWhenViewBack)
        {
            var animated = true;
            if (this.navigationPage.CurrentPage is INavigateFrom pageNavigateFrom)
            {
                animated = false;
                await pageNavigateFrom.NavigateFrom(pageType);
            }

            var page = this.ResolveView(pageType);
            this.navigationHandler?.Navigate(pageType.Name, page);
            this.callbacks.Add(page.BindingContext, o => callBackWhenViewBack((T) o));
            await this.navigationPage.PushAsync(page, animated);
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

        private object Resolve(Type pageType)
        {
            return this.Instantiate(pageType);
        }

        private object Instantiate(Type pageType)
        {
            return this.kernel.Get(pageType);
        }

        private Page ResolveView(Type pageType)
        {
            return this.Resolve(pageType) as Page;
        }

        public async Task NavigateRoot(Type pageType)
        {
            if (this.navigationPage.CurrentPage is INavigateFrom pageNavigateFrom)
            {
                await pageNavigateFrom.NavigateFrom(pageType);
            }

            var page = this.ResolveView(pageType);
            this.navigationHandler?.Navigate(pageType.Name, page);
            this.navigationPage.Navigation.InsertPageBefore(page,
                this.navigationPage.Navigation.NavigationStack.First());
            await this.navigationPage.PopToRootAsync(false);
        }

        public Task<T> GetParameter<T>()
        {
            return Task.FromResult((T) this.parameter);
        }
    }
}