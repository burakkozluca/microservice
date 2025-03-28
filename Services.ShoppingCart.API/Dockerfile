FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Services.ShoppingCart.API/Services.ShoppingCart.API.csproj", "Services.ShoppingCart.API/"]
RUN dotnet restore "Services.ShoppingCart.API/Services.ShoppingCart.API.csproj"
COPY . .
WORKDIR "/src/Services.ShoppingCart.API"
RUN dotnet build "Services.ShoppingCart.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Services.ShoppingCart.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Services.ShoppingCart.API.dll"] 