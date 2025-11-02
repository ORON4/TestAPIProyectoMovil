namespace TestAPI.Exceptions
{
    public class ValidationException : Exception
    {
        public ValidationException(string mensaje) : base(mensaje) { }
    }
}
