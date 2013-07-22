using System;
using System.Diagnostics;

namespace ComponentGlue.Performance
{
    class Program
    {
        const int NUMBER_OF_ITERATIONS = 1000;
		static bool outputEnabled = false;

        static void Main(string[] args)
        {
            // Warm up reflection.
			ConstructionViaContainer(1);

			outputEnabled = true;

            ConstructionWithoutContainer(NUMBER_OF_ITERATIONS);
            ConstructionViaContainer(NUMBER_OF_ITERATIONS);

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

        private static void ConstructionWithoutContainer(int iterations)
        {            
            Stopwatch stopwatch = Stopwatch.StartNew();

            for (int i = 0; i < iterations; i++)
            {
                var foo = new Foo(new Bar(new Baz()), new Baz());
            }

            stopwatch.Stop();

			if (outputEnabled)
				Console.WriteLine("ConstructionWithoutContainer: " + stopwatch.Elapsed);
        }

        private static void ConstructionViaContainer(int iterations)
        {
            ComponentContainer container = new ComponentContainer();
            container.Bind<IFoo>().To<Foo>().AsTransient();
            container.Bind<IBar>().To<Bar>().AsTransient();
            container.Bind<IBaz>().To<Baz>().AsTransient();

            Stopwatch stopwatch = Stopwatch.StartNew();

            for (int i = 0; i < iterations; i++)
            {
                var foo = container.Resolve<IFoo>();
            }

            stopwatch.Stop();

			if (outputEnabled)
				Console.WriteLine("ConstructionViaContainer: " + stopwatch.Elapsed);
        }
    }
}
