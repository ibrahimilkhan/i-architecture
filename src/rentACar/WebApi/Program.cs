using Application;
using Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("secrets.json", optional: true, reloadOnChange: true);

builder.Services.AddPersistenceServices(builder.Configuration);

builder.Services.AddControllers();
builder.Services.AddApplicationServices();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers();


var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
