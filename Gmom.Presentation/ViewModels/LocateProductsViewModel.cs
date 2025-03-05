﻿using System.Windows;
using Gmom.Domain.Interface;
using Gmom.Presentation.Views;

namespace Gmom.Presentation.ViewModels;

public class LocateProductsViewModel : BindableBase
{
    private readonly IWindowService<
        InsertOrUpdateProductView,
        InsertOrUpdateProductViewModel
    > _productUpdateWindowService;
    public DelegateCommand InsertProductCommand { get; }

    public LocateProductsViewModel(
        IWindowService<InsertOrUpdateProductView, InsertOrUpdateProductViewModel> productUpdateWindowService
    )
    {
        _productUpdateWindowService = productUpdateWindowService;
        
        InsertProductCommand = new DelegateCommand(InsertProduct);
    }

    private void InsertProduct()
    {
        _productUpdateWindowService.Create().ShowDialog();
    }
}
