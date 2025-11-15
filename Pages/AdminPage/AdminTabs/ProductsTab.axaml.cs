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
using QRCoder;
using System.Drawing;
using System.Drawing.Imaging;
using AppDbContext = AntiqueShopAvalonia.Model.AppDbContext;
using AntiqueShopAvalonia.Fonts;
using PdfSharp.Fonts;

namespace AntiqueShopAvalonia.Pages.AdminPage.AdminTabs;

public partial class ProductsTab : UserControl
{
	private DataGrid? _productsList;
	private TextBox? _productsSearchText;
	private TextBlock? _productsCountText;
	private System.Collections.Generic.List<Product>? _allProducts;

	public ProductsTab()
	{
		InitializeComponent();
		_ = LoadDataAsync();
	}

	private void InitializeComponent()
	{
		AvaloniaXamlLoader.Load(this);
		_productsList = this.FindControl<DataGrid>("ProductsList");
		_productsSearchText = this.FindControl<TextBox>("ProductsSearchText");
		_productsCountText = this.FindControl<TextBlock>("ProductsCountText");
	}

	private async Task LoadDataAsync()
	{
		try
		{
			using var db = new AppDbContext();
			var products = await Task.Run(() =>
				db.Products
					.Include(p => p.Category)
					.Include(p => p.Status)
					.OrderBy(p => p.Id)
					.ToList());

			_allProducts = products;
			
			if (_productsList != null)
			{
				_productsList.ItemsSource = null;
				_productsList.ItemsSource = products;
			}
			
			if (_productsCountText != null)
			{
				_productsCountText.Text = $"Всего товаров: {products.Count}";
			}
		}
		catch (Exception ex)
		{
			System.Diagnostics.Debug.WriteLine($"Error loading products: {ex.Message}");
		}
	}

	private void ProductsSearchText_TextChanged(object? sender, TextChangedEventArgs e)
	{
		if (_allProducts == null || _productsList == null) return;

		var searchText = _productsSearchText?.Text?.ToLower() ?? string.Empty;
		if (string.IsNullOrWhiteSpace(searchText))
		{
			_productsList.ItemsSource = _allProducts;
			return;
		}

		var filtered = _allProducts.Where(p =>
			(p.Name?.ToLower().Contains(searchText) ?? false) ||
			(p.Description?.ToLower().Contains(searchText) ?? false) ||
			(p.Category?.Name?.ToLower().Contains(searchText) ?? false) ||
			(p.Status?.Name?.ToLower().Contains(searchText) ?? false)
		).ToList();

		_productsList.ItemsSource = filtered;
	}

	private async void DeleteProductBtn_Click(object? sender, RoutedEventArgs e)
	{
		if (_productsList?.SelectedItem is not Product selectedProduct)
		{
			var msgBox = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Выберите товар!", ButtonEnum.Ok);
			await msgBox.ShowAsync();
			return;
		}

		var confirm = MessageBoxManager.GetMessageBoxStandard("Подтверждение", "Удалить данный товар?", ButtonEnum.YesNo);
		if (await confirm.ShowAsync() == ButtonResult.Yes)
		{
			using var db = new AppDbContext();
			db.Products.Remove(selectedProduct);
			await db.SaveChangesAsync();
			await LoadDataAsync();
		}
	}

	private async void EditProductBtn_Click(object? sender, RoutedEventArgs e)
	{
		if (_productsList?.SelectedItem is not Product selectedProduct)
		{
			var msgBox = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Выберите товар!", ButtonEnum.Ok);
			await msgBox.ShowAsync();
			return;
		}

		var editWindow = new AddOrEditProduct(selectedProduct);
		var parentWindow = (Window)TopLevel.GetTopLevel(this)!;
		await editWindow.ShowDialog(parentWindow);
		await LoadDataAsync();
	}

	private async void AddProductBtn_Click(object? sender, RoutedEventArgs e)
	{
		var addWindow = new AddOrEditProduct();
		var parentWindow = (Window)TopLevel.GetTopLevel(this)!;
		await addWindow.ShowDialog(parentWindow);
		await LoadDataAsync();
	}

	private async void EditProduct_Click(object? sender, RoutedEventArgs e)
	{
		if (sender is Button button && button.Tag is Product product)
		{
			var editWindow = new AddOrEditProduct(product);
			var parentWindow = (Window)TopLevel.GetTopLevel(this)!;
			await editWindow.ShowDialog(parentWindow);
			await LoadDataAsync();
		}
	}

	private async void DeleteProduct_Click(object? sender, RoutedEventArgs e)
	{
		if (sender is Button button && button.Tag is Product product)
		{
			var confirm = MessageBoxManager.GetMessageBoxStandard("Подтверждение", "Удалить данный товар?", ButtonEnum.YesNo);
			if (await confirm.ShowAsync() == ButtonResult.Yes)
			{
				using var db = new AppDbContext();
				db.Products.Remove(product);
				await db.SaveChangesAsync();
				await LoadDataAsync();
			}
		}
	}

