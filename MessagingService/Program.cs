using System.Text.Json;
using System.Text.Json.Serialization;
using MessagingService.Interfaces;
using MessagingService.Providers;
using MessagingService.Services;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Converters;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
.AddJsonOptions(o =>
    {
        o.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<IMessageService, MessageService>();
builder.Services.AddTransient<IWebHookService, WebHookService>();
builder.Services.AddScoped<MessageProviderFactory>();
builder.Services.AddDbContext<AppDbContext>(options =>
options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.ListenLocalhost(8080);
});
builder.Services.AddScoped<MockSmsProvider>();
builder.Services.AddScoped<MockEmailProvider>();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();

