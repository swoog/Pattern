namespace Pattern.Config
{
    using Pattern.Core.Interfaces;

    public class ComponentFactory : IFactory
    {
        public IFactory Factory { get; set; }

        public object Create(CallContext callContext)
        {
            return this.Factory.Create(callContext);
        }
    }
}