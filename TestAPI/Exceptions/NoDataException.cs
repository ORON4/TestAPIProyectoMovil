namespace TestAPI.Exceptions
{
    public class NoDataException : Exception

    {
        public NoDataException(string mensaje) : base(mensaje) { }
    }
}
