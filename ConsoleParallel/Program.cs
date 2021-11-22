using System;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleParallel
{
    internal class Program
    {
        static int valor = 0;

        static void Main(string[] args)
        {
            ThreadSample();
        }

        static void ThreadSample()
        {
            //Thread t = new Thread(() =>
            //{
            //    Console.WriteLine($"1.-Valor={valor}");
            //    valor++;
            //    Console.WriteLine($"2.-Valor={valor}");
            //});
            Thread t1 = new Thread(() => ThreadProc(1));
            Thread t2 = new Thread(() => ThreadProc(2));
            Thread t3 = new Thread(() => ThreadProc(3));

            t1.Start();
            t1.IsBackground = true;
            t2.Start();
            t2.IsBackground = true;
            t3.Start();



            Console.WriteLine($"3.-Valor={valor}");
            Console.WriteLine(valor);
            Console.WriteLine($"4.-Valor={valor}");
            //t1.Join();
            //t2.Join();
            //t3.Join();
            Console.WriteLine($"5.-Valor={valor}");
            Console.WriteLine(valor);
            Console.WriteLine($"6.-Valor={valor}");
        }

        static void ThreadProc(int j)
        {
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine("Thread: {1} - Iteración: {0}", i, j);
                Console.WriteLine($"1.-Valor={valor}");
                valor++;
                Interlocked.Increment(ref valor);
                Console.WriteLine($"2.-Valor={valor}");
                Thread.Sleep(1000);
            }
        }

        static void ParrallelSample()
        {
            Parallel.For(0, 100, i =>
            {
                Console.WriteLine(i);
                Console.WriteLine($"Hello World! {i}");
            });
        }

    }
}
