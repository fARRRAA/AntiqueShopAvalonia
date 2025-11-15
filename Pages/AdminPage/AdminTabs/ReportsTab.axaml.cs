using AntiqueShopAvalonia.Model;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AppDbContext = AntiqueShopAvalonia.Model.AppDbContext;

namespace AntiqueShopAvalonia.Pages.AdminPage.AdminTabs;

public partial class ReportsTab : UserControl
{
	private DataGrid? _salesReportGrid;
	private DataGrid? _returnsReportGrid;
	private DataGrid? _inventoryReportGrid;
	private DatePicker? _salesStartDate;
	private DatePicker? _salesEndDate;
	private DatePicker? _returnsStartDate;
	private DatePicker? _returnsEndDate;
	private DataGrid? _shareReportGrid;
	private DatePicker? _shareStartDate;
	private DatePicker? _shareEndDate;
	private DatePicker? _paymentsStartDate;
	private DatePicker? _paymentsEndDate;
	private ComboBox?   _paymentsPeriodType;
	private DataGrid?   _paymentsReportGrid;
    public ReportsTab()
	{
		InitializeComponent();
		InitializeDates();
	}

	private void InitializeComponent()
	{
		AvaloniaXamlLoader.Load(this);
		_salesReportGrid = this.FindControl<DataGrid>("SalesReportGrid");
		_returnsReportGrid = this.FindControl<DataGrid>("ReturnsReportGrid");
		_inventoryReportGrid = this.FindControl<DataGrid>("InventoryReportGrid");
		_shareReportGrid = this.FindControl<DataGrid>("ShareReportGrid");
        _salesStartDate = this.FindControl<DatePicker>("SalesStartDate");
		_salesEndDate = this.FindControl<DatePicker>("SalesEndDate");
		_returnsStartDate = this.FindControl<DatePicker>("ReturnsStartDate");
		_returnsEndDate = this.FindControl<DatePicker>("ReturnsEndDate");
        _shareStartDate = this.FindControl<DatePicker>("ShareStartDate");
        _shareEndDate = this.FindControl<DatePicker>("ShareEndDate");
        _paymentsStartDate= this.FindControl<DatePicker>("PaymentsStartDate");
        _paymentsEndDate = this.FindControl<DatePicker>("PaymentsEndDate");
        _paymentsReportGrid = this.FindControl<DataGrid>("PaymentsReportGrid");
		_paymentsPeriodType = this.FindControl<ComboBox>("PaymentsPeriodType");
    }

	private void InitializeDates()
	{
		var today = DateTime.Today;
		var monthAgo = today.AddMonths(-1);

		if (_salesStartDate != null) _salesStartDate.SelectedDate = monthAgo;
		if (_salesEndDate != null) _salesEndDate.SelectedDate = today;
		if (_returnsStartDate != null) _returnsStartDate.SelectedDate = monthAgo;
		if (_returnsEndDate != null) _returnsEndDate.SelectedDate = today;
	}

	private async void UpdateSalesReportBtn_Click(object? sender, RoutedEventArgs e)
	{
		if (_salesStartDate?.SelectedDate == null || _salesEndDate?.SelectedDate == null)
			return;

		try
		{
			using var db = new AppDbContext();
			var startDate = _salesStartDate.SelectedDate.Value;
			var endDate = _salesEndDate.SelectedDate.Value;

			var sales = await Task.Run(() =>
				db.Sales
					.Where(s => s.SaleDate.HasValue && 
						s.SaleDate.Value >= new DateOnly(startDate.Year, startDate.Month, startDate.Day) &&
						s.SaleDate.Value <= new DateOnly(endDate.Year, endDate.Month, endDate.Day))
					.ToList());

			var report = sales
				.GroupBy(s => s.SaleDate?.ToString("MM.yyyy") ?? "Без даты")
				.Select(g => new
				{
					Period = g.Key,
					TotalSales = g.Count(),
					ShopAmount = g.Sum(s => s.ShopAmount ?? 0),
					ClientAmount = g.Sum(s => s.ClientAmount ?? 0)
				})
				.ToList();

			if (_salesReportGrid != null)
			{
				_salesReportGrid.ItemsSource = report;
			}
		}
		catch (Exception ex)
		{
			System.Diagnostics.Debug.WriteLine($"Error loading sales report: {ex.Message}");
		}
	}

	private async void UpdateReturnsReportBtn_Click(object? sender, RoutedEventArgs e)
	{
		if (_returnsStartDate?.SelectedDate == null || _returnsEndDate?.SelectedDate == null)
			return;

		try
		{
			using var db = new AppDbContext();
			var startDate = _returnsStartDate.SelectedDate.Value;
			var endDate = _returnsEndDate.SelectedDate.Value;

			var returns = await Task.Run(() =>
				db.Returns
					.Include(r => r.Product)
					.Where(r => r.ReturnDate.HasValue &&
						r.ReturnDate.Value >= new DateOnly(startDate.Year, startDate.Month, startDate.Day) &&
						r.ReturnDate.Value <= new DateOnly(endDate.Year, endDate.Month, endDate.Day))
					.ToList());

			var report = returns
				.GroupBy(r => r.ReturnDate?.ToString("MM.yyyy") ?? "Без даты")
				.Select(g => new
				{
					Period = g.Key,
					Count = g.Count(),
					Amount = g.Sum(r => r.Product?.Price ?? 0)
				})
				.ToList();

			if (_returnsReportGrid != null)
			{
				_returnsReportGrid.ItemsSource = report;
			}
		}
		catch (Exception ex)
		{
			System.Diagnostics.Debug.WriteLine($"Error loading returns report: {ex.Message}");
		}
	}

