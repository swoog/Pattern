using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Pattern.Tasks;

namespace Pattern.Mvvm.Forms
{
    public abstract class ViewModelBase : Pattern.Mvvm.ViewModelBase, ILoadingHandler
    {
         private readonly Dictionary<string, ICommand> commands = new Dictionary<string, ICommand>();

         public object Parameter { get; set; }
         public Func<object, Task> Callback { get; set; }

         protected AsyncCommand<T> CreateCommand<T>(Func<T, Task> method, Func<T, bool> canExecute = null, ILoadingHandler loadingHandler = null,
             [CallerMemberName] string name = null)
         {
             if (this.commands.ContainsKey(name))
             {
                 return this.commands[name] as AsyncCommand<T>;
             }
 
             var command = new AsyncCommand<T>(method, loadingHandler, canExecute);
             this.commands.Add(name, command);
 
             return command;
         }
  
         protected AsyncCommand CreateCommand(Func<Task> method, Func<bool> canExecute = null,
             ILoadingHandler loadingHandler = null,
             [CallerMemberName] string name = null)
         {
             if (this.commands.ContainsKey(name))
             {
                 return this.commands[name] as AsyncCommand;
             }
 
             var command = new AsyncCommand(method, loadingHandler, canExecute);
             this.commands.Add(name, command);
 
             return command;
         }
         
         public abstract Task InitAsync();

         public virtual Task AfterNavigationAsync()
         {
             return Task.CompletedTask;
         }

        public abstract Task Resume();

        public abstract void StartLoading();

        public abstract void StopLoading();
    }
}