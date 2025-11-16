using AntiqueShopAvalonia.Pages.AdminPage.AdminTabs;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using System;

namespace AntiqueShopAvalonia;

public partial class ManagerPage : UserControl
{
    private readonly int _userId;
    private readonly Action _logoutAction;
    private ContentControl? _usersTabContent;
    private ContentControl? _productsTabContent;
    private ContentControl? _salesTabContent;
    private ContentControl? _returnsTabContent;

    public ManagerPage(int userId, Action logoutAction)
    {
        _userId = userId;
        _logoutAction = logoutAction;
        InitializeComponent();
        LoadTabs();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
        _usersTabContent = this.FindControl<ContentControl>("UsersTabContent");
        _productsTabContent = this.FindControl<ContentControl>("ProductsTabContent");
        _salesTabContent = this.FindControl<ContentControl>("SalesTabContent");
        _returnsTabContent = this.FindControl<ContentControl>("ReturnsTabContent");
    }

    private void LoadTabs()
    {
        _usersTabContent!.Content = new UsersTab();
        _productsTabContent!.Content = new Pages.AdminPage.AdminTabs.ProductsTab();
        _salesTabContent!.Content = new Pages.AdminPage.AdminTabs.SalesTab();
        _returnsTabContent!.Content = new Pages.AdminPage.AdminTabs.ReturnsTab();
    }

    private void QuitBtn_Click(object? sender, RoutedEventArgs e)
    {
        _logoutAction?.Invoke();
    }
}