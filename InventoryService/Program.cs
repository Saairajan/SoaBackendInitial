using InventoryService.DbConfig;
using Microsoft.EntityFrameworkCore;
using Common.Services.Implementations;
using InventoryService.Services.Implementation;
using Microsoft.OpenApi.Models;


var builder = WebApplication.CreateBuilder(args);

// builder.Services.AddDbContext<InventoryDbContext>(options =>
//     options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"), 
//         new MySqlServerVersion(new Version(8, 0, 23))));

builder.Services.AddDbContext<InventoryDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSqlConnection")));


builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
}); 
builder.Services.AddSingleton<RabbitMqConfig>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo { Title = "Your API", Version = "v1" }); });
builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<ImageService>();

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins",
        builder =>
        {
            builder.WithOrigins("http://localhost:4200") // Next.js development server
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => 
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Your API V1");
    });
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseStaticFiles();
app.UseCors("AllowSpecificOrigins");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
