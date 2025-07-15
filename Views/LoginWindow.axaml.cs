using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;
using BhyatPos.Services;
using BhyatPos.ViewModels;
namespace BhyatPos.Views;

public partial class LoginWindow : Window
{
    public LoginWindow()
    {
        InitializeComponent();
        LoginButton.Click += OnLoginClick;
    }

    private void OnLoginClick(object? sender, RoutedEventArgs e)
    {
        var auth = new AuthService();

        if (string.IsNullOrWhiteSpace(UsernameBox.Text) || string.IsNullOrWhiteSpace(PasswordBox.Text))
        {
            ErrorLabel.Text = "Please fill in both fields.";
            ErrorLabel.Foreground = Brushes.Red;
            ErrorLabel.IsVisible = true;
            return;
        }

        bool success = auth.AuthenticateUser(UsernameBox.Text, PasswordBox.Text);

        if (success)
        {
            ErrorLabel.Text = "Login Successful!";
            ErrorLabel.Foreground = Brushes.Green;


            var vm = new MainWindowViewModel
            {
                Username = UsernameBox.Text,
            };

            var mainWindow = new MainWindow
            {
                DataContext = vm,
            };

            mainWindow.Show();
            this.Close();
        }
        else
        {
            ErrorLabel.Text = "Login Failed!";
            ErrorLabel.Foreground = Brushes.Red;
        }

        ErrorLabel.IsVisible = true;

    }
}