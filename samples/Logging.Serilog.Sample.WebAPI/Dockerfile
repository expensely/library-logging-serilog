FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["samples/Logging.Serilog.Sample.WebAPI/Logging.Serilog.Sample.WebAPI.csproj", "samples/Logging.Serilog.Sample.WebAPI/"]
RUN dotnet restore "samples/Logging.Serilog.Sample.WebAPI/Logging.Serilog.Sample.WebAPI.csproj"
COPY . .
WORKDIR "/src/samples/Logging.Serilog.Sample.WebAPI"
RUN dotnet build "Logging.Serilog.Sample.WebAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Logging.Serilog.Sample.WebAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Logging.Serilog.Sample.WebAPI.dll"]
