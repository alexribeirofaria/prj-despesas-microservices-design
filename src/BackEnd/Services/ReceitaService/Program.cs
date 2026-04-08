using Application.Abstractions;
using Application.Dtos;
using Application.Dtos.Profile;
using Application.Implementations;
using AuthService.Register;
using Domain.Entities;
using Infrastructure.CommonInjectDependence;
using Repository.Persistency.Generic;
using Repository.UnitOfWork;
using Repository.UnitOfWork.Abstractions;

var builder = WebApplication.CreateBuilder(args);
  
builder.Configuration.AddEnvironmentVariables();

// Add services to the container.
builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.AddServerHeader = false;
    serverOptions.ListenAnyIP(9003);
});


builder.Services.AddConsulSettings(builder.Configuration);
builder.Services.AddIdentityServerSettings(builder.Configuration);

builder.Services.AddControllers();
builder.Services.AddScoped(typeof(IBusinessBase<ReceitaDto, Receita>), typeof(ReceitaBusinessImpl<ReceitaDto>));
builder.Services.AddAutoMapper(typeof(ReceitaProfile).Assembly);
builder.Services.ConfigureRegisterSqlContext(builder.Configuration);
builder.Services.AddScoped(typeof(IUnitOfWork<>), typeof(UnitOfWork<>));
builder.Services.AddScoped(typeof(IRepositorio<>), typeof(GenericRepositorio<>));
builder.Services.AddOptions();

var app = builder.Build();

app.UseConsul(); 
app.UseRouting();
app.MapControllers();
app.Run();
