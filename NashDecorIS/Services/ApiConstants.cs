namespace NashDecorIS.Services
{
    public static class ApiConstants
    {
        // Для Windows разработки
        public const string WindowsBaseUrl = "https://localhost:7000/";

        // Для Android эмулятора (10.0.2.2 - это localhost эмулятора)
        public const string AndroidEmulatorBaseUrl = "http://10.0.2.2:5000/";

        // Для реального Android устройства (укажите ваш IP)
        public const string AndroidDeviceBaseUrl = "http://192.168.1.100:5000/";

        // Для продакшена
        public const string ProductionBaseUrl = "https://your-production-api.com/";
    }
}