	private async void PrintSinglePriceTagBtn_Click(object? sender, RoutedEventArgs e)
	{
		if (_productsList?.SelectedItem is not Product selectedProduct)
		{
			var msgBox = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Выберите товар для печати ценника!", ButtonEnum.Ok);
			await msgBox.ShowAsync();
			return;
		}

		try
		{
			var saveFileDialog = new SaveFileDialog
			{
				Title = "Сохранить ценник",
				DefaultExtension = "pdf",
				InitialFileName = $"Ценник_{selectedProduct.Name}.pdf",
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
			var titleFont = new XFont("Arial", 14);

			var qrGenerator = new QRCodeGenerator();
			var qrData = $"ID: {selectedProduct.Id}\nНазвание: {selectedProduct.Name}";
			var qrCodeData = qrGenerator.CreateQrCode(qrData, QRCodeGenerator.ECCLevel.Q);
			var qrCode = new QRCode(qrCodeData);
			var qrImage = qrCode.GetGraphic(10);

			var tempFile = Path.GetTempFileName() + ".png";
			qrImage.Save(tempFile, ImageFormat.Png);

			int qrSize = 200;
			int startX = (int)((page.Width.Point - qrSize) / 2);
			int startY = 50;

			var titleText = "Ценник";
			var titleWidth = gfx.MeasureString(titleText, titleFont).Width;
			gfx.DrawString(titleText, titleFont, XBrushes.Black, startX + (qrSize - titleWidth) / 2, startY);

			using (var image = XImage.FromFile(tempFile))
			{
				gfx.DrawImage(image, startX, startY + 30, qrSize, qrSize);
			}

			startY += qrSize + 50;
			var textX = startX;

			gfx.DrawString($"ID: {selectedProduct.Id}", font, XBrushes.Black, textX, startY);
			gfx.DrawString($"Название: {selectedProduct.Name}", font, XBrushes.Black, textX, startY + 20);
			if (selectedProduct.Price.HasValue)
				gfx.DrawString($"Цена: {selectedProduct.Price.Value:C}", titleFont, XBrushes.Black, textX, startY + 40);
			if (selectedProduct.Category != null)
				gfx.DrawString($"Категория: {selectedProduct.Category.Name}", font, XBrushes.Black, textX, startY + 60);
			if (selectedProduct.ReceiptDate.HasValue)
				gfx.DrawString($"Дата приема: {selectedProduct.ReceiptDate.Value:dd.MM.yyyy}", font, XBrushes.Black, textX, startY + 80);

			File.Delete(tempFile);
			document.Save(filePath);

			var successMsg = MessageBoxManager.GetMessageBoxStandard("Успех", "Ценник успешно сохранен", ButtonEnum.Ok);
			await successMsg.ShowAsync();
		}
		catch (Exception ex)
		{
			var msgBox = MessageBoxManager.GetMessageBoxStandard("Ошибка", $"Ошибка при создании ценника:\n{ex.Message}", ButtonEnum.Ok);
			await msgBox.ShowAsync();
		}
	}

	private async void PrintAllPriceTagsBtn_Click(object? sender, RoutedEventArgs e)
	{
		try
		{
			using var db = new AppDbContext();
			var products = await Task.Run(() =>
				db.Products
					.Where(p => p.StatusId == 1) // Только товары в наличии
					.ToList());

			if (!products.Any())
			{
				var msgBox = MessageBoxManager.GetMessageBoxStandard("Предупреждение", "Нет товаров для печати ценников", ButtonEnum.Ok);
				await msgBox.ShowAsync();
				return;
			}

			var saveFileDialog = new SaveFileDialog
			{
				Title = "Сохранить ценники",
				DefaultExtension = "pdf",
				InitialFileName = "Ценники.pdf",
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

			int y = 50;
			int qrSize = 100;
			int itemsPerPage = 6;
			int currentItem = 0;

			foreach (var product in products)
			{
				if (currentItem >= itemsPerPage)
				{
					page = document.AddPage();
					gfx = XGraphics.FromPdfPage(page);
					y = 50;
					currentItem = 0;
				}

				var qrGenerator = new QRCodeGenerator();
				var qrData = $"ID: {product.Id}\nНазвание: {product.Name}";
				var qrCodeData = qrGenerator.CreateQrCode(qrData, QRCodeGenerator.ECCLevel.Q);
				var qrCode = new QRCode(qrCodeData);
				var qrImage = qrCode.GetGraphic(5);

				var tempFile = Path.GetTempFileName() + ".png";
				qrImage.Save(tempFile, ImageFormat.Png);

				using (var image = XImage.FromFile(tempFile))
				{
					gfx.DrawImage(image, 50, y, qrSize, qrSize);
				}

				gfx.DrawString($"ID: {product.Id}", font, XBrushes.Black, 160, y + 20);
				gfx.DrawString($"Название: {product.Name}", font, XBrushes.Black, 160, y + 40);
				if (product.Price.HasValue)
					gfx.DrawString($"Цена: {product.Price.Value:C}", font, XBrushes.Black, 160, y + 60);

				File.Delete(tempFile);
				y += qrSize + 50;
				currentItem++;
			}

			document.Save(filePath);

			var successMsg = MessageBoxManager.GetMessageBoxStandard("Успех", "Ценники успешно сохранены", ButtonEnum.Ok);
			await successMsg.ShowAsync();
		}
		catch (Exception ex)
		{
			var msgBox = MessageBoxManager.GetMessageBoxStandard("Ошибка", $"Ошибка при создании ценников:\n{ex.Message}", ButtonEnum.Ok);
			await msgBox.ShowAsync();
		}
	}
}

