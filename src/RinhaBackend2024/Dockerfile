FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["src/RinhaBackend2024/RinhaBackend2024.csproj", "src/RinhaBackend2024/"]
RUN dotnet restore "src/RinhaBackend2024/RinhaBackend2024.csproj"
COPY . .
WORKDIR "/src/src/RinhaBackend2024"
RUN dotnet build "RinhaBackend2024.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "RinhaBackend2024.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RinhaBackend2024.dll"]
