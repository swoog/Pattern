namespace Pattern.Config
{
    using Pattern.Core.Interfaces;

    public class FakeFactory : IFactory
    {
        public object Create(object[] parameters)
        {
            throw new System.NotImplementedException();
        }
    }
}
