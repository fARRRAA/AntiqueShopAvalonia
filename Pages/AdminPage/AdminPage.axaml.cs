using AntiqueShopAvalonia.Pages.AdminPage.AdminTabs;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using System;

namespace AntiqueShopAvalonia;

public partial class AdminPage : UserControl
{
	private readonly int _userId;
	private readonly Action _logoutAction;
	private ContentControl? _usersTabContent;
	private ContentControl? _clientsTabContent;
	private ContentControl? _productsTabContent;
	private ContentControl? _salesTabContent;
	private ContentControl? _returnsTabContent;
	private ContentControl? _reportsTabContent;

	public AdminPage(int userId, Action logoutAction)
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
		_clientsTabContent = this.FindControl<ContentControl>("ClientsTabContent");
		_productsTabContent = this.FindControl<ContentControl>("ProductsTabContent");
		_salesTabContent = this.FindControl<ContentControl>("SalesTabContent");
		_returnsTabContent = this.FindControl<ContentControl>("ReturnsTabContent");
		_reportsTabContent = this.FindControl<ContentControl>("ReportsTabContent");
	}

	private void LoadTabs()
	{
		_usersTabContent!.Content = new UsersTab();
		_clientsTabContent!.Content = new Pages.AdminPage.AdminTabs.ClientsTab();
		_productsTabContent!.Content = new Pages.AdminPage.AdminTabs.ProductsTab();
		_salesTabContent!.Content = new Pages.AdminPage.AdminTabs.SalesTab();
		_returnsTabContent!.Content = new Pages.AdminPage.AdminTabs.ReturnsTab();
		_reportsTabContent!.Content = new Pages.AdminPage.AdminTabs.ReportsTab();
	}

	private void QuitBtn_Click(object? sender, RoutedEventArgs e)
	{
		_logoutAction?.Invoke();
	}
}
