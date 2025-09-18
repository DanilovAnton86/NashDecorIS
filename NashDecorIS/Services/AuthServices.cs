using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace NashDecorIS.Services
{
    public interface IAuthServices
    {
        Task<bool> LoginAsync(string username, string password);
        void Logout();
        bool IsAuthenticated { get; }
        string CurrentUserName { get; }
        string CurrentUserRole { get; }
        bool IsUser {  get; }
        bool IsManager { get; }
        bool IsModerator { get; }
        bool IsAdmin { get; }
    }

    public class AuthServices : IAuthServices
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<AuthServices> _logger;

        private bool _isAuthenticated;
        private string _currentUserName;
        private string _currentUserRole;

        public bool IsAuthenticated => _isAuthenticated;
        public string CurrentUserName => _currentUserName;
        public string CurrentUserRole => _currentUserRole;

        public AuthServices(HttpClient httpClient, ILogger<AuthServices> logger)
        {
            _httpClient = httpClient;
            _logger = logger;

            // Базовый URL API
            _httpClient.BaseAddress = new Uri(GetBaseUrl());
            _httpClient.Timeout = TimeSpan.FromSeconds(30);
        }

        private string GetBaseUrl()
        {
#if WINDOWS
            return ApiConstants.WindowsBaseUrl;
#elif ANDROID
            // Для Android определяем эмулятор или устройство
            try
            {
                // Проверяем, работаем ли мы на эмуляторе
                var isEmulator = Android.OS.Build.Product.Contains("sdk") || 
                                Android.OS.Build.Model.Contains("sdk") ||
                                Android.OS.Build.Manufacturer.Contains("unknown");
                
                return isEmulator ? 
                    ApiConstants.AndroidEmulatorBaseUrl : 
                    ApiConstants.AndroidDeviceBaseUrl;
            }
            catch
            {
                // Если возникла ошибка (например, на Windows при компиляции для Android)
                return ApiConstants.AndroidEmulatorBaseUrl;
            }
#else
            return ApiConstants.ProductionBaseUrl;
#endif
        }
        public bool IsUser => CurrentUserRole?.Contains("User", StringComparison.OrdinalIgnoreCase) == true ||
                CurrentUserRole?.Contains("Пользователь", StringComparison.OrdinalIgnoreCase) == true;

        public bool IsManager => CurrentUserRole?.Contains("Manager", StringComparison.OrdinalIgnoreCase) == true ||
                CurrentUserRole?.Contains("Менеджер", StringComparison.OrdinalIgnoreCase) == true;

        public bool IsModerator => CurrentUserRole?.Contains("Moderator", StringComparison.OrdinalIgnoreCase) == true ||
                CurrentUserRole?.Contains("Модератор", StringComparison.OrdinalIgnoreCase) == true;

        public bool IsAdmin => CurrentUserRole?.Contains("Admin", StringComparison.OrdinalIgnoreCase) == true ||
                CurrentUserRole?.Contains("Администратор", StringComparison.OrdinalIgnoreCase) == true;

        public async Task<bool> LoginAsync(string username, string password)
        {
            try
            {
                _logger.LogInformation("Attempting login for: {Username}", username);

                var loginData = new { Username = username, Password = password };
                var response = await _httpClient.PostAsJsonAsync("api/auth/login", loginData);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<LoginResult>();

                    _isAuthenticated = true;
                    _currentUserName = result.FullName;
                    _currentUserRole = result.Role;

                    _logger.LogInformation("✅ Login successful: {Name}, Role: {Role}", CurrentUserName, CurrentUserRole);
                    return true;
                }

                _logger.LogWarning("❌ Login failed for: {Username}", username);
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Login error for: {Username}", username);
                return false;
            }
        }

        public void Logout()
        {
            _isAuthenticated = false;
            _currentUserName = null;
            _currentUserRole = null;
            _logger.LogInformation("User logged out");
        }

        private class LoginResult
        {
            public string FullName { get; set; }
            public string Role { get; set; }
        }
    }
}