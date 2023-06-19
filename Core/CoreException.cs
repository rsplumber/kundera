namespace Core;

public class CoreException : ApplicationException
{
    protected CoreException(int code, string message) : base(message)
    {
        Code = code;
    }

    public int Code { get; }
}