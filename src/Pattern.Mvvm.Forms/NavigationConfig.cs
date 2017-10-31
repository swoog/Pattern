using System.Reflection;

namespace Pattern.Mvvm.Forms
{
    public class NavigationConfig
    {
        public string PagePattern { get; set; }

        public Assembly PageAssembly { get; set; }
    }
}