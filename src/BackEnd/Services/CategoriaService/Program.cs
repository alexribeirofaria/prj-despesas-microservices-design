using Application.Abstractions;
using Application.Dtos;
using Application.Implementations;
using AuthService.Register;
using CrossCutting.CommonDependenceInject;
using Domain.Entities;
using Infrastructure.CommonInjectDependence;
using Repository.Persistency.Generic;
using Repository.UnitOfWork;
using Repository.UnitOfWork.Abstractions;


var builder = WebApplication.CreateBuilder(args);

var env = builder.Environment;
builder.Configuration
    .SetBasePath(env.ContentRootPath)
    .AddEnvironmentVariables();

builder.Services.AddConsulSettings(builder.Configuration);
builder.Services.AddIdentityServerSettings(builder.Configuration);

builder.Services.AddControllers();
builder.Services.AddScoped(typeof(ICategoriaBusiness<CategoriaDto, Categoria>), typeof(CategoriaBusinessImpl<CategoriaDto>));
builder.Services.AddAutoMapper(typeof(CategoriaProfile).Assembly);

builder.Services.AddCrossCuttingConfiguration();
builder.Services.AddScoped(typeof(IUnitOfWork<>), typeof(UnitOfWork<>));
builder.Services.AddScoped(typeof(IRepositorio<>), typeof(GenericRepositorio<>));
builder.Services.ConfigureRegisterSqlContext(builder.Configuration);

var app = builder.Build();

app.UseHsts();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseConsul();
app.MapControllers();
app.Run();