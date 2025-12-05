# Use official .NET 8 SDK image for building
FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src

# Copy the project file and restore dependencies
# This assumes your MasterApplication.csproj is in the root of your repo
COPY MasterApplication.csproj ./
RUN dotnet restore

# Copy everything else and build
COPY . .
RUN dotnet publish -c Release -o /app/publish

# Use .NET runtime for running
FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Expose the correct ports for Render's environment
EXPOSE 8080

# Set environment variable for Render
ENV ASPNETCORE_URLS=http://+:8080

# The entry point command must match the name of your published DLL
ENTRYPOINT ["dotnet", "MasterApplication.dll"]