	private async void UpdateInventoryReportBtn_Click(object? sender, RoutedEventArgs e)
	{
		try
		{
			using var db = new AppDbContext();
			var inventory = await Task.Run(() =>
				db.Products
					.Include(p => p.Category)
					.Where(p => p.StatusId == 1) // Только товары в наличии
					.GroupBy(p => p.Category != null ? p.Category.Name : "Без категории")
					.Select(g => new
					{
						Category = g.Key,
						Count = g.Count()
					})
					.ToList());

			if (_inventoryReportGrid != null)
			{
				_inventoryReportGrid.ItemsSource = inventory;
			}
		}
		catch (Exception ex)
		{
			System.Diagnostics.Debug.WriteLine($"Error loading inventory report: {ex.Message}");
		}
	}
    private async void UpdateShareChart_Click(object? sender, RoutedEventArgs e)
    {
        if (_shareStartDate?.SelectedDate == null || _shareEndDate?.SelectedDate == null)
            return;

        try
        {
            using var db = new AppDbContext();

            var start = _shareStartDate.SelectedDate.Value;
            var end = _shareEndDate.SelectedDate.Value;

            var sales = await Task.Run(() =>
                db.Sales
                    .Where(s => s.SaleDate.HasValue &&
                        s.SaleDate.Value >= new DateOnly(start.Year, start.Month, start.Day) &&
                        s.SaleDate.Value <= new DateOnly(end.Year, end.Month, end.Day))
                    .ToList());

            var report = sales
                .Where(s => s.SaleDate.HasValue)
                .GroupBy(s => s.SaleDate!.Value)
                .Select(g => new
                {
                    Period = g.Key.ToString("dd.MM.yyyy"),
                    ClientAmount = g.Sum(s => s.ClientAmount ?? 0),
                    ShopAmount = g.Sum(s => s.ShopAmount ?? 0),
                    TotalSales = g.Count()
                })
                .OrderBy(x => DateTime.Parse(x.Period))
                .ToList();

            if (_shareReportGrid != null)
                _shareReportGrid.ItemsSource = report;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Ошибка загрузки отчета выплат: {ex.Message}");
        }
    }
    private async void UpdatePaymentsChart_Click(object? sender, RoutedEventArgs e)
    {
        try
        {
            if (!_paymentsStartDate.SelectedDate.HasValue || !_paymentsEndDate.SelectedDate.HasValue)
            {
                return;
            }

            var startDate = _paymentsStartDate.SelectedDate.Value;
            var endDate = _paymentsEndDate.SelectedDate.Value;
            var byMonth = _paymentsPeriodType.SelectedIndex == 1;

            using var db = new AppDbContext();

            var sales = await Task.Run(() =>
                db.Sales
                    .Where(s => s.SaleDate.HasValue &&
                        s.SaleDate.Value >= new DateOnly(startDate.Year, startDate.Month, startDate.Day) &&
                        s.SaleDate.Value <= new DateOnly(endDate.Year, endDate.Month, endDate.Day))
                    .ToList());

            var groupedSales = byMonth
                ? sales
                    .Where(s => s.SaleDate.HasValue)
                    .GroupBy(s => new { s.SaleDate.Value.Year, s.SaleDate.Value.Month })
                    .Select(g => new
                    {
                        Period = new DateTime(g.Key.Year, g.Key.Month, 1).ToString("MM.yyyy"),
                        ClientAmount = g.Sum(s => s.ClientAmount ?? 0),
                        ShopAmount = g.Sum(s => s.ShopAmount ?? 0),
                        TotalSales = g.Count()
                    })
                    .OrderBy(x => DateTime.ParseExact(x.Period, "MM.yyyy", null))
                : sales
                    .Where(s => s.SaleDate.HasValue)
                    .GroupBy(s => s.SaleDate!.Value)
                    .Select(g => new
                    {
                        Period = g.Key.ToString("dd.MM.yyyy"),
                        ClientAmount = g.Sum(s => s.ClientAmount ?? 0),
                        ShopAmount = g.Sum(s => s.ShopAmount ?? 0),
                        TotalSales = g.Count()
                    })
                    .OrderBy(x => DateTime.ParseExact(x.Period, "dd.MM.yyyy", null));

            var report = groupedSales.ToList();

            if (_paymentsReportGrid != null)
                _paymentsReportGrid.ItemsSource = report;
        }
        catch (Exception ex)
        {
        }
    }
}

