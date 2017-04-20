namespace Pattern.Core.Tests.Fakes
{
    public class ObjectWithArguments
    {
        public int IntValue { get; set; }

        public string StringValue { get; set; }

        public ObjectWithArguments(int i, string stringValue)
        {
            this.IntValue = i;
            this.StringValue = stringValue;
        }
    }
}