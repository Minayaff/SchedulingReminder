using Microsoft.EntityFrameworkCore;
using SchedulingReminders.DAL;
using SchedulingReminders.Services;
using SchedulingReminders.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers(); // Add this line for Web API
builder.Services.AddDbContext<ReminderDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))); // For MSSQL
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddSingleton<ITelegramService, TelegramService>();
builder.Services.AddScoped<IReminderRepository, ReminderRepository>(); // Add this line to register the repository

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllers(); // Add this line for Web API



app.Run();
