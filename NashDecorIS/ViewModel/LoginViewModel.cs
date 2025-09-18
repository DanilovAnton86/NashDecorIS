using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NashDecorIS.Services;

namespace NashDecorIS.ViewModels
{
    public partial class LoginViewModel : ObservableObject
    {
        private readonly IAuthServices _authService;

        [ObservableProperty]
        private string _username;

        [ObservableProperty]
        private string _password;

        [ObservableProperty]
        private string _errorMessage;

        [ObservableProperty]
        private bool _hasError;

        [ObservableProperty]
        private bool _isBusy;

        public event EventHandler LoginCompleted;
        public event EventHandler<string> LoginFailed;

        public LoginViewModel(IAuthServices authService)
        {
            _authService = authService;
        }

        [RelayCommand]
        private async Task Login()
        {
            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
            {
                ErrorMessage = "Заполните все поля";
                HasError = true;
                return;
            }

            var success = await _authService.LoginAsync(Username, Password);

            if (success)
            {
                HasError = false;
                await Shell.Current.GoToAsync("//MainPage");
            }
            else
            {
                ErrorMessage = "Неверный логин или пароль";
                HasError = true;
            }
        }
    }
}