FROM  mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER root
WORKDIR /app
EXPOSE 9001

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build 
WORKDIR /src/
COPY ./src .

WORKDIR /src/BackEnd/Services/CategoriaService
RUN dotnet build --restore -c Release -o /app/build

FROM build AS publish
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app

COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CategoriaService.dll"]