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

public partial class AddOrEditProduct : Window
{
	private Product _currentProduct;
	private ComboBox? _categoryComboBox;
	private ComboBox? _statusComboBox;
	private DatePicker? _receiptDatePicker;

	public AddOrEditProduct()
	{
		_currentProduct = new Product();
		InitializeComponent();
		DataContext = _currentProduct;
		LoadComboBoxes();
		SetDefaultValues();
	}

	public AddOrEditProduct(Product product)
	{
		_currentProduct = product;
		InitializeComponent();
		DataContext = _currentProduct;
		LoadComboBoxes();
		Title = "Редактирование товара";
	}

	private void InitializeComponent()
	{
		AvaloniaXamlLoader.Load(this);
		_categoryComboBox = this.FindControl<ComboBox>("CategoryComboBox");
		_statusComboBox = this.FindControl<ComboBox>("StatusComboBox");
		_receiptDatePicker = this.FindControl<DatePicker>("ReceiptDatePicker");
	}

	private async void LoadComboBoxes()
	{
		using var db = new AppDbContext();
		try
		{
			var categories = await Task.Run(() => db.ProductTypes.ToList());
			_categoryComboBox!.ItemsSource = categories;

			var statuses = await Task.Run(() => db.ProductStatuses.ToList());
			_statusComboBox!.ItemsSource = statuses;

			if (_currentProduct.Id != 0)
			{
				_categoryComboBox.SelectedItem = categories.FirstOrDefault(c => c.Id == _currentProduct.CategoryId);
				_statusComboBox.SelectedItem = statuses.FirstOrDefault(s => s.Id == _currentProduct.StatusId);
			}
		}
		catch (Exception ex)
		{
			var msgBox = MessageBoxManager.GetMessageBoxStandard("Ошибка", $"Ошибка при загрузке данных:\n{ex.Message}", ButtonEnum.Ok);
			await msgBox.ShowAsync();
		}
	}

	private void SetDefaultValues()
	{
		if (_currentProduct.Id == 0)
		{
			if (_receiptDatePicker != null)
			{
				_receiptDatePicker.SelectedDate = DateTime.Today;
			}
			if (_statusComboBox != null && _statusComboBox.ItemsSource != null)
			{
				var statuses = _statusComboBox.ItemsSource.Cast<ProductStatus>().ToList();
				if (statuses.Any())
					_statusComboBox.SelectedItem = statuses.First();
			}
		}
		else if (_currentProduct.ReceiptDate.HasValue && _receiptDatePicker != null)
		{
			var date = _currentProduct.ReceiptDate.Value;
			_receiptDatePicker.SelectedDate = new DateTime(date.Year, date.Month, date.Day);
		}
	}

	private void CloseBtn_Click(object? sender, RoutedEventArgs e)
	{
		Close();
	}

	private async void ConfirmBtn_Click(object? sender, RoutedEventArgs e)
	{
		if (string.IsNullOrWhiteSpace(_currentProduct.Name))
		{
			var msgBox = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Введите название", ButtonEnum.Ok);
			await msgBox.ShowAsync();
			return;
		}

		if (_categoryComboBox?.SelectedItem is not ProductType selectedCategory)
		{
			var msgBox = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Выберите категорию", ButtonEnum.Ok);
			await msgBox.ShowAsync();
			return;
		}

		if (!_currentProduct.Price.HasValue || _currentProduct.Price <= 0)
		{
			var msgBox = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Введите корректную цену", ButtonEnum.Ok);
			await msgBox.ShowAsync();
			return;
		}

		if (!_currentProduct.ShopShare.HasValue || _currentProduct.ShopShare <= 0)
		{
			var msgBox = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Введите корректную долю магазина", ButtonEnum.Ok);
			await msgBox.ShowAsync();
			return;
		}

		if (_receiptDatePicker?.SelectedDate == null)
		{
			var msgBox = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Выберите дату приема", ButtonEnum.Ok);
			await msgBox.ShowAsync();
			return;
		}

		var selectedDate = _receiptDatePicker.SelectedDate.Value;
		_currentProduct.ReceiptDate = new DateOnly(selectedDate.Year, selectedDate.Month, selectedDate.Day);

		if (!_currentProduct.StoragePeriod.HasValue || _currentProduct.StoragePeriod <= 0)
		{
			var msgBox = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Введите корректный срок хранения", ButtonEnum.Ok);
			await msgBox.ShowAsync();
			return;
		}

		if (_statusComboBox?.SelectedItem is not ProductStatus selectedStatus)
		{
			var msgBox = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Выберите статус", ButtonEnum.Ok);
			await msgBox.ShowAsync();
			return;
		}

		try
		{
			_currentProduct.CategoryId = selectedCategory.Id;
			_currentProduct.StatusId = selectedStatus.Id;

			using var db = new AppDbContext();

			if (_currentProduct.Id == 0)
			{
				db.Products.Add(_currentProduct);
			}
			else
			{
				var dbProduct = await Task.Run(async () => await db.Products.FirstOrDefaultAsync(p => p.Id == _currentProduct.Id));
				if (dbProduct != null)
				{
					dbProduct.Name = _currentProduct.Name;
					dbProduct.CategoryId = _currentProduct.CategoryId;
					dbProduct.Description = _currentProduct.Description;
					dbProduct.Price = _currentProduct.Price;
					dbProduct.ShopShare = _currentProduct.ShopShare;
					dbProduct.ReceiptDate = _currentProduct.ReceiptDate;
					dbProduct.StoragePeriod = _currentProduct.StoragePeriod;
					dbProduct.StatusId = _currentProduct.StatusId;
					db.Products.Update(dbProduct);
				}
			}

			await db.SaveChangesAsync();

			var successMsg = MessageBoxManager.GetMessageBoxStandard("Успех", 
				_currentProduct.Id == 0 ? "Товар успешно добавлен" : "Товар успешно обновлен", 
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

