using Gmom.Domain.Exceptions;

namespace Gmom.Infrastructure.Exceptions;

public partial class GlobalExceptionHandler
{
    private static void HandleCurrentUserDeleteException(CurrentUserDeleteException currentUserDeleteException, Action<string> action)
    {
        action(currentUserDeleteException.Message);
    }
}