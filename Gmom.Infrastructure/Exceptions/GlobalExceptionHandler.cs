using System.Runtime.ExceptionServices;

namespace Gmom.Infrastructure.Exceptions;

public abstract partial class GlobalExceptionHandler
{
    public static void Handle(Exception exception, Action<string> action)
    {
        switch (exception)
        {
            case InvalidOperationException invalidOperationException:
                HandleInvalidOperationException(invalidOperationException, action);
                break;
            default:
                ExceptionDispatchInfo.Capture(exception).Throw();
                break;
        }
    }
}