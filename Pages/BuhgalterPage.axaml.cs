using AntiqueShopAvalonia.Pages.AdminPage.AdminTabs;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using System;

namespace AntiqueShopAvalonia;

public partial class BuhgalterPage : UserControl
{
    private readonly int _userId;

    private readonly Action _logoutAction;
    private ContentControl? _reportsTabContent;

    public BuhgalterPage(int userId, Action logoutAction)
    {
        _userId = userId;
        _logoutAction = logoutAction;
        InitializeComponent();
        LoadTabs();
    }
    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);

        _reportsTabContent = this.FindControl<ContentControl>("ReportsTabContent");
    }
    private void LoadTabs()
    {

        _reportsTabContent!.Content = new Pages.AdminPage.AdminTabs.ReportsTab();
    }
    private void QuitBtn_Click(object? sender, RoutedEventArgs e)
    {
        _logoutAction?.Invoke();
    }
}