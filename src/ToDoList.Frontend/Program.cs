using ToDoList.Frontend.Clients;
using ToDoList.Frontend.Components;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents().AddInteractiveServerComponents();

// Kestrel server configuration
builder.WebHost.ConfigureKestrel(options =>
{
    // Listen on port 5001 for HTTP requests
    options.ListenAnyIP(5001); // Listen on all available network interfaces on port 5001
});

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.Configuration["ToDoItemApiAddress"] ?? "http://localhost:5000") });
builder.Services.AddScoped<IToDoItemsClient, ToDoItemsClient>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>().AddInteractiveServerRenderMode();

app.Run();
