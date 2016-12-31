using NSubstitute;
using Pattern.Tests.Xunit;
using System;
using System.ComponentModel;
using Xunit;

namespace Pattern.Mvvm.Tests
{
    public class program
    {
        public static void Main()
        {

        }
    }

    public class BaseViewModelTests
    {
        [NamedFact(nameof(Should_notify_property_changed_when_raise_property))]
        public void Should_notify_property_changed_when_raise_property()
        {
            var baseViewModel = new FakeBaseViewModel();
            var watcher = new PropertyChangedWatcher();
            baseViewModel.PropertyChanged += watcher.PropertyChanged;

            baseViewModel.RaiseProperty();

            Assert.True(watcher.IsRaised);
        }

        [NamedFact(nameof(Should_notify_property_changed_toto_when_raise_property_toto))]
        public void Should_notify_property_changed_toto_when_raise_property_toto()
        {
            var baseViewModel = new FakeBaseViewModel();
            var watcher = new PropertyChangedWatcher();
            baseViewModel.PropertyChanged += watcher.PropertyChanged;

            baseViewModel.RaiseProperty("Toto");

            Assert.Equal("Toto", watcher.PropertyName);
        }

        [NamedFact(nameof(Should_notify_property_changed_toto_when_set_property))]
        public void Should_notify_property_changed_toto_when_set_property()
        {
            var baseViewModel = new FakeBaseViewModel();
            var watcher = new PropertyChangedWatcher();
            baseViewModel.PropertyChanged += watcher.PropertyChanged;

            baseViewModel.Toto = "Value";

            Assert.Equal("Toto", watcher.PropertyName);
        }
    }
}
