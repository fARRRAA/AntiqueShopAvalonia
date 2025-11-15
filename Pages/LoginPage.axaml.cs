using AntiqueShopAvalonia.Model;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System.Linq;
using System.Threading.Tasks;

namespace AntiqueShopAvalonia;

public partial class LoginPage : UserControl
{
    public LoginPage()
    {

        InitializeComponent();
        EmailTextBox.Text = "ivan.admin@example.com";
        PasswordTextBox.Text = "123";
    }

    private async void LoginButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        ErrorText.IsVisible = false;
        var email = EmailTextBox.Text?.Trim() ?? string.Empty;
        var password = PasswordTextBox.Text ?? string.Empty;

        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
        {
            ShowError("write email and adress");
            return;
        }

        using var db = new AppDbContext();
        var user = await Task.Run(() =>
            db.Users.FirstOrDefault(u => u.Email == email && u.Password == password));

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

        //var isTeacher = await Task.Run(() => db.CourseTeachers.Any(ct => ct.TeacherId == user.Id));
        //if (isTeacher)
        //{
        //    parentWindow.Navigate(new TeacherPage(user.Id, Logout), "EduTrack � �������������");

        //    return;
        //}

        //var isStudent = await Task.Run(() => db.CourseEnrollments.Any(ce => ce.StudentId == user.Id));
        //if (isStudent)
        //{
        //    parentWindow.Navigate(new StudentPage(user.Id, Logout), "EduTrack � �������");
        //    return;
        //}

        parentWindow.Navigate(new AdminPage(user.Id, Logout), "AntiqueShop - Admin");
    }
    private void ShowError(string message)
    {
        ErrorText.Text = message;
        ErrorText.IsVisible = true;
    }
}