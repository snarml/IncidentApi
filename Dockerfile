FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /api

# Copier tous les projets directement
COPY IncidentApiRimel/IncidentApiRimel.csproj IncidentApiRimel/
COPY AppTests/AppTests.csproj AppTests/

# Restore SANS solution
RUN dotnet restore IncidentApiRimel/IncidentApiRimel.csproj

COPY . .

RUN dotnet publish IncidentApiRimel/IncidentApiRimel.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app

ENV ASPNETCORE_URLS=http://0.0.0.0:80
EXPOSE 80

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "IncidentApiRimel.dll"]