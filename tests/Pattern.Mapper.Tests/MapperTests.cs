namespace Pattern.Mapper.Tests
{
    using System.Linq;

    using Pattern.Tests.Xunit;

    using Xunit;

    public class MapperTests
    {
        public MapperTests()
        {
            MapperExtensions.Clean();
        }

        [NamedFact(nameof(Should_map_to_null_when_object_is_null))]
        public void Should_map_to_null_when_object_is_null()
        {
            MapperExtensions.Add<SourceObject, DestinationObject>(s => new DestinationObject { MyStringProperty = s.MyStringProperty });

            SourceObject sourceObject = null;

            var destinationObject = sourceObject.Map<DestinationObject>();

            Assert.Null(destinationObject);
        }

        [NamedFact(nameof(Should_map_property_when_have_same_name))]
        public void Should_map_property_when_have_same_name()
        {
            MapperExtensions.Add<SourceObject, DestinationObject>(s => new DestinationObject { MyStringProperty = s.MyStringProperty });

            var sourceObject = new SourceObject() { MyStringProperty = "MyStringValue" };

            var destinationObject = sourceObject.Map<DestinationObject>();

            Assert.Equal("MyStringValue", destinationObject.MyStringProperty);
        }

        [NamedFact(nameof(Should_map_property_on_enumeration_when_have_same_name))]
        public void Should_map_property_on_enumeration_when_have_same_name()
        {
            MapperExtensions.Add<SourceObject, DestinationObject>(s => new DestinationObject { MyStringProperty = s.MyStringProperty });

            var sourceObjects = new[] { new SourceObject() { MyStringProperty = "MyStringValue" } };

            var destinationObject = sourceObjects.Map<DestinationObject>();

            Assert.Equal("MyStringValue", destinationObject.ElementAt(0).MyStringProperty);
        }

        [NamedFact(nameof(Should_trow_exception_when_destination_property_does_not_exists))]
        public void Should_trow_exception_when_destination_property_does_not_exists()
        {
            MapperExtensions.Add<SourceObject, NotFoundPropertyDestinationObject>(s => new NotFoundPropertyDestinationObject { MyStringProperty = s.MyStringProperty });

            var mappingException = Assert.Throws<MappingException>(() => MapperExtensions.Validate<SourceObject>().Map<NotFoundPropertyDestinationObject>().AllPropertyAreNotEmpty());

            Assert.Equal("MyNotFoundProperty is not map.", mappingException.Message);
        }

        [NamedFact(nameof(Should_trow_exception_when_destination_property_does_not_exists_and_type_is_int))]
        public void Should_trow_exception_when_destination_property_does_not_exists_and_type_is_int()
        {
            MapperExtensions.Add<SourceObject, DestinationObjectWithIntProperty>(s => new DestinationObjectWithIntProperty());

            var mappingException = Assert.Throws<MappingException>(() => MapperExtensions.Validate<SourceObject>().Map<DestinationObjectWithIntProperty>().AllPropertyAreNotEmpty());

            Assert.Equal("MyIntProperty is not map.", mappingException.Message);
        }

        [NamedFact(nameof(Should_validate_when_source_object_contains_readonly_property))]
        public void Should_validate_when_source_object_contains_readonly_property()
        {
            MapperExtensions.Add<ReadOnlySourceObject, DestinationObject>(s => new DestinationObject { MyStringProperty = s.MyStringProperty });

            MapperExtensions.Validate<ReadOnlySourceObject>().Map<DestinationObject>().AllPropertyAreNotEmpty();
        }


        [NamedFact(nameof(Should_validate_when_source_object_contains_property_on_parent))]
        public void Should_validate_when_source_object_contains_property_on_parent()
        {
            MapperExtensions.Add<ChildSourceObject, DestinationObject>(s => new DestinationObject { MyStringProperty = s.MyStringProperty });

            MapperExtensions.Validate<ChildSourceObject>().Map<DestinationObject>().AllPropertyAreNotEmpty();
        }
    }

    public class ChildSourceObject : SourceObject
    {
    }

    public class ReadOnlySourceObject
    {
        public string ReadOnlyProperty => "value of read only property";

        public string MyStringProperty { get; internal set; }
    }

    public class DestinationObjectWithIntProperty
    {
        public int MyIntProperty { get; set; }
    }
}
