FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 5003
ENV ASPNETCORE_URLS http://*:5003

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src
COPY ["Auto.VehicleCatalog.API/Auto.VehicleCatalog.API.csproj", "Auto.VehicleCatalog.API/"]
RUN dotnet restore "Auto.VehicleCatalog.API/Auto.VehicleCatalog.API.csproj"
COPY . .
WORKDIR "/src/Auto.VehicleCatalog.API"
RUN dotnet build "Auto.VehicleCatalog.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Auto.VehicleCatalog.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Auto.VehicleCatalog.API.dll"]
