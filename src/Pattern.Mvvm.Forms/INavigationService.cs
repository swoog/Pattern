using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pattern.Mvvm.Forms
{
    public interface INavigationService
    {
        Dictionary<string, string> QueryString { get; set; }

        Task Navigate(string path, bool toPopup = false, bool toPane = false);

        Task Navigate<T>(string path, Action<T> callBack, bool toPopup = false, bool toPane = false);

        Task NavigateBack();

        Task NavigateRoot(string path);
    }
}