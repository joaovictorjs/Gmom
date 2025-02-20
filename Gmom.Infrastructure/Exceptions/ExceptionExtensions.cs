namespace Gmom.Infrastructure.Exceptions;

public static class ExceptionExtensions
{
    public static void UseGlobalHandle(this Exception exception, Action<string> action)
    {
        GlobalExceptionHandler.Handle(exception, action);
    }
}