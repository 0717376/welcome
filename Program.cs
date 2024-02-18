using Microsoft.AspNetCore.Builder;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using welcomeApp.Services; // Импорт сервисов
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;

var builder = WebApplication.CreateBuilder(args);

// Добавление сервисов в контейнер DI
builder.Services.AddControllersWithViews();
builder.Services.AddSingleton<LinkService>(); // Регистрация LinkService как Singleton

// Регистрация HttpClient для использования в OfficeLocationService с автоматической распаковкой сжатого содержимого
builder.Services.AddHttpClient<OfficeLocationService>().ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
{
    AutomaticDecompression = System.Net.DecompressionMethods.GZip | System.Net.DecompressionMethods.Deflate
});

var app = builder.Build();

// Установка культуры
var supportedCultures = new[] { "ru-RU" };
var localizationOptions = new RequestLocalizationOptions()
    .SetDefaultCulture(supportedCultures[0])
    .AddSupportedCultures(supportedCultures)
    .AddSupportedUICultures(supportedCultures);

app.UseRequestLocalization(localizationOptions);

// Настройка конвейера запросов HTTP
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseStaticFiles(); // Для обслуживания статических файлов из wwwroot
app.UseRouting();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Link}/{action=GetLinkPage}/{guid?}");

app.Run();
