namespace Pattern.Core.Interfaces
{
    using Pattern.Core.Interfaces.Factories;

    public interface IFactory
    {
        object Create(CallContext callContext);
    }
}