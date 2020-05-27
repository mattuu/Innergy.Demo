namespace Innergy.Demo.Domain
{
    public interface ITokenizer
    {
        Token Token { get; }

        string Value { get; }
        void NextToken();
    }
}