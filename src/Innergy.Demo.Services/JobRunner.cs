using System;
using System.IO;
using Innergy.Demo.Domain;

namespace Innergy.Demo.Services
{
    public class JobRunner : IJobRunner
    {
        private readonly IInputStrategy _inputStrategy;
        private readonly IDataProcessor _dataProcessor;
        private readonly IOutputWriterStrategy _outputWriterStrategy;

        public JobRunner(IInputStrategy inputStrategy, IDataProcessor dataProcessor, IOutputWriterStrategy outputWriterStrategy)
        {
            _inputStrategy = inputStrategy ?? throw new ArgumentNullException(nameof(inputStrategy));
            _dataProcessor = dataProcessor ?? throw new ArgumentNullException(nameof(dataProcessor));
            _outputWriterStrategy = outputWriterStrategy ?? throw new ArgumentNullException(nameof(outputWriterStrategy));
        }

        public void Run(string source, StreamWriter outputWriter)
        {
            var inputModels = _inputStrategy.Load(source);
         
            var outputModels = _dataProcessor.Process(inputModels);
            
            _outputWriterStrategy.Write(outputModels);
        }
    }
}