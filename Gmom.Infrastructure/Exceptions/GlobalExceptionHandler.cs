using System.Runtime.ExceptionServices;
using Gmom.Domain.Exceptions;

namespace Gmom.Infrastructure.Exceptions;

public abstract partial class GlobalExceptionHandler
{
    public static void Handle(Exception exception, Action<string> action)
    {
        switch (exception)
        {
            case CurrentUserDeleteException currentUserDeleteException:
                HandleCurrentUserDeleteException(currentUserDeleteException, action);
                break;
            case AdminException adminException:
                HandleAdminException(adminException, action);
                break;
            case DuplicatedValueException duplicatedValueException:
                HandleDuplicatedValueException(duplicatedValueException, action);
                break;
            case InvalidOperationException invalidOperationException:
                HandleInvalidOperationException(invalidOperationException, action);
                break;
            default:
                ExceptionDispatchInfo.Capture(exception).Throw();
                break;
        }
    }
}
