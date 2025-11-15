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
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using AppDbContext = AntiqueShopAvalonia.Model.AppDbContext;
using AntiqueShopAvalonia.Fonts;
using PdfSharp.Fonts;

namespace AntiqueShopAvalonia.Pages.AdminPage.AdminTabs;

public partial class SalesTab : UserControl
{
	private DataGrid? _salesList;
	private TextBox? _salesSearchText;
	private TextBlock? _salesCountText;
	private System.Collections.Generic.List<Sale>? _allSales;

	public SalesTab()
	{
		InitializeComponent();
		_ = LoadDataAsync();
	}

	private void InitializeComponent()
	{
		AvaloniaXamlLoader.Load(this);
		_salesList = this.FindControl<DataGrid>("SalesList");
		_salesSearchText = this.FindControl<TextBox>("SalesSearchText");
		_salesCountText = this.FindControl<TextBlock>("SalesCountText");
	}

	private async Task LoadDataAsync()
	{
		try
		{
			using var db = new AppDbContext();
			var sales = await Task.Run(() =>
				db.Sales
					.Include(s => s.Product)
					.Include(s => s.Client)
					.Include(s => s.PayMethod)
					.OrderByDescending(s => s.SaleDate)
					.ToList());

			_allSales = sales;
			
			if (_salesList != null)
			{
				_salesList.ItemsSource = null;
				_salesList.ItemsSource = sales;
			}
			
			if (_salesCountText != null)
			{
				_salesCountText.Text = $"Всего продаж: {sales.Count}";
			}
		}
		catch (Exception ex)
		{
			System.Diagnostics.Debug.WriteLine($"Error loading sales: {ex.Message}");
		}
	}

	private void SalesSearchText_TextChanged(object? sender, TextChangedEventArgs e)
	{
		if (_allSales == null || _salesList == null) return;

		var searchText = _salesSearchText?.Text?.ToLower() ?? string.Empty;
		if (string.IsNullOrWhiteSpace(searchText))
		{
			_salesList.ItemsSource = _allSales;
			return;
		}

		var filtered = _allSales.Where(s =>
			(s.Product?.Name?.ToLower().Contains(searchText) ?? false) ||
			(s.PayMethod?.Name?.ToLower().Contains(searchText) ?? false) ||
			(s.SaleDate?.ToString().Contains(searchText) ?? false)
		).ToList();

		_salesList.ItemsSource = filtered;
	}

	private async void DeleteSaleBtn_Click(object? sender, RoutedEventArgs e)
	{
		if (_salesList?.SelectedItem is not Sale selectedSale)
		{
			var msgBox = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Выберите продажу!", ButtonEnum.Ok);
			await msgBox.ShowAsync();
			return;
		}

		var confirm = MessageBoxManager.GetMessageBoxStandard("Подтверждение", "Удалить данную продажу?", ButtonEnum.YesNo);
		if (await confirm.ShowAsync() == ButtonResult.Yes)
		{
			using var db = new AppDbContext();
			// Восстанавливаем статус товара
			if (selectedSale.ProductId.HasValue)
			{
				var product = await Task.Run(async () => await db.Products.FirstOrDefaultAsync(p => p.Id == selectedSale.ProductId.Value));
				if (product != null)
				{
					// Предполагаем, что статус 1 - "В наличии"
					product.StatusId = 1;
					db.Products.Update(product);
				}
			}
			db.Sales.Remove(selectedSale);
			await db.SaveChangesAsync();
			await LoadDataAsync();
		}
	}

	private async void EditSaleBtn_Click(object? sender, RoutedEventArgs e)
	{
		if (_salesList?.SelectedItem is not Sale selectedSale)
		{
			var msgBox = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Выберите продажу!", ButtonEnum.Ok);
			await msgBox.ShowAsync();
			return;
		}

		var editWindow = new AddOrEditSale(selectedSale);
		var parentWindow = (Window)TopLevel.GetTopLevel(this)!;
		await editWindow.ShowDialog(parentWindow);
		await LoadDataAsync();
	}

	private async void AddSaleBtn_Click(object? sender, RoutedEventArgs e)
	{
		var addWindow = new AddOrEditSale();
		var parentWindow = (Window)TopLevel.GetTopLevel(this)!;
		await addWindow.ShowDialog(parentWindow);
		await LoadDataAsync();
	}

	private async void EditSale_Click(object? sender, RoutedEventArgs e)
	{
		if (sender is Button button && button.Tag is Sale sale)
		{
			var editWindow = new AddOrEditSale(sale);
			var parentWindow = (Window)TopLevel.GetTopLevel(this)!;
			await editWindow.ShowDialog(parentWindow);
			await LoadDataAsync();
		}
	}

