# Build stage
FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build

WORKDIR /src

# Copy everything from repo root
COPY . .

# Restore project (csproj is in root)
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
