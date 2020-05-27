using System;
using System.IO;
using Innergy.Demo.Domain;

namespace Innergy.Demo.Services.Output.Writers
{
    public class TextFileWriterStrategy : IWriterStrategy, IDisposable
    {
        private readonly string _filePath;
        private bool _disposed;
        private StreamWriter _textWriter;

        public TextFileWriterStrategy(string filePath)
        {
            _filePath = filePath ?? throw new ArgumentNullException(nameof(filePath));
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                _textWriter.Dispose();
                _disposed = true;
            }
        }

        public void WriteLine(string line)
        {
            if (_textWriter == null)
            {
                _textWriter = new StreamWriter(File.OpenWrite(_filePath));
            }

            _textWriter.WriteLine(line);
            _textWriter.Flush();
        }
    }
}