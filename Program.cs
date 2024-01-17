using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using welcomeApp.Services; // Импорт нашего сервиса LinkService
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Добавление сервисов в контейнер DI
builder.Services.AddControllersWithViews(); 
builder.Services.AddSingleton<LinkService>(); // Регистрация LinkService как Singleton

var app = builder.Build();

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
