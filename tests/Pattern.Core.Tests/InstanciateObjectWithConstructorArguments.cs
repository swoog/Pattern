namespace Pattern.Core.Tests
{
    using Pattern.Core.Interfaces;
    using Pattern.Core.Tests.Fakes;

    using Xunit;

    public class InstanciateObjectWithConstructorArguments
    {
        private readonly IKernel kernel;

        public InstanciateObjectWithConstructorArguments()
        {
            this.kernel = new Kernel();
        }

        [CustomFact(DisplayName = nameof(Should_instanciate_object_When_have_custom_arguments))]
        public void Should_instanciate_object_When_have_custom_arguments()
        {
            var instance = this.kernel.Get(null, typeof(ObjectWithArguments), 1, "Toto");

            var instanceOfObjsctWithArguments = Assert.IsType<ObjectWithArguments>(instance);
            Assert.Equal("Toto", instanceOfObjsctWithArguments.StringValue);
            Assert.Equal(1, instanceOfObjsctWithArguments.IntValue);
        }
    }
}