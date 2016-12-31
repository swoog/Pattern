using NSubstitute;
using Pattern.Tests.Xunit;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Pattern.Mvvm.Tests
{
    public class RelayCommandTests
    {
        [NamedFact(nameof(Should_action_invoked_when_command_execute))]
        public void Should_action_invoked_when_command_execute()
        {
            var action = Substitute.For<Action>();
            var relayCommand = new RelayCommand(action);

            relayCommand.Execute(null);

            action.Received().Invoke();
        }

        [NamedFact(nameof(Should_action_is_not_invoked_when_command_can_execute_return_false))]
        public void Should_action_is_not_invoked_when_command_can_execute_return_false()
        {
            var action = Substitute.For<Action>();
            var relayCommand = new RelayCommand(action, () => false);

            relayCommand.Execute(null);

            action.DidNotReceive().Invoke();
        }

        [NamedFact(nameof(Should_can_execute_return_false_when_command_can_execute_return_false))]
        public void Should_can_execute_return_false_when_command_can_execute_return_false()
        {
            var action = Substitute.For<Action>();
            var relayCommand = new RelayCommand(action, () => false);

            relayCommand.Execute(null);

            Assert.False(relayCommand.CanExecute(null));
        }
    }
}
