# ==========================================
# STAGE 1: Compiling
# ==========================================
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy Project files (.csproj) 
COPY ["src/LabAws.Api/LabAws.Api.csproj", "src/LabAws.Api/"]
COPY ["src/LabAws.Application/LabAws.Application.csproj", "src/LabAws.Application/"]
COPY ["src/LabAws.Domain/LabAws.Domain.csproj", "src/LabAws.Domain/"]
COPY ["src/LabAws.Infrastructure/LabAws.Infrastructure.csproj", "src/LabAws.Infrastructure/"]
COPY ["src/AwsLab.Web/AwsLab.Web.csproj", "src/AwsLab.Web/"]

# Restore Nuget packages
RUN dotnet restore "src/LabAws.Api/LabAws.Api.csproj"

# Copy anything else to the container
COPY . .

# Compile API project in Release saving in /app/publish
WORKDIR "/src/src/LabAws.Api"
RUN dotnet publish "LabAws.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# ==========================================
# STAGE 2: Runtime
# ==========================================
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS final
WORKDIR /app

# Exposes port to .NET 8 use in Linux containers (8080)
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

# Copy build files
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "LabAws.Api.dll"]
