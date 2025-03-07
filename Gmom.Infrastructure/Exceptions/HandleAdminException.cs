using Gmom.Domain.Exceptions;

namespace Gmom.Infrastructure.Exceptions;

public partial class GlobalExceptionHandler
{
    private static void HandleAdminException(AdminException adminException, Action<string> action)
    {
       action(adminException.Message);
    }
}