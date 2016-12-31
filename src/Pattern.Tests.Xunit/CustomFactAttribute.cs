namespace Pattern.Tests.Xunit
{
    using global::Xunit;

    public class NamedFact : FactAttribute
    {
        private string displayName;

        public NamedFact(string displayName)
        {
            this.displayName = displayName.Replace("_", " ");
        }

        public override string DisplayName
        {
            get
            {
                return this.displayName;
            }
            set
            {
                this.displayName = value.Replace("_", " ");
            }
        }
    }
}