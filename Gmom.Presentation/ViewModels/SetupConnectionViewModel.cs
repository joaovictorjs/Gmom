﻿using System.Windows;
using Gmom.Domain.Interface;
using Gmom.Infrastructure.Exceptions;

namespace Gmom.Presentation.ViewModels;

public class SetupConnectionViewModel : BindableBase
{
    private readonly IMigrationService _migrationService;
    private readonly IMessageBoxService _messageBoxService;

    private string _host = string.Empty;
    private string _port = string.Empty;
    private string _database = string.Empty;
    private string _username = string.Empty;
    private string _password = string.Empty;
    private bool _isLoading;

    public string Host
    {
        get => _host;
        set => SetProperty(ref _host, value);
    }
    public string Port
    {
        get => _port;
        set => SetProperty(ref _port, value);
    }
    public string Database
    {
        get => _database;
        set => SetProperty(ref _database, value);
    }
    public string Username
    {
        get => _username;
        set => SetProperty(ref _username, value);
    }
    public string Password
    {
        get => _password;
        set => SetProperty(ref _password, value);
    }
    public bool IsLoading
    {
        get => _isLoading;
        set => SetProperty(ref _isLoading, value);
    }

    public AsyncDelegateCommand SaveCommand { get; }

    public SetupConnectionViewModel(
        IMessageBoxService messageBoxService,
        IMigrationService migrationService
    )
    {
        _messageBoxService = messageBoxService;
        _migrationService = migrationService;
        
        SaveCommand = new AsyncDelegateCommand(Save, CanSave);
    }

    private bool CanSave()
    {
        return !string.IsNullOrWhiteSpace(Host)
            && !string.IsNullOrWhiteSpace(Port)
            && !string.IsNullOrWhiteSpace(Database)
            && !string.IsNullOrWhiteSpace(Username)
            && !string.IsNullOrWhiteSpace(Password)
            && !IsLoading;
    }

    private async Task Save()
    {
        try
        {
            await _migrationService.MigrateAsync();
        }
        catch (Exception ex)
        {
            ex.UseGlobalHandle(_messageBoxService.ShowError);
        }
    }

    protected override bool SetProperty<T>(ref T storage, T value, string? propertyName = null)
    {
        SaveCommand.RaiseCanExecuteChanged();
        return base.SetProperty(ref storage, value, propertyName);
    }
}
