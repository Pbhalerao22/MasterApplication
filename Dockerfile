FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src

# Copy project file
COPY MasterApplication.csproj ./

# Copy the external DLL folder
COPY bin/Debug/netcoreapp3.1/ ./DLL

# Copy rest of the source code
COPY . ./

# Publish
RUN dotnet publish MasterApplication.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS final
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
ENTRYPOINT ["dotnet", "MasterApplication.dll"]
