using System;
using System.IO;

namespace Innergy.Demo.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var filePath = Path.Combine(@"C:\tmp", "input.txt");

            using(var file = File.OpenText(filePath)){
                System.Console.WriteLine(file.ReadToEnd());
            }

            System.Console.ReadLine();
        }
    }
}
