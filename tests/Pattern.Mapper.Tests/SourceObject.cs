namespace Pattern.Mapper.Tests
{
    public class SourceObject
    {
        public SourceObject()
        {
        }

        public string MyStringProperty { get; internal set; }
    }

    public class SourceObjectWithAnotherProperty : SourceObject
    {
        public string MyStringProperty2 { get; internal set; }
    }
}