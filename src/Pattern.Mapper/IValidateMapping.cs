namespace Pattern.Mapper
{
    public interface IValidateMapping
    {
        IValidateMapping Map<T>() where T : new();

        void AllPropertyAreNotEmpty();
    }
}