using ToDoList.Domain.Models;
using ToDoList.Persistence;
using ToDoList.Persistence.Repositories;


var builder = WebApplication.CreateBuilder(args);
{
    // Kestrel server configuration
    builder.WebHost.ConfigureKestrel(options =>
    {
        // Listen on port 5000 for HTTP requests
        options.ListenAnyIP(5000); // Listen on all available network interfaces on port 5000
    });

    //WebApi services
    builder.Services.AddControllers();
    builder.Services.AddSwaggerGen();

    //Persistence services
    builder.Services.AddDbContext<ToDoItemsContext>();
    builder.Services.AddScoped<IRepositoryAsync<ToDoItem>, ToDoItemsRepository>();
}

var app = builder.Build();
{
    //Configure Middleware (HTTP request pipeline)
    app.MapControllers();
    app.UseSwagger();
    app.UseSwaggerUI(config => config.SwaggerEndpoint("/swagger/v1/swagger.json", "ToDoList API V1"));
}

app.Run();
