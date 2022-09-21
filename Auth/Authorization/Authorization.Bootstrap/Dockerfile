FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Auth/Authorization/Authorization.Bootstrap/Authorization.Bootstrap.csproj", "Authorization.Bootstrap/"]
RUN dotnet restore "Auth/Authorization/Authorization.Bootstrap/Authorization.Bootstrap.csproj"
COPY . .
WORKDIR "/src/Authorization.Bootstrap"
RUN dotnet build "Authorization.Bootstrap.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Authorization.Bootstrap.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Authorization.Bootstrap.dll"]
