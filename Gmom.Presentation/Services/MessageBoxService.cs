using System.Windows;
using Gmom.Domain.Interface;

namespace Gmom.Presentation.Services;

public class MessageBoxService : IMessageBoxService
{
    public void ShowError(string message)
    {
        MessageBox.Show(message, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
    }

    public void ShowInformation(string message)
    {
        MessageBox.Show(message, "Informação", MessageBoxButton.OK, MessageBoxImage.Information);
    }

    public void ShowWarning(string message)
    {
        MessageBox.Show(message, "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
    }

    public bool ShowConfirmation(string message)
    {
        return MessageBox.Show(
                message,
                "Confirmação",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question
            ) == MessageBoxResult.Yes;
    }
}
