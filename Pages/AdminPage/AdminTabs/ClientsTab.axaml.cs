using AntiqueShopAvalonia.Model;
using AntiqueShopAvalonia.Windows;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia;
using Microsoft.EntityFrameworkCore;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using System;
using System.Linq;
using System.Threading.Tasks;
using AppDbContext = AntiqueShopAvalonia.Model.AppDbContext;

namespace AntiqueShopAvalonia.Pages.AdminPage.AdminTabs;

public partial class ClientsTab : UserControl
{
	private DataGrid? _clientsList;
	private TextBox? _clientsSearchText;
	private TextBlock? _clientsCountText;
	private System.Collections.Generic.List<Client>? _allClients;

	public ClientsTab()
	{
		InitializeComponent();
		_ = LoadDataAsync();
	}

	private void InitializeComponent()
	{
		AvaloniaXamlLoader.Load(this);
		_clientsList = this.FindControl<DataGrid>("ClientsList");
		_clientsSearchText = this.FindControl<TextBox>("ClientsSearchText");
		_clientsCountText = this.FindControl<TextBlock>("ClientsCountText");
	}

	private async Task LoadDataAsync()
	{
		try
		{
			using var db = new AppDbContext();
			var clients = await Task.Run(() =>
				db.Clients
					.OrderBy(c => c.Id)
					.ToList());

			_allClients = clients;
			
			if (_clientsList != null)
			{
				_clientsList.ItemsSource = null;
				_clientsList.ItemsSource = clients;
			}
			
			if (_clientsCountText != null)
			{
				_clientsCountText.Text = $"Всего клиентов: {clients.Count}";
			}
		}
		catch (Exception ex)
		{
			System.Diagnostics.Debug.WriteLine($"Error loading clients: {ex.Message}");
		}
	}

	private void ClientsSearchText_TextChanged(object? sender, TextChangedEventArgs e)
	{
		if (_allClients == null || _clientsList == null) return;

		var searchText = _clientsSearchText?.Text?.ToLower() ?? string.Empty;
		if (string.IsNullOrWhiteSpace(searchText))
		{
			_clientsList.ItemsSource = _allClients;
			return;
		}

		var filtered = _allClients.Where(c =>
			(c.Lname?.ToLower().Contains(searchText) ?? false) ||
			(c.Fname?.ToLower().Contains(searchText) ?? false) ||
			(c.Patronymic?.ToLower().Contains(searchText) ?? false) ||
			(c.Phone?.ToLower().Contains(searchText) ?? false)
		).ToList();

		_clientsList.ItemsSource = filtered;
	}

	private async void DeleteClientBtn_Click(object? sender, RoutedEventArgs e)
	{
		if (_clientsList?.SelectedItem is not Client selectedClient)
		{
			var msgBox = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Выберите клиента!", ButtonEnum.Ok);
			await msgBox.ShowAsync();
			return;
		}

		var confirm = MessageBoxManager.GetMessageBoxStandard("Подтверждение", "Удалить данного клиента?", ButtonEnum.YesNo);
		if (await confirm.ShowAsync() == ButtonResult.Yes)
		{
			using var db = new AppDbContext();
			db.Clients.Remove(selectedClient);
			await db.SaveChangesAsync();
			await LoadDataAsync();
		}
	}

	private async void EditClientBtn_Click(object? sender, RoutedEventArgs e)
	{
		if (_clientsList?.SelectedItem is not Client selectedClient)
		{
			var msgBox = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Выберите клиента!", ButtonEnum.Ok);
			await msgBox.ShowAsync();
			return;
		}

		var editWindow = new AddOrEditClient(selectedClient);
		var parentWindow = (Window)TopLevel.GetTopLevel(this)!;
		await editWindow.ShowDialog(parentWindow);
		await LoadDataAsync();
	}

	private async void AddClientBtn_Click(object? sender, RoutedEventArgs e)
	{
		var addWindow = new AddOrEditClient();
		var parentWindow = (Window)TopLevel.GetTopLevel(this)!;
		await addWindow.ShowDialog(parentWindow);
		await LoadDataAsync();
	}

	private async void EditClient_Click(object? sender, RoutedEventArgs e)
	{
		if (sender is Button button && button.Tag is Client client)
		{
			var editWindow = new AddOrEditClient(client);
			var parentWindow = (Window)TopLevel.GetTopLevel(this)!;
			await editWindow.ShowDialog(parentWindow);
			await LoadDataAsync();
		}
	}

	private async void DeleteClient_Click(object? sender, RoutedEventArgs e)
	{
		if (sender is Button button && button.Tag is Client client)
		{
			var confirm = MessageBoxManager.GetMessageBoxStandard("Подтверждение", "Удалить данного клиента?", ButtonEnum.YesNo);
			if (await confirm.ShowAsync() == ButtonResult.Yes)
			{
				using var db = new AppDbContext();
				db.Clients.Remove(client);
				await db.SaveChangesAsync();
				await LoadDataAsync();
			}
		}
	}
}

