using AntiqueShopAvalonia.Model;
using Avalonia.Controls;
using Avalonia.Controls.Selection;
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

public partial class AddOrEditSale : Window
{
	private Sale? _currentSale;
	private ComboBox? _productComboBox;
	private ComboBox? _clientComboBox;
	private ComboBox? _paymentMethodComboBox;
	private TextBlock? _priceText;
	private TextBlock? _shopShareText;
	private TextBlock? _clientPayoutText;
	private decimal _shopShare;
	private decimal _total;
	private decimal _clientAmount;

	public AddOrEditSale()
	{
		_currentSale = new Sale();
		InitializeComponent();
		LoadComboBoxes();
	}

	public AddOrEditSale(Sale sale)
	{
		_currentSale = sale;
		InitializeComponent();
		LoadComboBoxes();
		LoadSaleData();
		Title = "Редактирование продажи";
	}

	private void InitializeComponent()
	{
		AvaloniaXamlLoader.Load(this);
		_productComboBox = this.FindControl<ComboBox>("ProductComboBox");
		_clientComboBox = this.FindControl<ComboBox>("ClientComboBox");
		_paymentMethodComboBox = this.FindControl<ComboBox>("PaymentMethodComboBox");
		_priceText = this.FindControl<TextBlock>("PriceText");
		_shopShareText = this.FindControl<TextBlock>("ShopShareText");
		_clientPayoutText = this.FindControl<TextBlock>("ClientPayoutText");
	}

	private async void LoadComboBoxes()
	{
		using var db = new AppDbContext();
		try
		{
			// При редактировании загружаем все товары, при создании - только в наличии
			var products = await Task.Run(() =>
			{
				var query = db.Products.Include(p => p.Status).AsQueryable();
				if (_currentSale == null || _currentSale.Id == 0)
				{
					// При создании - только товары в наличии
					query = query.Where(p => p.StatusId == 1);
				}
				return query.ToList();
			});
			_productComboBox!.ItemsSource = products;

			var clients = await Task.Run(() => db.Clients.ToList());
			_clientComboBox!.ItemsSource = clients;

			var paymentMethods = await Task.Run(() => db.PaymentMethods.ToList());
			_paymentMethodComboBox!.ItemsSource = paymentMethods;

			if (_currentSale != null && _currentSale.Id != 0)
			{
				_productComboBox.SelectedItem = products.FirstOrDefault(p => p.Id == _currentSale.ProductId);
				_clientComboBox.SelectedItem = clients.FirstOrDefault(c => c.Id == _currentSale.ClientId);
				_paymentMethodComboBox.SelectedItem = paymentMethods.FirstOrDefault(pm => pm.Id == _currentSale.PayMethodId);
			}
		}
		catch (Exception ex)
		{
			var msgBox = MessageBoxManager.GetMessageBoxStandard("Ошибка", $"Ошибка при загрузке данных:\n{ex.Message}", ButtonEnum.Ok);
			await msgBox.ShowAsync();
		}
	}

	private void LoadSaleData()
	{
		if (_currentSale == null) return;

		if (_currentSale.TotalAmount.HasValue)
			_total = _currentSale.TotalAmount.Value;
		if (_currentSale.ShopAmount.HasValue)
			_shopShare = _currentSale.ShopAmount.Value;
		if (_currentSale.ClientAmount.HasValue)
			_clientAmount = _currentSale.ClientAmount.Value;

		UpdateInfoText();
	}

	private void ProductComboBox_SelectionChanged(object? sender, SelectionChangedEventArgs e)
	{
		if (_productComboBox?.SelectedItem is Product selectedProduct)
		{
			if (selectedProduct.Price.HasValue)
			{
				_total = selectedProduct.Price.Value;
				if (selectedProduct.ShopShare.HasValue)
				{
					_shopShare = (decimal)(selectedProduct.Price.Value * selectedProduct.ShopShare.Value / 100);
				}
				else
				{
					_shopShare = 0;
				}
				_clientAmount = _total - _shopShare;
				UpdateInfoText();
			}
		}
	}

	private void UpdateInfoText()
	{
		if (_priceText != null)
			_priceText.Text = $"Цена: {_total:C}";
		if (_shopShareText != null)
			_shopShareText.Text = $"Доля магазина: {_shopShare:C}";
		if (_clientPayoutText != null)
			_clientPayoutText.Text = $"Выплата клиенту: {_clientAmount:C}";
	}

	private void CancelBtn_Click(object? sender, RoutedEventArgs e)
	{
		Close();
	}

	private async void ConfirmBtn_Click(object? sender, RoutedEventArgs e)
	{
		if (_productComboBox?.SelectedItem is not Product selectedProduct)
		{
			var msgBox = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Выберите товар", ButtonEnum.Ok);
			await msgBox.ShowAsync();
			return;
		}

		if (_clientComboBox?.SelectedItem is not Client selectedClient)
		{
			var msgBox = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Выберите клиента", ButtonEnum.Ok);
			await msgBox.ShowAsync();
			return;
		}

		if (_paymentMethodComboBox?.SelectedItem is not PaymentMethod selectedPaymentMethod)
		{
			var msgBox = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Выберите способ оплаты", ButtonEnum.Ok);
			await msgBox.ShowAsync();
			return;
		}

		try
		{
			using var db = new AppDbContext();

			if (_currentSale == null || _currentSale.Id == 0)
			{
				// Создание новой продажи
				var sale = new Sale
				{
					ProductId = selectedProduct.Id,
					ClientId = selectedClient.Id,
					PayMethodId = selectedPaymentMethod.Id,
					SaleDate = DateOnly.FromDateTime(DateTime.Now),
					TotalAmount = _total,
					ShopAmount = _shopShare,
					ClientAmount = _clientAmount
				};

				// Меняем статус товара на "Продано" (StatusId = 2)
				var product = await Task.Run(async () => await db.Products.FirstOrDefaultAsync(p => p.Id == selectedProduct.Id));
				if (product != null)
				{
					product.StatusId = 2;
					db.Products.Update(product);
				}

				db.Sales.Add(sale);
			}
			else
			{
				// Редактирование существующей продажи
				var dbSale = await Task.Run(async () => await db.Sales.FirstOrDefaultAsync(s => s.Id == _currentSale.Id));
				if (dbSale != null)
				{
					// Если товар изменился, восстанавливаем старый и меняем новый
					if (dbSale.ProductId != selectedProduct.Id)
					{
						var oldProduct = await Task.Run(async () => await db.Products.FirstOrDefaultAsync(p => p.Id == dbSale.ProductId));
						if (oldProduct != null)
						{
							oldProduct.StatusId = 1; // Восстанавливаем статус
							db.Products.Update(oldProduct);
						}

						var newProduct = await Task.Run(async () => await db.Products.FirstOrDefaultAsync(p => p.Id == selectedProduct.Id));
						if (newProduct != null)
						{
							newProduct.StatusId = 2; // Меняем статус на "Продано"
							db.Products.Update(newProduct);
						}
					}

					dbSale.ProductId = selectedProduct.Id;
					dbSale.ClientId = selectedClient.Id;
					dbSale.PayMethodId = selectedPaymentMethod.Id;
					dbSale.TotalAmount = _total;
					dbSale.ShopAmount = _shopShare;
					dbSale.ClientAmount = _clientAmount;
					db.Sales.Update(dbSale);
				}
			}

			await db.SaveChangesAsync();

			var successMsg = MessageBoxManager.GetMessageBoxStandard("Успех", 
				_currentSale == null || _currentSale.Id == 0 ? "Продажа успешно добавлена" : "Продажа успешно обновлена", 
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

