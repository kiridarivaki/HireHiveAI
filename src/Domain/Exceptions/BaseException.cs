namespace HireHive.Domain.Exceptions
{
    public abstract class BaseException : Exception
    {
        public ExceptionType Type { get; }

        protected BaseException(string? message = "") : base(message)
        {
            Type = ExceptionType.Default;
        }

        protected BaseException(ExceptionType type, string? message = "") : base(message)
        {
            Type = type;
        }
    }
}
