namespace Pattern.Config
{
    using Pattern.Core.Interfaces;

    public class ComponentFactory : IFactory
    {
        public IFactory Factory { get; set; }

        public object Create(object[] parameters)
        {
            return this.Factory.Create(parameters);
        }
    }
}