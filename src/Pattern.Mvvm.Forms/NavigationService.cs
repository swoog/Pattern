using Pattern.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Pattern.Mvvm.Forms
{
    public class NavigationService : INavigationService
    {
        private readonly IKernel kernel;

        private readonly NavigationPage navigationPage;
        private readonly INavigationHandler navigationHandler;
        private Action<object> callBack;
        private readonly NavigationConfig config;

        public NavigationService(IKernel kernel, NavigationPage navigationPage, INavigationHandler navigationHandler, NavigationConfig navigationConfig)
        {
            this.kernel = kernel;
            this.navigationPage = navigationPage;
            this.navigationHandler = navigationHandler;
            config = navigationConfig;
        }

        public Dictionary<string, string> QueryString { get; set; }

        public Task Navigate(string path, bool toPopup = false, bool toPane = false)
        {
            var page = this.ResolveView(path);

            this.navigationHandler?.Navigate(path, page);

            return this.navigationPage.PushAsync(page, false);
        }

        public Task Navigate<T>(string path, Action<T> callBack, bool toPopup = false, bool toPane = false)
        {
            var page = this.ResolveView(path);

            this.navigationHandler?.Navigate(path, page);

            this.callBack = (o) => callBack((T)o);
            this.navigationPage.Popped += NavigationPage_Popped;

            return this.navigationPage.PushAsync(page, false);
        }

        void NavigationPage_Popped(object sender, NavigationEventArgs e)
        {
            this.navigationPage.Popped -= NavigationPage_Popped;
            this.callBack?.Invoke(e.Page.BindingContext);
            this.callBack = null;
        }

        public async Task NavigateBack()
        {
            this.navigationHandler?.NavigateBack();

            await this.navigationPage.PopAsync(true);
        }

        private object Resolve(string path, string formatName)
        {
            var regex = new Regex(@"^/(.+)\.xaml([\?&][a-zA-Z]+=.+)*$");

            Match match = regex.Match(path);
            if (match.Success)
            {
                var queryString = GetQueryString(match.Groups?[2]?.Value);

                this.QueryString = queryString;

                return this.Instantiate(formatName, match.Groups[1].Value);
            }

            throw new PageNotFoundException(path);
        }

        private Dictionary<string, string> GetQueryString(string value)
        {
            var queryString = new Dictionary<string, string>();
            if (string.IsNullOrEmpty(value))
            {
                return queryString;
            }

            var parameters = value.Substring(1).Split('&');

            foreach (var parameter in parameters)
            {
                var nameParam = parameter.Split('=')[0];
                var valueParam = parameter.Split('=')[1];
                queryString.Add(nameParam, valueParam);
            }

            return queryString;
        }

        private object Instantiate(string formatName, string name)
        {
            var assembly = typeof(NavigationService).GetTypeInfo().Assembly;
            var type = assembly.GetType(string.Format(formatName, name));

            return this.kernel.Get(type);
        }

        private Page ResolveView(string path)
        {
            return this.Resolve(path, config.PagePattern) as Page;
        }

        public async Task NavigateRoot(string path)
        {
            var page = this.ResolveView(path);

            this.navigationHandler?.Navigate(path, page);

            this.navigationPage.Navigation.InsertPageBefore(page,
                this.navigationPage.Navigation.NavigationStack.First());
            await this.navigationPage.PopToRootAsync(false);
        }
    }
}