namespace Gmom.Domain.Interface;

public interface IMessageBoxService
{
    void ShowError(string message);
    void ShowInformation(string message);
    void ShowWarning(string message);
    bool ShowConfirmation(string message);
}