#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["services/DossierService/DossierService.csproj", "services/DossierService/"]
RUN dotnet restore "services/DossierService/DossierService.csproj"
COPY . .
WORKDIR "/src/services/DossierService"
RUN dotnet build "DossierService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DossierService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DossierService.dll"]