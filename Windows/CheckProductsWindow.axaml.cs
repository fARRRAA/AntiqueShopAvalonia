using AntiqueShopAvalonia.Model;
using AntiqueShopAvalonia.Pages.AdminPage.AdminTabs;
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

public partial class CheckProductsWindow : Window
{
	private readonly ReturnsTab? _returnsTab;
	private ComboBox? _productComboBox;
	private ComboBox? _returnTypeComboBox;
	private TextBox? _reasonTextBox;
	private System.Collections.Generic.List<Product>? _expiredProducts;

	public CheckProductsWindow(ReturnsTab? returnsTab = null)
	{
		_returnsTab = returnsTab;
		InitializeComponent();
		_ = LoadDataAsync();
	}

	private void InitializeComponent()
	{
		AvaloniaXamlLoader.Load(this);
		_productComboBox = this.FindControl<ComboBox>("ProductComboBox");
		_returnTypeComboBox = this.FindControl<ComboBox>("ReturnTypeComboBox");
		_reasonTextBox = this.FindControl<TextBox>("ReasonTextBox");
	}

	private async Task LoadDataAsync()
	{
		try
		{
			using var db = new AppDbContext();
			var currentDate = DateOnly.FromDateTime(DateTime.Now);

			_expiredProducts = await Task.Run(() =>
			{
				var products = db.Products
					.Where(p => p.StatusId == 1 && // Только товары в наличии
						p.ReceiptDate.HasValue &&
						p.StoragePeriod.HasValue)
					.ToList();

				// Фильтруем товары с истекшим сроком хранения
				return products
					.Where(p => p.ReceiptDate!.Value.AddDays(p.StoragePeriod!.Value) < currentDate)
					.ToList();
			});

			_productComboBox!.ItemsSource = _expiredProducts;

			var returnTypes = await Task.Run(() => db.ReturnTypes.ToList());
			_returnTypeComboBox!.ItemsSource = returnTypes;

			if (!_expiredProducts.Any())
			{
				var msgBox = MessageBoxManager.GetMessageBoxStandard("Информация", "Нет товаров с истекшим сроком хранения", ButtonEnum.Ok);
				await msgBox.ShowAsync();
				Close();
			}
		}
		catch (Exception ex)
		{
			var msgBox = MessageBoxManager.GetMessageBoxStandard("Ошибка", $"Ошибка при загрузке данных:\n{ex.Message}", ButtonEnum.Ok);
			await msgBox.ShowAsync();
		}
	}

	private void CancelBtn_Click(object? sender, RoutedEventArgs e)
	{
		Close();
	}

	private async void CreateReturnBtn_Click(object? sender, RoutedEventArgs e)
	{
		if (_productComboBox?.SelectedItem is not Product selectedProduct)
		{
			var msgBox = MessageBoxManager.GetMessageBoxStandard("Предупреждение", "Выберите товар", ButtonEnum.Ok);
			await msgBox.ShowAsync();
			return;
		}

		if (_returnTypeComboBox?.SelectedItem is not ReturnType selectedReturnType)
		{
			var msgBox = MessageBoxManager.GetMessageBoxStandard("Предупреждение", "Выберите тип возврата", ButtonEnum.Ok);
			await msgBox.ShowAsync();
			return;
		}

		if (string.IsNullOrWhiteSpace(_reasonTextBox?.Text))
		{
			var msgBox = MessageBoxManager.GetMessageBoxStandard("Предупреждение", "Введите причину возврата", ButtonEnum.Ok);
			await msgBox.ShowAsync();
			return;
		}

		try
		{
			using var db = new AppDbContext();

			var returnItem = new Return
			{
				ProductId = selectedProduct.Id,
				ReturnTypeId = selectedReturnType.Id,
				ReturnDate = DateOnly.FromDateTime(DateTime.Now),
				Reason = _reasonTextBox.Text.Trim()
			};

			// Меняем статус товара в зависимости от типа возврата
			// Предполагаем: 3 - Возврат собственнику, 4 - Списание
			var product = await Task.Run(async () => await db.Products.FirstOrDefaultAsync(p => p.Id == selectedProduct.Id));
			if (product != null)
			{
				product.StatusId = selectedReturnType.Name == "Списание" ? 4 : 3;
				db.Products.Update(product);
			}

			db.Returns.Add(returnItem);
			await db.SaveChangesAsync();

			if (_returnsTab != null)
			{
				await _returnsTab.LoadDataAsync();
			}

			var successMsg = MessageBoxManager.GetMessageBoxStandard("Успех", "Возврат успешно создан", ButtonEnum.Ok);
			await successMsg.ShowAsync();
			Close();
		}
		catch (Exception ex)
		{
			var msgBox = MessageBoxManager.GetMessageBoxStandard("Ошибка", $"Ошибка при создании возврата:\n{ex.Message}", ButtonEnum.Ok);
			await msgBox.ShowAsync();
		}
	}
}

