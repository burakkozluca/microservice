FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Services.ProductAPI/Services.ProductAPI.csproj", "Services.ProductAPI/"]
RUN dotnet restore "Services.ProductAPI/Services.ProductAPI.csproj"
COPY . .
WORKDIR "/src/Services.ProductAPI"
RUN dotnet build "Services.ProductAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Services.ProductAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Services.ProductAPI.dll"] 