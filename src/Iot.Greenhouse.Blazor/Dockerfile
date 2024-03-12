#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["NuGet.Config", "."]
COPY ["src/Iot.Greenhouse.Blazor/Iot.Greenhouse.Blazor.csproj", "src/Iot.Greenhouse.Blazor/"]
COPY ["src/Iot.Greenhouse.Application/Iot.Greenhouse.Application.csproj", "src/Iot.Greenhouse.Application/"]
COPY ["src/Iot.Greenhouse.Domain/Iot.Greenhouse.Domain.csproj", "src/Iot.Greenhouse.Domain/"]
COPY ["src/Iot.Greenhouse.Domain.Shared/Iot.Greenhouse.Domain.Shared.csproj", "src/Iot.Greenhouse.Domain.Shared/"]
COPY ["src/Iot.Greenhouse.Application.Contracts/Iot.Greenhouse.Application.Contracts.csproj", "src/Iot.Greenhouse.Application.Contracts/"]
COPY ["src/Iot.Greenhouse.HttpApi/Iot.Greenhouse.HttpApi.csproj", "src/Iot.Greenhouse.HttpApi/"]
COPY ["src/Iot.Greenhouse.EntityFrameworkCore/Iot.Greenhouse.EntityFrameworkCore.csproj", "src/Iot.Greenhouse.EntityFrameworkCore/"]
RUN dotnet restore "./src/Iot.Greenhouse.Blazor/./Iot.Greenhouse.Blazor.csproj"
COPY . .
WORKDIR "/src/src/Iot.Greenhouse.Blazor"
RUN dotnet build "./Iot.Greenhouse.Blazor.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Iot.Greenhouse.Blazor.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Iot.Greenhouse.Blazor.dll"]