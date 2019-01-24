using System;
using System.Threading.Tasks;

namespace Pattern.Mvvm.Forms
{
    public interface INavigateFrom
    {
        Task NavigateFrom(Type toPage);
    }
}