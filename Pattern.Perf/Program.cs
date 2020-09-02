using System;

namespace Pattern.Perf
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Text;

    public class MyClass : IMyClass
    {

    }

    public interface IMyClass
    {

    }
    
    public class MyClassGeneric<T> : IMyClassGeneric<T>
    {

    }

    public interface IMyClassGeneric<T>
    {

    }

    public partial class Program
    {
        static void Main(string[] args)
        {
            var numbers = new int[] {  1, 1000, 10000, 100000 };

            var perfsNumbers = new List<PerfNumber>();

            foreach (var number in numbers)
            {
                GC.Collect(0, GCCollectionMode.Forced, true);
                GC.Collect(1, GCCollectionMode.Forced, true);
                GC.Collect(2, GCCollectionMode.Forced, true);

                var perfs = PerfStatsFor(args, number);

                var perfsNumber = new PerfNumber { Number = number, Perfs = perfs };

                perfsNumbers.Add(perfsNumber);
            }

            var sbReports = new StringBuilder();

            foreach (var perfsNumber in perfsNumbers)
            {
                var plurial = perfsNumber.Number > 1 ? "s" : string.Empty;
                sbReports.AppendLine($"For {perfsNumber.Number} instance{plurial} :");
                sbReports.AppendLine();
                sbReports.AppendLine($"| Container | Init | Milisecondes | Coef |");
                sbReports.AppendLine($"| - | - | - | -: |");
                foreach (var perf in perfsNumber.Perfs)
                {
                    sbReports.AppendLine($"| {perf.Name} | {perf.ElapsedMillisecondsInit} | {perf.ElapsedMilliseconds} | x{perf.Coef:##.00} |");
                }

                sbReports.AppendLine();
            }

            File.WriteAllText("../performances.md", sbReports.ToString());
        }

        private static List<PerfStat> PerfStatsFor(string[] args, int count)
        {
            var perfAnalysers = new IPerfAnalyser[]
                              {
                                  new ServiceCollectionPerfAnalyser(),
                                  new NinjectPerfAnalyser(), 
                                  new AutofacPerfAnalyser(), 
                                  new KernelPerfAnalyser(),
                                  new UnityPerfAnalyser(),
                              };
            
            var perfs = new List<PerfStat>();

            foreach (var kernel in perfAnalysers)
            {
                var stopWatch = new Stopwatch();
                stopWatch.Start();

                kernel.Create();
                kernel.Bind();
                kernel.Init();

                stopWatch.Stop();

                var perfStat = Perf(kernel, count);
                perfStat.ElapsedMillisecondsInit = stopWatch.ElapsedMilliseconds;
                perfs.Add(perfStat);
            }

            perfs = perfs.OrderBy(p => p.ElapsedMilliseconds).ToList();

            var p1 = perfs.First();

            foreach (var perfStat in perfs)
            {
                perfStat.Coef = perfStat.ElapsedMilliseconds * 1.0 / p1.ElapsedMilliseconds;
            }

            return perfs;
        }

        private static PerfStat Perf(IPerfAnalyser kernel, int l)
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            for (int i = 0; i < l; i++)
            {
                kernel.Get();
            }

            stopWatch.Stop();

            return new PerfStat(kernel.Name, stopWatch.ElapsedMilliseconds, l);
        }
    }

    internal class PerfNumber
    {
        public int Number { get; set; }

        public List<PerfStat> Perfs { get; set; }
    }

    internal class PerfStat
    {
        public string Name { get; }

        public long ElapsedMilliseconds { get; }

        public int Count { get; }

        public double Coef { get; set; }
        public long ElapsedMillisecondsInit { get; set; }

        public override string ToString()
        {
            return $"{Name} : {ElapsedMilliseconds * 1.0 / Count} (x{Coef:##.00})";
        }

        public PerfStat(string name, long elapsedMilliseconds, int count)
        {
            this.Name = name;
            this.ElapsedMilliseconds = elapsedMilliseconds;
            this.Count = count;
        }
    }
}