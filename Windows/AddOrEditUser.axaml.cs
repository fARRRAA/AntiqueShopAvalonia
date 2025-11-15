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

public partial class AddOrEditUser : Window
{
	private User? _currentUser;
	private TextBox? _lastNameTextBox;
	private TextBox? _firstNameTextBox;
	private TextBox? _patronymicTextBox;
	private DatePicker? _dateBirthPicker;
	private TextBox? _phoneTextBox;
	private TextBox? _emailTextBox;
	private TextBox? _loginTextBox;
	private TextBox? _passwordTextBox;
	private ComboBox? _roleComboBox;
	private bool _isEditMode;

	public AddOrEditUser()
	{
		_currentUser = new User();
		_isEditMode = false;
		InitializeComponent();
		LoadComboBoxes();
	}

	public AddOrEditUser(User user)
	{
		_currentUser = user;
		_isEditMode = true;
		InitializeComponent();
		LoadComboBoxes();
		LoadUserData();
	}

	private void InitializeComponent()
	{
		AvaloniaXamlLoader.Load(this);
		_lastNameTextBox = this.FindControl<TextBox>("LastNameTextBox");
		_firstNameTextBox = this.FindControl<TextBox>("FirstNameTextBox");
		_patronymicTextBox = this.FindControl<TextBox>("PatronymicTextBox");
		_dateBirthPicker = this.FindControl<DatePicker>("DateBirthPicker");
		_phoneTextBox = this.FindControl<TextBox>("PhoneTextBox");
		_emailTextBox = this.FindControl<TextBox>("EmailTextBox");
		_loginTextBox = this.FindControl<TextBox>("LoginTextBox");
		_passwordTextBox = this.FindControl<TextBox>("PasswordTextBox");
		_roleComboBox = this.FindControl<ComboBox>("RoleComboBox");

		if (_isEditMode)
		{
			Title = "Редактирование работника";
		}
	}

	private async void LoadComboBoxes()
	{
		using var db = new AppDbContext();
		try
		{
			var roles = await Task.Run(() => db.Roles.ToList());
			_roleComboBox!.ItemsSource = roles;

			if (_isEditMode && _currentUser != null)
			{
				_roleComboBox.SelectedItem = roles.FirstOrDefault(r => r.Id == _currentUser.RoleId);
			}
		}
		catch (Exception ex)
		{
			var msgBox = MessageBoxManager.GetMessageBoxStandard("Ошибка", $"Ошибка при загрузке ролей:\n{ex.Message}", ButtonEnum.Ok);
			await msgBox.ShowAsync();
		}
	}

	private void LoadUserData()
	{
		if (_currentUser == null) return;

		_lastNameTextBox!.Text = _currentUser.Lname ?? string.Empty;
		_firstNameTextBox!.Text = _currentUser.Fname ?? string.Empty;
		_patronymicTextBox!.Text = _currentUser.Patronymic ?? string.Empty;
		_phoneTextBox!.Text = _currentUser.Phone ?? string.Empty;
		_emailTextBox!.Text = _currentUser.Email ?? string.Empty;
		_loginTextBox!.Text = _currentUser.Login ?? string.Empty;
		_passwordTextBox!.Text = _currentUser.Password ?? string.Empty;

		if (_currentUser.DateBirth.HasValue)
		{
			_dateBirthPicker!.SelectedDate = new DateTime(
				_currentUser.DateBirth.Value.Year,
				_currentUser.DateBirth.Value.Month,
				_currentUser.DateBirth.Value.Day
			);
		}
	}

	private void CloseBtn_Click(object? sender, RoutedEventArgs e)
	{
		Close();
	}

	private async void ConfirmBtn_Click(object? sender, RoutedEventArgs e)
	{
		var inputs = new[]
		{
			_loginTextBox?.Text,
			_lastNameTextBox?.Text,
			_firstNameTextBox?.Text,
			_patronymicTextBox?.Text,
			_phoneTextBox?.Text,
			_emailTextBox?.Text,
			_passwordTextBox?.Text
		};

		if (inputs.Any(string.IsNullOrWhiteSpace) )
		{
			var msgBox = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Заполните все поля", ButtonEnum.Ok);
			await msgBox.ShowAsync();
			return;
		}

		if (_roleComboBox?.SelectedItem is not Role selectedRole)
		{
			var msgBox = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Выберите роль", ButtonEnum.Ok);
			await msgBox.ShowAsync();
			return;
		}

		using var db = new AppDbContext();

		var existingUser =
			await db.Users.FirstOrDefaultAsync(u => u.Login == _loginTextBox!.Text && (!_isEditMode || u.Id != _currentUser!.Id));

		if (existingUser != null)
		{
			var msgBox = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Работник с таким логином уже существует", ButtonEnum.Ok);
			await msgBox.ShowAsync();
			return;
		}

		try
		{
			var selectedDate = _dateBirthPicker!.SelectedDate!.Value;
			var dateBirth = new DateOnly(selectedDate.Year, selectedDate.Month, selectedDate.Day);

			if (_isEditMode && _currentUser != null)
			{
				var dbUser = await Task.Run(async () => await db.Users.FirstOrDefaultAsync(u => u.Id == _currentUser.Id));
				if (dbUser != null)
				{
					dbUser.Fname = _firstNameTextBox!.Text;
					dbUser.Lname = _lastNameTextBox!.Text;
					dbUser.Patronymic = _patronymicTextBox!.Text;
					dbUser.Phone = _phoneTextBox!.Text;
					dbUser.Email = _emailTextBox!.Text;
					dbUser.Login = _loginTextBox!.Text;
					dbUser.Password = _passwordTextBox!.Text;
					dbUser.DateBirth = dateBirth;
					dbUser.RoleId = selectedRole.Id;
					db.Users.Update(dbUser);
				}
			}
			else
			{
				// Create new user
				_currentUser = new User
				{
					Fname = _firstNameTextBox!.Text,
					Lname = _lastNameTextBox!.Text,
					Patronymic = _patronymicTextBox!.Text,
					Phone = _phoneTextBox!.Text,
					Email = _emailTextBox!.Text,
					Login = _loginTextBox!.Text,
					Password = _passwordTextBox!.Text,
					DateBirth = dateBirth,
					RoleId = selectedRole.Id,
					IsActive = true
				};
				db.Users.Add(_currentUser);
			}

			await db.SaveChangesAsync();

			var successMsg = MessageBoxManager.GetMessageBoxStandard("Успех", 
				_isEditMode ? "Работник успешно обновлен" : "Работник успешно добавлен", 
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