	private async void DeleteSale_Click(object? sender, RoutedEventArgs e)
	{
		if (sender is Button button && button.Tag is Sale sale)
		{
			var confirm = MessageBoxManager.GetMessageBoxStandard("Подтверждение", "Удалить данную продажу?", ButtonEnum.YesNo);
			if (await confirm.ShowAsync() == ButtonResult.Yes)
			{
				using var db = new AppDbContext();
				// Восстанавливаем статус товара
				if (sale.ProductId.HasValue)
				{
					var product = await Task.Run(async () => await db.Products.FirstOrDefaultAsync(p => p.Id == sale.ProductId.Value));
					if (product != null)
					{
						product.StatusId = 1;
						db.Products.Update(product);
					}
				}
				db.Sales.Remove(sale);
				await db.SaveChangesAsync();
				await LoadDataAsync();
			}
		}
	}

	private async void PrintReceiptBtn_Click(object? sender, RoutedEventArgs e)
	{
		if (_salesList?.SelectedItem is not Sale selectedSale)
		{
			var msgBox = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Выберите продажу для печати чека!", ButtonEnum.Ok);
			await msgBox.ShowAsync();
			return;
		}

		try
		{
			var saveFileDialog = new SaveFileDialog
			{
				Title = "Сохранить чек",
				DefaultExtension = "pdf",
				InitialFileName = $"Чек_{selectedSale.Id}.pdf",
				Filters = new System.Collections.Generic.List<FileDialogFilter>
				{
					new FileDialogFilter { Name = "PDF файлы", Extensions = new System.Collections.Generic.List<string> { "pdf" } }
				}
			};

			var parentWindow = (Window)TopLevel.GetTopLevel(this)!;
			var filePath = await saveFileDialog.ShowAsync(parentWindow);
			if (string.IsNullOrEmpty(filePath)) return;
            GlobalFontSettings.FontResolver = CustomFontResolver.Instance;

            using var document = new PdfDocument();
			var page = document.AddPage();
			var gfx = XGraphics.FromPdfPage(page);
			var font = new XFont("Arial", 12);
			var titleFont = new XFont("Arial", 20);
			var headerFont = new XFont("Arial", 14);

			int y = 50;
			int margin = 50;

			// Заголовок
			var titleText = "ТОВАРНЫЙ ЧЕК";
			var titleWidth = gfx.MeasureString(titleText, titleFont).Width;
			gfx.DrawString(titleText, titleFont, XBrushes.Black, (page.Width.Point - titleWidth) / 2, y);
			y += 40;

			// Основная информация
			gfx.DrawString($"Дата продажи: {selectedSale.SaleDate:dd.MM.yyyy}", font, XBrushes.Black, margin, y);
			y += 20;
			gfx.DrawString($"Номер чека: {selectedSale.Id}", font, XBrushes.Black, margin, y);
			y += 30;

			// Информация о товаре
			gfx.DrawString("ИНФОРМАЦИЯ О ТОВАРЕ", headerFont, XBrushes.Black, margin, y);
			y += 25;
			if (selectedSale.Product != null)
			{
				gfx.DrawString($"Наименование: {selectedSale.Product.Name}", font, XBrushes.Black, margin, y);
				y += 20;
				if (selectedSale.Product.Price.HasValue)
				{
					gfx.DrawString($"Цена: {selectedSale.Product.Price.Value:C}", font, XBrushes.Black, margin, y);
					y += 20;
				}
			}
			if (selectedSale.PayMethod != null)
			{
				gfx.DrawString($"Способ оплаты: {selectedSale.PayMethod.Name}", font, XBrushes.Black, margin, y);
				y += 20;
			}
			if (selectedSale.Client != null)
			{
				gfx.DrawString($"Клиент: {selectedSale.Client.Fname} {selectedSale.Client.Lname}", font, XBrushes.Black, margin, y);
				y += 20;
			}
			y += 10;

			// Итоговая сумма
			if (selectedSale.Product?.Price.HasValue == true)
			{
				gfx.DrawString($"Итого к оплате: {selectedSale.Product.Price.Value:C}", headerFont, XBrushes.Black, margin, y);
				y += 30;
			}

			// Подписи
			gfx.DrawString("Подпись продавца: ________________", font, XBrushes.Black, margin, y);
			y += 20;
			gfx.DrawString("Подпись покупателя: ________________", font, XBrushes.Black, margin, y);
			y += 30;
			gfx.DrawString("Спасибо за покупку!", new XFont("Arial", 12), XBrushes.Black, margin, y);

			document.Save(filePath);

			var successMsg = MessageBoxManager.GetMessageBoxStandard("Успех", "Чек успешно сохранен", ButtonEnum.Ok);
			await successMsg.ShowAsync();
		}
		catch (Exception ex)
		{
			var msgBox = MessageBoxManager.GetMessageBoxStandard("Ошибка", $"Ошибка при печати чека:\n{ex.Message}", ButtonEnum.Ok);
			await msgBox.ShowAsync();
		}
	}
}

