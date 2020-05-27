using System;
using Innergy.Demo.Domain;

namespace Innergy.Demo.Services.Output.Writers
{
    public class ConsoleWriterStrategy : IWriterStrategy
    {
        public void WriteLine(string line)
        {
            Console.WriteLine(line);
        }
    }
}