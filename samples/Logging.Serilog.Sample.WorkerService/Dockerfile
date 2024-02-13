FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["samples/Logging.Serilog.Sample.WorkerService/Logging.Serilog.Sample.WorkerService.csproj", "samples/Logging.Serilog.Sample.WorkerService/"]
RUN dotnet restore "samples/Logging.Serilog.Sample.WorkerService/Logging.Serilog.Sample.WorkerService.csproj"
COPY . .
WORKDIR "/src/samples/Logging.Serilog.Sample.WorkerService"
RUN dotnet build "Logging.Serilog.Sample.WorkerService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Logging.Serilog.Sample.WorkerService.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Logging.Serilog.Sample.WorkerService.dll"]
