using System;
using System.IO;
using Innergy.Demo.Domain;

namespace Innergy.Demo.Services
{
    public class JobRunner : IJobRunner
    {
        private readonly IInputReader _inputReader;
        private readonly IDataProcessor _dataProcessor;
        private readonly IOutputWriter _outputWriter;

        public JobRunner(IInputReader inputReader, IDataProcessor dataProcessor, IOutputWriter outputWriter)
        {
            _inputReader = inputReader ?? throw new ArgumentNullException(nameof(inputReader));
            _dataProcessor = dataProcessor ?? throw new ArgumentNullException(nameof(dataProcessor));
            _outputWriter = outputWriter ?? throw new ArgumentNullException(nameof(outputWriter));
        }

        public void Run(StreamReader inputReader, StreamWriter outputWriter)
        {
            var inputModels = _inputReader.Parse(inputReader);
         
            var outputModels = _dataProcessor.Process(inputModels);
            
            _outputWriter.Write(outputWriter, outputModels);
        }
    }
}