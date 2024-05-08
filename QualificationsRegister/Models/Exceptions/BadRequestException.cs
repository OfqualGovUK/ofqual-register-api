namespace Ofqual.Common.RegisterAPI.Models.Exceptions
{
    public class BadRequestException : Exception
    {
        public BadRequestException(string message): base(message) { }
    }
}
