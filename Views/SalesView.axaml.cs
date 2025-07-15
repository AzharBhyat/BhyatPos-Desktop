using Avalonia.Controls;
using BhyatPos.ViewModels;

namespace BhyatPos.Views;

public partial class SalesView : UserControl
{
    private SalesViewModel viewModel;
    public SalesView()
    {
        InitializeComponent();
        viewModel = new SalesViewModel();
        DataContext = viewModel;

    }

    private void TextBox_KeyDown(object? sender, Avalonia.Input.KeyEventArgs e)
    {
        if (e.Key == Avalonia.Input.Key.Enter)
        {
            viewModel.AddItemByBarcode();
        }
    }

    private void NumericUpDown_ValueChanged(object? sender, Avalonia.Controls.NumericUpDownValueChangedEventArgs e)
    {
        viewModel.RecalculateTotal();
    }
}