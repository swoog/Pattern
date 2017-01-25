namespace Pattern.Mapper
{
    using System;

    public class LambdaMapping
    {
        public LambdaMapping(Func<object, object> lambda, Type source, Type destination)
        {
            this.Lambda = lambda;
            this.Source = source;
            this.Destination = destination;
        }

        public Type Destination { get; private set; }

        public Type Source { get; private set; }

        public Func<object, object> Lambda { get; private set; }

        public object To(object source)
        {
            return this.Lambda(source);
        }
    }
}