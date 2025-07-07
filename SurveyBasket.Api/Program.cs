using FluentValidation.AspNetCore;
using MapsterMapper;
using SurveyBasket.Api.Contracts.Validation;
using System.Reflection;
var builder = WebApplication.CreateBuilder(args);
// Add services to the container.  

builder.Services.AddControllers();
// Add mapster  
var mapperConfig = TypeAdapterConfig.GlobalSettings;
// Add fluent validation  
builder.Services.AddFluentValidationAutoValidation().
    AddValidatorsFromAssemblies(new[] { Assembly.GetExecutingAssembly() });
mapperConfig.Scan(Assembly.GetExecutingAssembly());
builder.Services.AddSingleton<IMapper>(new Mapper(mapperConfig));
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi  
builder.Services.AddOpenApi();
builder.Services.AddScoped<IPollService, PollService>();

var app = builder.Build();

// Configure the HTTP request pipeline.  
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options => options.SwaggerEndpoint("/openapi/v1.json", "v1"));
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
