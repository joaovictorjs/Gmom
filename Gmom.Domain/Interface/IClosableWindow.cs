namespace Gmom.Domain.Interface;

public interface IClosableWindow
{
    Action? Close { get; set; }
}