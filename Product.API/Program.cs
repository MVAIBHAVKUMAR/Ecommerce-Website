using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Product.API.Extensions;
using Product.API.Middleware;
using Product.Infrastructure;
using Product.Infrastructure.Data;
using StackExchange.Redis;
using System.Reflection;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options => options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true);
builder.Services.AddApiRegistration();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//builder.Services.AddEndpointsApiExplorer();
builder.Services.InfraStructureConfigration(builder.Configuration);
//Configure Redis
//builder.Services.AddSingleton<IConnectionMultiplexer>(i =>
//{
//    var configure = ConfigurationOptions.Parse(builder.Configuration.GetConnectionString("Redis"), true);
//    return ConnectionMultiplexer.Connect(configure);
//});

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefualtConnection")));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("CorsPolicy");
app.UseMiddleware<ExceptionMiddleware>();
app.UseStatusCodePagesWithReExecute("/errors/{0}");

app.UseAuthorization();
app.UseStaticFiles();
app.MapControllers();

app.Run();
//Asp.Net Core 8 Web API :https://www.youtube.com/watch?v=UqegTYn2aKE&list=PLazvcyckcBwitbcbYveMdXlw8mqoBDbTT&index=1

