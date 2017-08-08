namespace Pattern.Config
{
    using Pattern.Core.Interfaces;

    public class FakeFactory : IFactory
    {
        public object Create(CallContext callContext, object[] parameters)
        {
            throw new System.NotImplementedException();
        }
    }
}
