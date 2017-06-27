namespace Pattern.Config
{
    using Pattern.Core.Interfaces;

    public class ComponentFactory : IFactory
    {
        public IFactory Factory { get; set; }

        public object Create(CallContext callContext, object[] parameters)
        {
            return this.Factory.Create(callContext, parameters);
        }
    }
}