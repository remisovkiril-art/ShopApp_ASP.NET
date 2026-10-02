namespace ShopApi.Exceptions;

public class ValidationAppException : Exception
{
    public object Errors { get; }

    public ValidationAppException(object errors)
        : base("Validation error")
    {
        Errors = errors;
    }
}