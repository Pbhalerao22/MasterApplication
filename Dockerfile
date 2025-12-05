FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src

# Copy only the project file first to leverage caching
COPY MasterApplication.csproj ./
RUN dotnet restore MasterApplication.csproj

# Copy the rest of the source code
COPY . ./

# Publish
RUN dotnet publish MasterApplication.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS final
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "MasterApplication.dll"]
