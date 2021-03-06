using chatsolution.Core.Services;
using chatsolution.Data;
using chatsolution.Hubs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddSignalR();

builder.Services.AddScoped<IStockService, StockService>();
builder.Services.AddSingleton<IQueuePublisherService, QueuePublisherService>();
builder.Services.AddScoped<IMessageRepository, MessageRepository>();

builder.Services.AddHostedService<QueueConsumerService>();

builder.Services.AddHttpClient();


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
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=History}/{action=Index}/{id?}");

app.UseAuthorization();

app.MapRazorPages();
app.MapHub<ChatHub>("/chathub");

app.Run();
