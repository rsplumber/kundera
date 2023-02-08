namespace Core;

public class KunderaException : ApplicationException
{
    public KunderaException(int code, string message)
    {
        Code = code;
        Message = message;
    }

    public int Code { get; }

    public new string Message { get; }
}