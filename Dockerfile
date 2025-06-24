FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Solution və bütün .csproj fayllarını kopyala
COPY *.sln .

COPY Core/CRMSystem.Application/CRMSystem.Application.csproj ./Core/CRMSystem.Application/
COPY Core/CRMSystem.Domain/CRMSystem.Domain.csproj ./Core/CRMSystem.Domain/
COPY Infrastructure/CRMSystem.Infrastructure/CRMSystem.Infrastructure.csproj ./Infrastructure/CRMSystem.Infrastructure/
COPY Infrastructure/CRMSystem.Persistence/CRMSystem.Persistence.csproj ./Infrastructure/CRMSystem.Persistence/
COPY Presentation/CRMSystem.WebAPi/CRMSystem.WebAPi.csproj ./Presentation/CRMSystem.WebAPi/

# Restore
RUN dotnet restore

# Bütün source kodları kopyala
COPY . .

WORKDIR /app/Presentation/CRMSystem.WebAPi
RUN dotnet publish -c Release -o out

# Run
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/Presentation/CRMSystem.WebAPi/out .

ENTRYPOINT ["dotnet", "CRMSystem.WebAPi.dll"]
