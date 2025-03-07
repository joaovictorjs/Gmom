using Gmom.Domain.Exceptions;

namespace Gmom.Infrastructure.Exceptions;

public partial class GlobalExceptionHandler
{
    private static void HandleDuplicatedValueException(DuplicatedValueException duplicatedValueException, Action<string> action)
    {
        action(duplicatedValueException.Message);
    }
}