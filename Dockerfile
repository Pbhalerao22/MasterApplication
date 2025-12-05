# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build

WORKDIR /src

# Copy everything from repo
COPY . .

# Enter the project folder EXACTLY as it is named in your repo
WORKDIR /src/MasterApplication

# Restore dependencies
RUN dotnet restore

# Publish
RUN dotnet publish -c Release -o /app/publish


# Runtime Stage
FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS final

WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "MasterApplication.dll"]
