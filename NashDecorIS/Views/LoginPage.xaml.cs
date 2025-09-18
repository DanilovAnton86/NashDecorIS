using NashDecor;
using Microsoft.EntityFrameworkCore;
using NashDecor.Data.Data;
using NashDecor.Data.ModelEF;
using NashDecorIS.ViewModels;

namespace NashDecorIS.Views
{
    public partial class LoginPage : ContentPage
    {
        private readonly LoginViewModel _viewModel;

        public LoginPage(LoginViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = _viewModel = viewModel;

            // Подписываемся на события
            _viewModel.LoginCompleted += OnLoginCompleted;
            _viewModel.LoginFailed += OnLoginFailed;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            // Сброс полей при появлении страницы
            _viewModel.Username = string.Empty;
            _viewModel.Password = string.Empty;
            _viewModel.HasError = false;
        }

        private async void OnLoginCompleted(object sender, EventArgs e)
        {
            // Успешный вход - переход на главную страницу
            await Shell.Current.GoToAsync("//MainPage");
        }

        private void OnLoginFailed(object sender, string errorMessage)
        {
            // Показать сообщение об ошибке
            DisplayAlert("Ошибка", errorMessage, "OK");
        }

        // Обработка аппаратной кнопки "Назад" на Android
        protected override bool OnBackButtonPressed()
        {
            // Запрещаем выход по кнопке "Назад" на странице логина
            return true;
        }

        // Очистка ресурсов
        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            // Отписываемся от событий
            _viewModel.LoginCompleted -= OnLoginCompleted;
            _viewModel.LoginFailed -= OnLoginFailed;
        }
    }
}