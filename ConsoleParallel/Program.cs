using System;
using System.Threading.Tasks;

namespace ConsoleParallel
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Parallel.For(0, 100, i =>
            {
                Console.WriteLine(i);
                Console.WriteLine($"Hello World! {i}");
            });
        }
    }
}
