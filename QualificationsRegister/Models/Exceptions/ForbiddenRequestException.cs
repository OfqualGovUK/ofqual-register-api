namespace Ofqual.Common.RegisterAPI.Models.Exceptions
{
    public class ForbiddenRequestException : Exception
    {
        public ForbiddenRequestException(string message): base(message) { }
    }
}
