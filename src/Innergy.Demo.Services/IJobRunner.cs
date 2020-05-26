using System.IO;

namespace Innergy.Demo.Services
{
    public interface IJobRunner
    {
        void Run(StreamReader inputReader, StreamWriter outputWriter);
    }
}