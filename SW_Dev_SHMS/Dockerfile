# Use official ASP.NET Core runtime as base image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080

# Use SDK image to build the app
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["SW_Dev_SHMS/SW_Dev_SHMS.csproj", "SW_Dev_SHMS/"]
RUN dotnet restore "SW_Dev_SHMS/SW_Dev_SHMS.csproj"
COPY . .
WORKDIR "/src/SW_Dev_SHMS"
RUN dotnet build "SW_Dev_SHMS.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SW_Dev_SHMS.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SW_Dev_SHMS.dll"]
