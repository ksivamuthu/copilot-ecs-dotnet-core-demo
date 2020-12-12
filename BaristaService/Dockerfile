FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["BaristaService.csproj", "BaristaService/"]
RUN dotnet restore "BaristaService/BaristaService.csproj"

WORKDIR "/src/BaristaService"
COPY . .

RUN dotnet build "BaristaService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BaristaService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BaristaService.dll"]
