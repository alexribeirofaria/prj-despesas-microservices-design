FROM  mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER app
WORKDIR /app
EXPOSE 5055
EXPOSE 7199

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build 
WORKDIR /src/
COPY ./src/IdentityServer .

RUN dotnet publish "./sts-server.csproj" -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "sts-server.dll"]