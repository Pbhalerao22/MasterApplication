# Use the official .NET Core runtime image (no SDK needed)
FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS runtime

# Set working directory inside container
WORKDIR /app

# Copy all pre-built binaries from host to container
COPY bin/Debug/netcoreapp3.1/ ./

# Copy MVC Views and static files
COPY Views/ ./Views/
COPY wwwroot/ ./wwwroot/

# Copy configuration files
COPY appsettings*.json ./

# Expose port 8080
EXPOSE 8080

# Set ASP.NET Core to listen on port 8080
ENV ASPNETCORE_URLS=http://+:8080

# Run the main DLL
ENTRYPOINT ["dotnet", "MasterApplication.dll"]
