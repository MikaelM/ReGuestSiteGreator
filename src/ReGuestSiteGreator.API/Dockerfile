FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["src/ReGuestSiteGreator.API/ReGuestSiteGreator.API.csproj", "src/ReGuestSiteGreator.API/"]
COPY ["src/ReGuestSiteGreator.Application/ReGuestSiteGreator.Application.csproj", "src/ReGuestSiteGreator.Application/"]
COPY ["src/ReGuestSiteGreator.Domain/ReGuestSiteGreator.Domain.csproj", "src/ReGuestSiteGreator.Domain/"]
COPY ["src/ReGuestSiteGreator.Infrastructure/ReGuestSiteGreator.Infrastructure.csproj", "src/ReGuestSiteGreator.Infrastructure/"]
RUN dotnet restore "src/ReGuestSiteGreator.API/ReGuestSiteGreator.API.csproj"
COPY . .
WORKDIR "/src/src/ReGuestSiteGreator.API"
RUN dotnet build "ReGuestSiteGreator.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ReGuestSiteGreator.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ReGuestSiteGreator.API.dll"]
