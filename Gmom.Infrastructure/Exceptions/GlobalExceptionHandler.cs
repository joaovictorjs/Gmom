using System.Runtime.ExceptionServices;

namespace Gmom.Infrastructure.Exceptions;

public abstract class GlobalExceptionHandler
{
    public static void Handle(Exception exception, Action<string> action)
    {
        switch (exception)
        {
            default:
                ExceptionDispatchInfo.Capture(exception).Throw();
                break;
        }
    }
}
