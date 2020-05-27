using System.IO;

namespace Innergy.Demo.Services
{
    public interface IJobRunner
    {
        void Run(string source, StreamWriter outputWriter);
    }
}