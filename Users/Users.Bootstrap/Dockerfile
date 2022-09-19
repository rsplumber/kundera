FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Users/Users.Bootstrap/Users.Bootstrap.csproj", "Users.Bootstrap/"]
RUN dotnet restore "Users/Users.Bootstrap/Users.Bootstrap.csproj"
COPY . .
WORKDIR "/src/Users.Bootstrap"
RUN dotnet build "Users.Bootstrap.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Users.Bootstrap.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Users.Bootstrap.dll"]
