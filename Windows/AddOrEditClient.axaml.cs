using AntiqueShopAvalonia.Model;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Microsoft.EntityFrameworkCore;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using System;
using System.Linq;
using System.Threading.Tasks;
using AppDbContext = AntiqueShopAvalonia.Model.AppDbContext;

namespace AntiqueShopAvalonia.Windows;

public partial class AddOrEditClient : Window
{
	private Client _currentClient;

	public AddOrEditClient()
	{
		_currentClient = new Client { RoleId = 3 }; // RoleId = 3 для клиентов
		InitializeComponent();
		DataContext = _currentClient;
	}

	public AddOrEditClient(Client client)
	{
		_currentClient = client;
		InitializeComponent();
		DataContext = _currentClient;
		Title = "Редактирование клиента";
	}

	private void InitializeComponent()
	{
		AvaloniaXamlLoader.Load(this);
	}

	private void CloseBtn_Click(object? sender, RoutedEventArgs e)
	{
		Close();
	}

	private async void ConfirmBtn_Click(object? sender, RoutedEventArgs e)
	{
		if (string.IsNullOrWhiteSpace(_currentClient.Lname))
		{
			var msgBox = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Введите фамилию", ButtonEnum.Ok);
			await msgBox.ShowAsync();
			return;
		}

		if (string.IsNullOrWhiteSpace(_currentClient.Fname))
		{
			var msgBox = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Введите имя", ButtonEnum.Ok);
			await msgBox.ShowAsync();
			return;
		}

		if (string.IsNullOrWhiteSpace(_currentClient.Phone))
		{
			var msgBox = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Введите телефон", ButtonEnum.Ok);
			await msgBox.ShowAsync();
			return;
		}

		if (string.IsNullOrWhiteSpace(_currentClient.Email))
		{
			var msgBox = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Введите email", ButtonEnum.Ok);
			await msgBox.ShowAsync();
			return;
		}

		if (string.IsNullOrWhiteSpace(_currentClient.PassportData))
		{
			var msgBox = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Введите паспортные данные", ButtonEnum.Ok);
			await msgBox.ShowAsync();
			return;
		}

		if (string.IsNullOrWhiteSpace(_currentClient.BankDetail))
		{
			var msgBox = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Введите банковские данные", ButtonEnum.Ok);
			await msgBox.ShowAsync();
			return;
		}

		using var db = new AppDbContext();

		// Проверка уникальности
		var existingClient = await Task.Run(async () =>
			await db.Clients.FirstOrDefaultAsync(c => 
				(c.Phone == _currentClient.Phone || 
				 c.PassportData == _currentClient.PassportData || 
				 c.BankDetail == _currentClient.BankDetail) &&
				c.Id != _currentClient.Id));

		if (existingClient != null)
		{
			var msgBox = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Клиент с таким телефоном, паспортом или банковскими данными уже существует", ButtonEnum.Ok);
			await msgBox.ShowAsync();
			return;
		}

		try
		{
			if (_currentClient.Id == 0)
			{
				_currentClient.RoleId = 3; // RoleId = 3 для клиентов
				db.Clients.Add(_currentClient);
			}
			else
			{
				var dbClient = await Task.Run(async () => await db.Clients.FirstOrDefaultAsync(c => c.Id == _currentClient.Id));
				if (dbClient != null)
				{
					dbClient.Fname = _currentClient.Fname;
					dbClient.Lname = _currentClient.Lname;
					dbClient.Patronymic = _currentClient.Patronymic;
					dbClient.Phone = _currentClient.Phone;
					dbClient.Email = _currentClient.Email;
					dbClient.PassportData = _currentClient.PassportData;
					dbClient.BankDetail = _currentClient.BankDetail;
					db.Clients.Update(dbClient);
				}
			}

			await db.SaveChangesAsync();

			var successMsg = MessageBoxManager.GetMessageBoxStandard("Успех", 
				_currentClient.Id == 0 ? "Клиент успешно добавлен" : "Клиент успешно обновлен", 
				ButtonEnum.Ok);
			await successMsg.ShowAsync();
			Close();
		}
		catch (Exception ex)
		{
			var msgBox = MessageBoxManager.GetMessageBoxStandard("Ошибка", $"Произошла ошибка:\n{ex.Message}", ButtonEnum.Ok);
			await msgBox.ShowAsync();
		}
	}
}


