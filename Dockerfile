FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["ECSDotNetCoreDemo/ECSDotNetCoreDemo.csproj", "ECSDotNetCoreDemo/"]
RUN dotnet restore "ECSDotNetCoreDemo/ECSDotNetCoreDemo.csproj"
COPY . .
WORKDIR "/src/ECSDotNetCoreDemo"
RUN dotnet build "ECSDotNetCoreDemo.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ECSDotNetCoreDemo.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ECSDotNetCoreDemo.dll"]
