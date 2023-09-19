using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Metadata;
using Microsoft.VisualStudio.Services.Identity;
using MP4practic.Context;
using MP4practic.Models;
using MP4practic.Views;
using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using MessageBox.Avalonia;

namespace MP4practic.DataSourse
{
    public class AurorizashionSourse : INotifyPropertyChanged
    {
        private static PostgresContext _context;
        private string login = null!;
        private string password = null!;
        private string textcapch = null!;

        public string Login
        {
            get { return login; }
            set
            {
                login = value;
                CheckPassLog();
                NotifyPropertyChanged();
            }
        }

        public string Password
        {
            get { return password; }
            set
            {
                password = value;
                CheckPassLog();
                NotifyPropertyChanged();
            }
        }

        public string CapchText
        {
            get { return textcapch; }
            set
            {
                textcapch = value;
                NotifyPropertyChanged();
            }
        }

        private int o = 0;
        private bool check = false;

        public AurorizashionSourse()
        {
            _context = PostgresContext.GetContext();
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public void CheckPassLog()
        {
            if (o == 0)
            {
                if (Password != null && Login != null)
                {
                    if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
                    {
                        desktop.Windows.OfType<Authorization>().First().LogIn.IsEnabled = true;
                        o++;
                    }
                }
            }
        }

        public void GostButton()
        {
            LobbyApp lobby = new LobbyApp();
            lobby.Show();

            if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.Windows.OfType<Authorization>().First().Close();
            }
        }

        public void Test()
        {
            if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {

                string str = desktop.Windows.OfType<Authorization>().First().firstSim.Text +
                             desktop.Windows.OfType<Authorization>().First().secondSim.Text +
                             desktop.Windows.OfType<Authorization>().First().threeSim.Text +
                             desktop.Windows.OfType<Authorization>().First().fourSim.Text;
                if (CapchText == str)
                {
                    check = false;
                    var mess = MessageBoxManager.GetMessageBoxStandardWindow("Проверка пройдена успешна",
                        "Капча введена верно");
                    mess.ShowDialog(desktop.Windows.OfType<Authorization>().First());
                }
                else
                {
                    desktop.Windows.OfType<Authorization>().First().CreateCapch();
                    desktop.Windows.OfType<Authorization>().First().LogIn.IsEnabled = false;
                    var mess = MessageBoxManager.GetMessageBoxStandardWindow("Ошибка капчи",
                        "Капча введена неверно. До следующей попытки входа 10 секунд.");
                    mess.ShowDialog(desktop.Windows.OfType<Authorization>().First());
                    TenSecond();
                }
            }
        }

        public async void TenSecond()
        {
            await Task.Delay(TimeSpan.FromSeconds(10));
            if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.Windows.OfType<Authorization>().First().LogIn.IsEnabled = true;
            }
        }

        public void LogInButton()
        {
            try
            {
                var i = _context.Users.Where(x => x.Userlogin == Login && x.Userpassword == Password).First();
                if (i != null)
                {
                    if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop1)
                    {
                        string str = desktop1.Windows.OfType<Authorization>().First().firstSim.Text +
                                     desktop1.Windows.OfType<Authorization>().First().secondSim.Text +
                                     desktop1.Windows.OfType<Authorization>().First().threeSim.Text +
                                     desktop1.Windows.OfType<Authorization>().First().fourSim.Text;
                        if (desktop1.Windows.OfType<Authorization>().First().CapchaPanel.IsVisible = true)
                        {
                            if (str == "")
                            {
                                var mess = MessageBoxManager.GetMessageBoxStandardWindow("Не введена капча",
                                    "Введите капчу для входа");
                                mess.ShowDialog(desktop1.Windows.OfType<Authorization>().First());
                            }
                            else if (str.Length != 4)
                            {
                                var mess = MessageBoxManager.GetMessageBoxStandardWindow("Ошибка капчи",
                                    "Символов капчи больше/меньше нужного");
                                mess.ShowDialog(desktop1.Windows.OfType<Authorization>().First());
                            }
                            else if (check == true)
                            {
                                var mess = MessageBoxManager.GetMessageBoxStandardWindow("Ошибка капчи",
                                    "Вы не проверили капчу");
                                mess.ShowDialog(desktop1.Windows.OfType<Authorization>().First());
                            }
                            else
                            {
                                if (i != null)
                                {
                                    LobbyApp lobby = new LobbyApp(i.Username + "_" + i.Usersurname, i.Userrole);
                                    lobby.Show();

                                    if (Application.Current?.ApplicationLifetime is
                                        IClassicDesktopStyleApplicationLifetime
                                        desktop)
                                    {
                                        desktop.Windows.OfType<Authorization>().First().Close();
                                    }

                                }
                                

                            }

                        }
                    }
                }
                else
                {
                    if (Application.Current?.ApplicationLifetime is
                        IClassicDesktopStyleApplicationLifetime
                        desktop)
                    {
                        var mess = MessageBoxManager.GetMessageBoxStandardWindow("Ошибка авторизации",
                            "Неверный логин или пароль");
                        mess.ShowDialog(desktop.Windows.OfType<Authorization>().First());
                        Login = "";
                        Password = "";
                        check = true;
                        desktop.Windows.OfType<Authorization>().First().passwordUser.Text = "";
                        desktop.Windows.OfType<Authorization>().First().loginUser.Text = "";
                        desktop.Windows.OfType<Authorization>().First().LogIn.IsEnabled = false;
                        desktop.Windows.OfType<Authorization>().First().CapchaPanel.IsVisible = true;
                        desktop.Windows.OfType<Authorization>().First().BorderPanel.IsVisible = true;
                    }
                }
            }catch
            {
                if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime
                    desktop)
                {
                    var mess = MessageBoxManager.GetMessageBoxStandardWindow("Ошибка авторизации",
                        "Неверный логин или пароль.");
                    mess.ShowDialog(desktop.Windows.OfType<Authorization>().First());
                    Login = "";
                    Password = "";
                    check = true;
                    desktop.Windows.OfType<Authorization>().First().passwordUser.Text = "";
                    desktop.Windows.OfType<Authorization>().First().loginUser.Text = "";
                    desktop.Windows.OfType<Authorization>().First().LogIn.IsEnabled = false;
                    desktop.Windows.OfType<Authorization>().First().CapchaPanel.IsVisible = true;
                    desktop.Windows.OfType<Authorization>().First().BorderPanel.IsVisible = true;
                }
            }
            
        }
    

    private void NotifyPropertyChanged([CallerMemberName] string propertuName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertuName));

        }
    }
}
