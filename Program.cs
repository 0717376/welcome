using Microsoft.AspNetCore.Builder;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using welcomeApp.Services; // Импорт сервиса LinkService
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Добавление сервисов в контейнер DI
builder.Services.AddControllersWithViews(); 
builder.Services.AddSingleton<LinkService>(); // Регистрация LinkService как Singleton

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
