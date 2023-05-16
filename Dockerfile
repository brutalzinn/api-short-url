#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
ENV TZ="America/Sao_Paulo"

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /app
COPY ["app/ApiShortUrl.csproj", "."]
RUN dotnet restore "./ApiShortUrl.csproj"
COPY . .
WORKDIR "/app/."
RUN dotnet build "ApiShortUrl.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ApiShortUrl.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ApiShortUrl.dll"]