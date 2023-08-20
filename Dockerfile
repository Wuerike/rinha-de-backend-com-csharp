FROM mcr.microsoft.com/dotnet/aspnet:8.0-preview AS base
WORKDIR /app
EXPOSE 6000

ENV ASPNETCORE_URLS=http://+:6000

USER app
FROM mcr.microsoft.com/dotnet/sdk:8.0-preview AS build
ARG configuration=Release
WORKDIR /src
COPY ["src/RinhaBackend.csproj", "./"]
RUN dotnet restore "RinhaBackend.csproj"
COPY src .
RUN dotnet build "RinhaBackend.csproj" -c $configuration -o /app/build

FROM build AS publish
ARG configuration=Release
RUN dotnet publish "RinhaBackend.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RinhaBackend.dll"]
