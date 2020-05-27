using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Innergy.Demo.Domain;

namespace Innergy.Demo.Services.Parsing
{
    public class Tokenizer : ITokenizer
    {
        private const string IdRegex = @"^[A-Za-z0-9]{2,8}-[\w-]{2,}";
        private const string WarehouseRegex = @"WH-[A-C]{1},[-\d.]+";
        private readonly Regex _idRegex;

        private readonly TextReader _reader;
        private readonly Regex _warehouseRegex;
        private char _currentChar;

        public Tokenizer(TextReader reader)
        {
            _reader = reader;
            _idRegex = new Regex(IdRegex);
            _warehouseRegex = new Regex(WarehouseRegex);

            NextChar();
            NextToken();
        }

        public char Delimiter => ';';

        public Token Token { get; private set; }

        public string Value { get; private set; }

        public void NextToken()
        {
            while (_currentChar == Delimiter)
            {
                NextChar();
            }

            if (_currentChar == '\0')
            {
                Token = Token.EOL;
                NextChar();
            }

            var allowedChars = new[] {'|'};
            if ((char.IsLetterOrDigit(_currentChar) || char.IsWhiteSpace(_currentChar) ||
                 char.IsSeparator(_currentChar) || char.IsPunctuation(_currentChar) ||
                 allowedChars.Contains(_currentChar)) && _currentChar != Delimiter)
            {
                var sb = new StringBuilder();
                while ((char.IsLetterOrDigit(_currentChar) || char.IsWhiteSpace(_currentChar) ||
                        char.IsSeparator(_currentChar) || char.IsPunctuation(_currentChar) ||
                        allowedChars.Contains(_currentChar)) && _currentChar != Delimiter)
                {
                    sb.Append(_currentChar);
                    NextChar();
                }

                Value = sb.ToString();
                Token = Token.ProductName;

                if (Value.StartsWith("#"))
                {
                    Token = Token.Comment;
                    return;
                }

                if (_idRegex.IsMatch(Value))
                {
                    Token = Token.ProductId;
                    return;
                }

                if (_warehouseRegex.IsMatch(Value))
                {
                    Token = Token.WarehouseInfo;
                }
            }

            //throw new InvalidDataException($"Unexpected character: {_currentChar}");
        }

        private void NextChar()
        {
            var ch = _reader.Read();
            _currentChar = ch < 0 ? '\0' : (char) ch;
        }
    }

    public class WarehouseInfoTokenizer 
    {
        private const string IdRegex = @"[A-Za-z0-9]{2,6}-[\w-]{2,}";
        private const string WarehouseRegex = @"WH-[A-C]{1},[-\d.]+";
        private readonly Regex _idRegex;

        private readonly TextReader _reader;
        private readonly Regex _warehouseRegex;
        private char _currentChar;

        public WarehouseInfoTokenizer(TextReader reader)
        {
            _reader = reader;
            _idRegex = new Regex(IdRegex);
            _warehouseRegex = new Regex(WarehouseRegex);

            NextChar();
            NextToken();
        }

        public char Delimiter => ';';

        public Token Token { get; private set; }

        public string Value { get; private set; }

        public void NextToken()
        {
            while (_currentChar == Delimiter)
            {
                NextChar();
            }

            if (_currentChar == '\0')
            {
                Token = Token.EOL;
                NextChar();
            }

            var allowedChars = new[] { '|' };
            if ((char.IsLetterOrDigit(_currentChar) || char.IsWhiteSpace(_currentChar) ||
                 char.IsSeparator(_currentChar) || char.IsPunctuation(_currentChar) ||
                 allowedChars.Contains(_currentChar)) && _currentChar != Delimiter)
            {
                var sb = new StringBuilder();
                while ((char.IsLetterOrDigit(_currentChar) || char.IsWhiteSpace(_currentChar) ||
                        char.IsSeparator(_currentChar) || char.IsPunctuation(_currentChar) ||
                        allowedChars.Contains(_currentChar)) && _currentChar != Delimiter)
                {
                    sb.Append(_currentChar);
                    NextChar();
                }

                Value = sb.ToString();
                Token = Token.ProductName;

                if (Value.StartsWith("#"))
                {
                    Token = Token.Comment;
                    return;
                }

                if (_idRegex.IsMatch(Value))
                {
                    Token = Token.ProductId;
                    return;
                }

                if (_warehouseRegex.IsMatch(Value))
                {
                    Token = Token.WarehouseInfo;
                }
            }

            //throw new InvalidDataException($"Unexpected character: {_currentChar}");
        }

        private void NextChar()
        {
            var ch = _reader.Read();
            _currentChar = ch < 0 ? '\0' : (char)ch;
        }
    }
}