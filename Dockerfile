FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 5179
EXPOSE 7179


FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
RUN mkdir "outp"
COPY ["./." , "outp/"]

RUN dotnet restore "outp/Web.Api/Web.Api.csproj"

RUN dotnet build "outp/Web.Api/Web.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "outp/Web.Api/Web.Api.csproj" -c Release -o /app/publish



FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Web.Api.dll"]
