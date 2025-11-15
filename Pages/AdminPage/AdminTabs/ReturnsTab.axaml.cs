using AntiqueShopAvalonia.Model;
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

public partial class ReturnsTab : UserControl
{
	private DataGrid? _returnsList;
	private TextBox? _returnsSearchText;
	private TextBlock? _returnsCountText;
	private System.Collections.Generic.List<Return>? _allReturns;

	public ReturnsTab()
	{
		InitializeComponent();
		_ = LoadDataAsync();
	}

	private void InitializeComponent()
	{
		AvaloniaXamlLoader.Load(this);
		_returnsList = this.FindControl<DataGrid>("ReturnsList");
		_returnsSearchText = this.FindControl<TextBox>("ReturnsSearchText");
		_returnsCountText = this.FindControl<TextBlock>("ReturnsCountText");
	}

	public async Task LoadDataAsync()
	{
		try
		{
			using var db = new AppDbContext();
			var returns = await Task.Run(() =>
				db.Returns
					.Include(r => r.Product)
					.Include(r => r.ReturnType)
					.OrderByDescending(r => r.ReturnDate)
					.ToList());

			_allReturns = returns;
			
			if (_returnsList != null)
			{
				_returnsList.ItemsSource = null;
				_returnsList.ItemsSource = returns;
			}
			
			if (_returnsCountText != null)
			{
				_returnsCountText.Text = $"Всего возвратов: {returns.Count}";
			}
		}
		catch (Exception ex)
		{
			System.Diagnostics.Debug.WriteLine($"Error loading returns: {ex.Message}");
		}
	}

	private void ReturnsSearchText_TextChanged(object? sender, TextChangedEventArgs e)
	{
		if (_allReturns == null || _returnsList == null) return;

		var searchText = _returnsSearchText?.Text?.ToLower() ?? string.Empty;
		if (string.IsNullOrWhiteSpace(searchText))
		{
			_returnsList.ItemsSource = _allReturns;
			return;
		}

		var filtered = _allReturns.Where(r =>
			(r.Product?.Name?.ToLower().Contains(searchText) ?? false) ||
			(r.Reason?.ToLower().Contains(searchText) ?? false) ||
			(r.ReturnType?.Name?.ToLower().Contains(searchText) ?? false)
		).ToList();

		_returnsList.ItemsSource = filtered;
	}

	private async void CheckProductsBtn_Click(object? sender, RoutedEventArgs e)
	{
		var checkProductsWindow = new Windows.CheckProductsWindow(this);
		var parentWindow = (Window)TopLevel.GetTopLevel(this)!;
		await checkProductsWindow.ShowDialog(parentWindow);
	}

	private async void PrintReturnActBtn_Click(object? sender, RoutedEventArgs e)
	{
		if (_returnsList?.SelectedItem is not Return selectedReturn)
		{
			var msgBox = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Выберите возврат для печати акта!", ButtonEnum.Ok);
			await msgBox.ShowAsync();
			return;
		}

		try
		{
			var saveFileDialog = new SaveFileDialog
			{
				Title = "Сохранить акт возврата",
				DefaultExtension = "pdf",
				InitialFileName = $"Акт_возврата_{selectedReturn.Id}.pdf",
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
			var font = new XFont("Arial", emSize: 12);
			var titleFont = new XFont("Arial", 20);
			var headerFont = new XFont("Arial", 14);

			int y = 50;
			int margin = 50;

			// Заголовок
			var titleText = "АКТ ВОЗВРАТА ТОВАРА";
			var titleWidth = gfx.MeasureString(titleText, titleFont).Width;
			gfx.DrawString(titleText, titleFont, XBrushes.Black, (page.Width.Point - titleWidth) / 2, y);
			y += 40;

			// Основная информация
			gfx.DrawString($"Дата возврата: {selectedReturn.ReturnDate:dd.MM.yyyy}", font, XBrushes.Black, margin, y);
			y += 20;
			gfx.DrawString($"Номер акта: {selectedReturn.Id}", font, XBrushes.Black, margin, y);
			y += 30;

			// Информация о товаре
			gfx.DrawString("ИНФОРМАЦИЯ О ТОВАРЕ", headerFont, XBrushes.Black, margin, y);
			y += 25;
			if (selectedReturn.Product != null)
			{
				gfx.DrawString($"Наименование: {selectedReturn.Product.Name}", font, XBrushes.Black, margin, y);
				y += 20;
				if (selectedReturn.Product.Price.HasValue)
				{
					gfx.DrawString($"Цена: {selectedReturn.Product.Price.Value:C}", font, XBrushes.Black, margin, y);
					y += 20;
				}
			}
			if (selectedReturn.ReturnType != null)
			{
				gfx.DrawString($"Тип возврата: {selectedReturn.ReturnType.Name}", font, XBrushes.Black, margin, y);
				y += 30;
			}

			// Причина возврата
			gfx.DrawString("ПРИЧИНА ВОЗВРАТА", headerFont, XBrushes.Black, margin, y);
			y += 25;
			if (!string.IsNullOrEmpty(selectedReturn.Reason))
			{
				var reasonLines = selectedReturn.Reason.Split('\n');
				foreach (var line in reasonLines)
				{
					gfx.DrawString(line, font, XBrushes.Black, margin, y);
					y += 20;
				}
			}
			y += 10;

			// Подписи
			gfx.DrawString("Подпись сотрудника: ________________", font, XBrushes.Black, margin, y);
			y += 20;
			if (selectedReturn.ReturnType?.Name == "Возврат собственнику")
			{
				gfx.DrawString("Подпись собственника: ________________", font, XBrushes.Black, margin, y);
			}

			document.Save(filePath);

			var successMsg = MessageBoxManager.GetMessageBoxStandard("Успех", "Акт возврата успешно сохранен", ButtonEnum.Ok);
			await successMsg.ShowAsync();
		}
		catch (Exception ex)
		{
			var msgBox = MessageBoxManager.GetMessageBoxStandard("Ошибка", $"Ошибка при создании акта возврата:\n{ex.Message}", ButtonEnum.Ok);
			await msgBox.ShowAsync();
		}
	}
}

