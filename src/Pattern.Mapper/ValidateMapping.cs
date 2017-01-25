namespace Pattern.Mapper
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    public class ValidateMapping : IValidateMapping
    {
        private readonly object source;

        public ValidateMapping(object source)
        {
            this.source = source;
        }

        public IValidateMapping Map<T1>() where T1 : new()
        {
            return new ValidateMapping(this.source.Map<T1>());
        }

        public void AllPropertyAreNotEmpty()
        {
            foreach (var declaredProperty in this.source.GetType().GetTypeInfo().GetProperties())
            {
                this.ThrowIfEmpty(declaredProperty.PropertyType, declaredProperty.Name, declaredProperty.GetValue(this.source));
            }
        }

        private void ThrowIfEmpty(Type type, string declaredPropertyName, object getValue)
        {
            if (type == typeof(string) && getValue == null)
            {
                throw new MappingException($"{declaredPropertyName} is not map.");
            }

            if (type == typeof(int) && (int)getValue == 0)
            {
                throw new MappingException($"{declaredPropertyName} is not map.");
            }
        }
    }

    public static class TypeInfoExtensions
    {
        public static IEnumerable<PropertyInfo> GetProperties(this TypeInfo typeInfo)
        {
            if (typeInfo == typeof(object).GetTypeInfo())
            {
                yield break;
            }

            foreach (var property in typeInfo.DeclaredProperties)
            {
                yield return property;
            }

            foreach (var property in GetProperties(typeInfo.BaseType.GetTypeInfo()))
            {
                yield return property;
            }
        }
    }
}