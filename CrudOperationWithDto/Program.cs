using CrudOperationWithDto;
using CrudOperationWithDto.Interface;
using CrudOperationWithDto.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.

//register the context file here
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("Db")));
//here we will use AddScoped dependency injection 
// it will create new instance or object per http request
builder.Services.AddScoped<IProductService,ProductService>();
// what is AddTransient?
//it will create new instance or object every http request e.g. IEmailService,EmailService

//what is AddSingleton
// it will create only one instance or object for the whole application (e.g conventional middleware)

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

//prebuild middleware
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
