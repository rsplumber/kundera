FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Bootstrap/Bootstrap.csproj", "Bootstrap/"]
RUN dotnet restore "Bootstrap/Bootstrap.csproj"
COPY . .
WORKDIR "/src/Bootstrap"
RUN dotnet build "Bootstrap.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Bootstrap.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Bootstrap.dll"]
