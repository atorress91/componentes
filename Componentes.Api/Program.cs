using Componentes.Core.Middlewares;
using Componentes.Ioc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHealthChecks();
builder.Services.IocInjectAllDependencies();

builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseCors();
app.UseAuthorization();

app.UseSwagger();
app.UseSwaggerUI(s => { s.SwaggerEndpoint("/swagger/v1/swagger.json", "ProyectoComponentes API"); });

var logger = app.Services.GetRequiredService<ILogger<Program>>();
logger.LogInformation("Application started");

app.UseMiddleware<ExceptionMiddleware>();
app.UseMiddleware<JwtMiddleware>();

app.MapHealthChecks("/health");
app.MapControllers();

app.Run();