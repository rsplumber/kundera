FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 5178
EXPOSE 5179

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
RUN mkdir "outp"
COPY ["./." , "outp/"]

RUN dotnet restore "outp/Web/Web.csproj"

RUN dotnet build "outp/Web/Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "outp/Web/Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Web.dll"]

