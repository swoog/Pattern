using System;

namespace Pattern.Tasks
{
    public interface IHandleError
    {
        void Handle(Exception ex);
    }
}