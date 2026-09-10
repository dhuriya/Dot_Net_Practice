using CrudOperationWithRepo;
using CrudOperationWithRepo.Services;
using CrudOperationWithRepo.Services.IService;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

//register the context file here
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("Db")));

// Add services to the container

//we have three types of dependency injection in C#
//1.AddTransient: it creates a new instanace of the service every time it is requested.
//2.AddScoped: it creates a new instance of the serivce per request.
//3.AddSingleton: it creates a single instance of the service and shares it across all requests.
builder.Services.AddScoped<IProductService, ProductServices>();

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
