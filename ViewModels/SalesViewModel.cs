using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using BhyatPos.Models;
using BhyatPos.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BhyatPos.ViewModels;

public partial class SalesViewModel : ViewModelBase
{
    public string? CurrentUser { get; set; }

    public ObservableCollection<SaleItem> SaleItems { get; }

    private readonly ProductService _productService;
    private readonly List<SaleItem> _saleItemBuffer;
    private Sale _currentSale;

    [ObservableProperty]
    private decimal? eftTotal;

    [ObservableProperty]
    private decimal? cashTotal;

    [ObservableProperty]
    private decimal? cardTotal;
    
    [ObservableProperty]
    private decimal totalPrice = 0;

    [ObservableProperty]
    private string? barcode;

    [ObservableProperty]
    private decimal? totalDue;

    [ObservableProperty]
    private decimal? totalChange = 0;

    public SalesViewModel()
    {

        _saleItemBuffer = new List<SaleItem>();
        SaleItems = new ObservableCollection<SaleItem>();
        _productService = new ProductService();

        _currentSale = new Sale
        {
            InvoiceNumber = "INV001",
            UserName = "admin",
            UserID = 1,
            CustomerID = 1,
            TotalPrice = 0
        };

        PropertyChanged += OnPropertyChanged;
        UpdateTotalDue();
    }
    private void OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        // Recalculate TotalDue when any dependent property changes
        if (e.PropertyName is nameof(TotalPrice) or nameof(CashTotal) or nameof(CardTotal) or nameof(EftTotal))
        {
            UpdateTotalDue();
        }
    }

    public void RecalculateTotal()
    {
        TotalPrice = 0;

        foreach (SaleItem item in SaleItems)
        {
            decimal price = item.PriceAtSale;
            int quantity = item.QuantitySold;
            TotalPrice += price*quantity;
        }

        _currentSale.TotalPrice = TotalPrice;
    }

    private void UpdateTotalDue()
    {
        // Handle null values by treating them as 0
        // FIX MY SHIT LIKE TOTAL CALC
        decimal price = TotalPrice;
        decimal cash = CashTotal ?? 0m;
        decimal card = CardTotal ?? 0m;
        decimal eft = EftTotal ?? 0m;

        decimal totalRecived = cash + card + eft;

        TotalChange = (totalRecived > TotalPrice) ? -(price - (cash + card + eft)) : 0;

        TotalDue = (TotalChange > 0) ? 0 : (price - (cash + card + eft));
    }

    [RelayCommand]
    public void AddItemByBarcode()
    {
        if (string.IsNullOrWhiteSpace(Barcode))
            return;

        Product? product = _productService.ReadProductFromBarcode(Barcode);

        if (product is null)
        {
            Console.WriteLine("Product not found.");
            return;
        }

        var newItem = new SaleItem
        {
            ProductID = product.ProductID,
            QuantitySold = 1,
            PriceAtSale = product.Price,
            Subtotal = product.Price,

            ProductName = product.ProductName,
            Description = product.Description,
            Category = product.Category,
            Barcode = product.Barcode

        };


        _saleItemBuffer.Add(newItem);
        SaleItems.Add(newItem);

        RecalculateTotal();
        Barcode = "";
    }


    [RelayCommand]
    public void CompleteSale()
    {
        if (TotalDue == 0)
        {
            var salesService = new SalesService();
            salesService.ProcessSale(_currentSale, _saleItemBuffer);

            _saleItemBuffer.Clear();
            SaleItems.Clear();
        }

    }

}