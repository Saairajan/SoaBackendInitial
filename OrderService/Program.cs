using Common.Services.Implementations;
using Microsoft.EntityFrameworkCore;
using OrderService.DbConfig;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<OrderDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSqlConnection")));

builder.Services.AddControllers();
builder.Services.AddSingleton<RabbitMqConfig>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(); 
builder.Services.AddScoped<OrderService.Services.Implementations.OrderService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();
// app.UseCors("AllowSpecificOrigins");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
