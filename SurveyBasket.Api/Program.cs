using Microsoft.EntityFrameworkCore;
using SurveyBasket.Api.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDependecies(builder.Configuration);

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
