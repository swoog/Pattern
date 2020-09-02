namespace Pattern.Perf
{
    public interface IPerfAnalyser
    {
        void Create();
            
        void Bind();

        void Init();

        void Get();

        string Name { get; }
    }
}