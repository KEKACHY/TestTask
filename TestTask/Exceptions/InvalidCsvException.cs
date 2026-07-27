namespace TestTask.Exceptions
{
    public class InvalidCsvException : Exception
    {
        public InvalidCsvException(string message)
            : base(message)
        {
        }
    }
}
