# Build stage
FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src

# Copy only main project file
COPY MasterApplication.csproj ./

RUN dotnet restore MasterApplication.csproj

# Copy all source code
COPY . ./

# Publish to folder (includes Views, DLLs, static files)
RUN dotnet publish MasterApplication.csproj -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS final
WORKDIR /app

# Copy everything from the publish folder
COPY --from=build /app/publish .

# Expose Render port
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

# Run your application
ENTRYPOINT ["dotnet", "MasterApplication.dll"]
