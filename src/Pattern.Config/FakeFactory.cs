namespace Pattern.Config
{
    using Pattern.Core.Interfaces;

    public class FakeFactory : IFactory
    {
        public object Create(CallContext callContext)
        {
            throw new System.NotImplementedException();
        }
    }
}
