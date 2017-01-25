namespace Pattern.Mapper
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    public static class MapperExtensions
    {
        private static Dictionary<string, LambdaMapping> mappings = new Dictionary<string, LambdaMapping>();

        public static void Clean()
        {
            mappings.Clear();
        }

        public static IEnumerable<T> Map<T>(this IEnumerable<object> sourceObjects)
            where T : new()
        {
            foreach (var sourceObject in sourceObjects)
            {
                string key = CreateKey<T>(sourceObject.GetType());

                var destinationObject = mappings[key].To(sourceObject);

                yield return (T)destinationObject;
            }
        }

        public static T Map<T>(this object sourceObject)
            where T : new()
        {
            if (sourceObject == null)
            {
                return default(T);
            }

            string key = CreateKey<T>(sourceObject.GetType());

            var destinationObject = mappings[key].To(sourceObject);

            return (T)destinationObject;
        }

        public static void Add<T1, T2>(Func<T1, T2> p)
        {
            string key = CreateKey<T2>(typeof(T1));
            mappings.Add(key, new LambdaMapping(obj => p((T1)obj), typeof(T1), typeof(T2)));
        }

        private static string CreateKey<T2>(Type typeSource)
        {
            return $"{typeSource.FullName}$${typeof(T2).FullName}";
        }

        public static IValidateMapping Validate<T>() where T : new()
        {
            var source = new T();

            foreach (var declaredProperty in typeof(T).GetTypeInfo().GetProperties())
            {
                if (declaredProperty.CanWrite)
                {
                    declaredProperty.SetValue(source, NotEmptyValue(declaredProperty.PropertyType));
                }
            }

            return new ValidateMapping(source);
        }

        private static object NotEmptyValue(Type type)
        {
            if (type == typeof(string))
            {
                return "1";
            }

            if (type == typeof(int))
            {
                return 1;
            }

            return null;
        }
    }
}
