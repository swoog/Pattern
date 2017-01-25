namespace Pattern.Mapper.Tests
{
    using System.Collections.Generic;

    public class DestinationObject
    {
        public string MyStringProperty { get; internal set; }
    }

    public class NotFoundPropertyDestinationObject
    {
        public IEnumerable<char> MyStringProperty { get; internal set; }

        public string MyNotFoundProperty { get; internal set; }
    }
}