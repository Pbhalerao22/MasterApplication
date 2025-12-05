# Build stage
FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build

WORKDIR /src

# Copy everything
COPY . .

# Navigate into the project folder
WORKDIR /src/MasterApplication

# Restore
RUN dotnet restore

# Publish
RUN dotnet publish -c Release -o /app/publish


# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS final
WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "MasterApplication.dll"]
