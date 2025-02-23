namespace Gmom.Infrastructure.Exceptions;

public partial class GlobalExceptionHandler
{
    private static void HandleInvalidOperationException(
        InvalidOperationException invalidOperationException,
        Action<string> action
    )
    {
        action(invalidOperationException.Message);
    }
}