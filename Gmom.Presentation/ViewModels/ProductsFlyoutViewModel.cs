namespace Gmom.Presentation.ViewModels;

public class ProductsFlyoutViewModel : BindableBase
{
    public DelegateCommand InsertProductCommand { get; }

    public ProductsFlyoutViewModel()
    {
        InsertProductCommand = new DelegateCommand(InsertProduct);
    }

    private  void InsertProduct()
    {
        throw new NotImplementedException();
    }
}