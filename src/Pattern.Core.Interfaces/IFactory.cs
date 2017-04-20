namespace Pattern.Core.Interfaces
{
    public interface IFactory
    {
        object Create(object[] parameters);
    }
}