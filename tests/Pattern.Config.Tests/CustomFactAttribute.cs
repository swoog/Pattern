namespace Pattern.Config.Tests
{
    using Xunit;

    public class CustomFactAttribute : FactAttribute
    {
        private string displayName;

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