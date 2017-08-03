using System;

namespace Pattern.Perf
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Text;

    using Pattern.Core;
    using Pattern.Core.Autofac;
    using Pattern.Core.Interfaces;
    using Pattern.Core.Interfaces.Factories;
    using Pattern.Core.Ninject;
    using Pattern.Core.SimpleIoc;
    using Pattern.Core.Unity;

    public class MyClass : IMyClass
    {

    }

    public interface IMyClass
    {

    }

    public class Program
    {
        static void Main(string[] args)
        {
            var numbers = new int[] { 100000, 10000, 1000, 1 };

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
                sbReports.AppendLine($"| Container | Milisecondes | Coef |");
                sbReports.AppendLine($"| - | - | -: |");
                foreach (var perf in perfsNumber.Perfs)
                {
                    sbReports.AppendLine($"| {perf.Name} | {perf.ElapsedMilliseconds} | x{perf.Coef:##.00} |");
                }

                sbReports.AppendLine();
            }

            File.WriteAllText("../performances.md", sbReports.ToString());
#if DEBUG
            Console.ReadLine();
#endif
        }

        private static List<PerfStat> PerfStatsFor(string[] args, int count)
        {
            var kernels = new IKernel[]
                              {
                                  new NinjectStandardKernel(),
                                  new UnityStandardKernel(),
                                  new AutofacKernel(),
                                  new SimpleIocKernel(),
                                  new Kernel()
                              };

            foreach (var kernel in kernels)
            {
                kernel.Bind(typeof(IMyClass), new TypeFactory(typeof(MyClass), kernel));
            }

            foreach (var kernel in kernels.OfType<AutofacKernel>())
            {
                kernel.Init();
            }

            var perfs = new List<PerfStat>();

            foreach (var kernel in kernels)
            {
                perfs.Add(Perf(kernel, count));
            }

            perfs = perfs.OrderBy(p => p.ElapsedMilliseconds).ToList();

            var p1 = perfs.First();

            foreach (var perfStat in perfs)
            {
                perfStat.Coef = perfStat.ElapsedMilliseconds * 1.0 / p1.ElapsedMilliseconds;
            }

            return perfs;
        }

        private static PerfStat Perf(IKernel kernel, int l)
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            for (int i = 0; i < l; i++)
            {
                var c = kernel.Get(null, typeof(IMyClass));
            }

            stopWatch.Stop();

            return new PerfStat(kernel.GetType().Name, stopWatch.ElapsedMilliseconds, l);
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