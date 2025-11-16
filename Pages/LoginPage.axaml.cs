using AntiqueShopAvalonia.Model;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace AntiqueShopAvalonia;

public partial class LoginPage : UserControl
{
    public LoginPage()
    {

        InitializeComponent();
        LoginTextBox.Text = "admin";
        PasswordTextBox.Text = "123";
    }

    private async void LoginButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        var errorContainer = this.FindControl<Border>("ErrorContainer");
        if (errorContainer != null)
            errorContainer.IsVisible = false;
        var login = LoginTextBox.Text?.Trim() ?? string.Empty;
        var password = PasswordTextBox.Text ?? string.Empty;

        if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password))
        {
            ShowError("write login and adress");
            return;
        }

        using var db = new AppDbContext();
        var user = await Task.Run(() =>
            db.Users.Include(x=>x.Role).FirstOrDefault(u => u.Login == login && u.Password == password));

        if (user is null)
        {
            ShowError("this user dont exist");
            return;
        }
        var parentWindow = (MainWindow)TopLevel.GetTopLevel(this)!;

        void Logout()
        {
            parentWindow.Navigate(new LoginPage(), "AntiqueShop");

        }
        if (user.Role.Name == "Менеджер")
        {
            parentWindow.Navigate(new ManagerPage(user.Id, Logout), "AntiqueShop - Manager");
            return;
        }
        if (user.Role.Name == "Кассир")
        {
            parentWindow.Navigate(new KassirPage(user.Id, Logout), "AntiqueShop - Cashier");
            return;
        }
        if (user.Role.Name == "Бухгалтер")
        {
            parentWindow.Navigate(new KassirPage(user.Id, Logout), "AntiqueShop - Accountant");
            return;
        }
        parentWindow.Navigate(new AdminPage(user.Id, Logout), "AntiqueShop - Admin");
    }
    private void ShowError(string message)
    {
        var errorContainer = this.FindControl<Border>("ErrorContainer");
        ErrorText.Text = message;
        if (errorContainer != null)
            errorContainer.IsVisible = true;
    }
}