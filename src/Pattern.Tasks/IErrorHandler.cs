using System;

namespace Pattern.Tasks
{
    public interface IErrorHandler
    {
        void Handle(Exception ex);
    }
}