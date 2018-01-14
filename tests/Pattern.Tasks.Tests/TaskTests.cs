using System;
using System.Threading.Tasks;
using NSubstitute;
using Pattern.Tests.Xunit;
using Xunit;

namespace Pattern.Tasks.Tests
{
    public class TaskTests
    {
        private bool hasRun;

        public TaskTests()
        {
            this.hasRun = false;
        }

        [NamedFact(nameof(Should_run_task_When_fire))]
        public void Should_run_task_When_fire()
        {
            TaskRun().Fire();

            Assert.True(this.hasRun);
        }

        [NamedFact(nameof(Should_load_handler_task_When_fire))]
        public void Should_load_handler_task_When_fire()
        {
            var loader = Substitute.For<ILoadingHandler>();
            TaskRun().Fire(loader);

            Received.InOrder(() =>
            {
                loader.Received(1).StartLoading();
                loader.Received(1).StopLoading();
            });
        }

        [NamedFact(nameof(Should_do_nothing_When_fire_exception_and_no_error_handler))]
        public void Should_do_nothing_When_fire_exception_and_no_error_handler()
        {
            TaskRunException().Fire();
        }

        [NamedFact(nameof(Should_error_handler_task_When_fire))]
        public void Should_error_handler_task_When_fire()
        {
            var handleError = Substitute.For<IErrorHandler>();
            TaskRunException().Fire(handleError);

            handleError.Received(1).Handle(Arg.Any<Exception>());
        }

        [NamedFact(nameof(Should_default_error_handler_When_fire))]
        public void Should_default_error_handler_When_fire()
        {
            var handleError = Substitute.For<IErrorHandler>()
                                .UseDefault();
            TaskRunException().Fire();

            handleError.Received(1).Handle(Arg.Any<Exception>());
        }

        [NamedFact(nameof(Should_combine_default_error_handler_When_fire))]
        public void Should_combine_default_error_handler_When_fire()
        {
            var handleError2 = Substitute.For<IErrorHandler>();
            var handleError = Substitute.For<IErrorHandler>()
                .UseDefault();

            TaskRunException().Fire(handleError2.CombineWithDefault());

            handleError.Received(1).Handle(Arg.Any<Exception>());
        }

        private async Task TaskRun()
        {
            this.hasRun = true;
        }

        private async Task TaskRunException()
        {
            throw new Exception();
        }
    }
}
