﻿using System;
using Innergy.Demo.Domain;

namespace Innergy.Demo.Services
{
    public class JobRunner : IJobRunner
    {
        private readonly IInputStrategy _inputStrategy;
        private readonly IDataProcessor _dataProcessor;
        private readonly IOutputWriter _outputWriter;

        public JobRunner(IInputStrategy inputStrategy, IDataProcessor dataProcessor, IOutputWriter outputWriter)
        {
            _inputStrategy = inputStrategy ?? throw new ArgumentNullException(nameof(inputStrategy));
            _dataProcessor = dataProcessor ?? throw new ArgumentNullException(nameof(dataProcessor));
            _outputWriter = outputWriter ?? throw new ArgumentNullException(nameof(outputWriter));
        }

        public void Run()
        {
            var inputModels = _inputStrategy.Load();
         
            var outputModels = _dataProcessor.Process(inputModels);
            
            _outputWriter.Write(outputModels);
        }
    }
}