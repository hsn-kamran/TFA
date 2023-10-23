using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Filters;
using System.Reflection;
using TFA.Api.Middlewares;
using TFA.Domain.DependencyInjection;
using TFA.Storage.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddLogging(cfg => cfg.AddSerilog(new LoggerConfiguration()
    .MinimumLevel.Debug()
        .WriteTo.Console()
        .WriteTo.File($"{Path.Combine(Environment.CurrentDirectory, "Logs", "log.log")}")
            .Filter.ByExcluding(Matching.FromSource("Microsoft"))
    .CreateLogger()));


builder.Services
    .AddForumDomain()
    .AddForumStorage(builder.Configuration.GetConnectionString("Postgres")!);


builder.Services.AddControllers();
builder.Services.AddAutoMapper(cfg => cfg.AddMaps(Assembly.GetExecutingAssembly()));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.UseMiddleware<ErrorHandlingMiddleware>();

app.Run();

public partial class Program { }