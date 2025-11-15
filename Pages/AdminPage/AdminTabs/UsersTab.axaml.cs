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

public partial class UsersTab : UserControl
{
	private DataGrid? _usersList;
	private TextBox? _usersSearchText;
	private TextBlock? _usersCountText;
	private System.Collections.Generic.List<User>? _allUsers;

	public UsersTab()
	{
		InitializeComponent();
		_ = LoadDataAsync();
	}

	private void InitializeComponent()
	{
		AvaloniaXamlLoader.Load(this);
		_usersList = this.FindControl<DataGrid>("UsersList");
		_usersSearchText = this.FindControl<TextBox>("UsersSearchText");
		_usersCountText = this.FindControl<TextBlock>("UsersCountText");
	}

	private async Task LoadDataAsync()
	{
		try
		{
			using var db = new AppDbContext();
			var users = await Task.Run(() =>
				db.Users
					.Include(u => u.Role)
					.OrderBy(u => u.Id)
					.ToList());

			_allUsers = users;
			
			if (_usersList != null)
			{
				_usersList.ItemsSource = null;
				_usersList.ItemsSource = users;
			}
			
			if (_usersCountText != null)
			{
				_usersCountText.Text = $"Всего работников: {users.Count}";
			}
		}
		catch (Exception ex)
		{
			// Log error or show message
			System.Diagnostics.Debug.WriteLine($"Error loading users: {ex.Message}");
		}
	}

	private void LoadData()
	{
		_ = LoadDataAsync();
	}

	private void UsersSearchText_TextChanged(object? sender, TextChangedEventArgs e)
	{
		if (_allUsers == null || _usersList == null) return;

		var searchText = _usersSearchText?.Text?.ToLower() ?? string.Empty;
		if (string.IsNullOrWhiteSpace(searchText))
		{
			_usersList.ItemsSource = _allUsers;
			return;
		}

		var filtered = _allUsers.Where(u =>
			(u.Lname?.ToLower().Contains(searchText) ?? false) ||
			(u.Fname?.ToLower().Contains(searchText) ?? false) ||
			(u.Patronymic?.ToLower().Contains(searchText) ?? false) ||
			(u.Phone?.ToLower().Contains(searchText) ?? false) ||
			(u.Email?.ToLower().Contains(searchText) ?? false) ||
			(u.Role?.Name?.ToLower().Contains(searchText) ?? false)
		).ToList();

		_usersList.ItemsSource = filtered;
	}

	private async void DeleteUserBtn_Click(object? sender, RoutedEventArgs e)
	{
		if (_usersList?.SelectedItem is not User selectedUser)
		{
			var msgBox = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Выберите работника!", ButtonEnum.Ok);
			await msgBox.ShowAsync();
			return;
		}

		var confirm = MessageBoxManager.GetMessageBoxStandard("Подтверждение", "Удалить данного работника?", ButtonEnum.YesNo);
		if (await confirm.ShowAsync() == ButtonResult.Yes)
		{
			using var db = new AppDbContext();
			db.Users.Remove(selectedUser);
			await db.SaveChangesAsync();
			LoadData();
		}
	}

	private async void EditUserBtn_Click(object? sender, RoutedEventArgs e)
	{
		if (_usersList?.SelectedItem is not User selectedUser)
		{
			var msgBox = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Выберите работника!", ButtonEnum.Ok);
			await msgBox.ShowAsync();
			return;
		}

		var editWindow = new AddOrEditUser(selectedUser);
		var parentWindow = (Window)TopLevel.GetTopLevel(this)!;
		await editWindow.ShowDialog(parentWindow);
		LoadData();
	}

	private async void AddUserBtn_Click(object? sender, RoutedEventArgs e)
	{
		var addWindow = new AddOrEditUser();
		var parentWindow = (Window)TopLevel.GetTopLevel(this)!;
		await addWindow.ShowDialog(parentWindow);
		LoadData();
	}

	private async void EditUser_Click(object? sender, RoutedEventArgs e)
	{
		if (sender is Button button && button.Tag is User user)
		{
			var editWindow = new AddOrEditUser(user);
			var parentWindow = (Window)TopLevel.GetTopLevel(this)!;
			await editWindow.ShowDialog(parentWindow);
			LoadData();
		}
	}

	private async void DeleteUser_Click(object? sender, RoutedEventArgs e)
	{
		if (sender is Button button && button.Tag is User user)
		{
			var confirm = MessageBoxManager.GetMessageBoxStandard("Подтверждение", "Удалить данного работника?", ButtonEnum.YesNo);
			if (await confirm.ShowAsync() == ButtonResult.Yes)
			{
				using var db = new AppDbContext();
				db.Users.Remove(user);
				await db.SaveChangesAsync();
				LoadData();
			}
		}
	}
}

