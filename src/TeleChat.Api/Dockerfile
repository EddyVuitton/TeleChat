FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore "TeleChat.Api/TeleChat.Api.csproj"
WORKDIR "/src/TeleChat.Api"
RUN dotnet build "TeleChat.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TeleChat.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TeleChat.Api.dll"]