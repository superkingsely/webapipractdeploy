# =========================build stage=========================
FROM mcr.microsoft.com/dotnet/sdk:10.0 as build
WORKDIR /src

Copy webapiapp/ webapiapp/
copy infrastructure/ infrastructure/

Run dotnet restore "webapiapp/webapiapp.csproj"

copy . .
workdir "/src/webapiapp"
Run dotnet build "webapiapp.csproj" -c Release -o /app/build

# =========================publish stage=========================
run dotnet publish "webapiapp.csproj" -c Release -o /app/publish /p:UseAppHost=false
# =========================runtime stage=========================
from mcr.microsoft.com/dotnet/aspnet:10.0 as final

WORKDIR /app
# note the dot below .=/app
COPY --from=build /app/publish .
# =========================expose port=========================
ENV ASPNETCORE_HTTP_PORTS=10000
EXPOSE 10000
# ===================entrypoint=========================
ENTRYPOINT ["dotnet", "webapiapp.dll"]



# # ================================
# # 1. BUILD STAGE
# # ================================
# FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

# WORKDIR /src

# # Copy project files first
# COPY ["webapiapp/webapiapp.csproj", "webapiapp/"]
# COPY ["infrastructure/infrastructure.csproj", "infrastructure/"]

# # Restore dependencies
# RUN dotnet restore "webapiapp/webapiapp.csproj"

# # Copy the remaining source code
# COPY . .

# # Build
# WORKDIR "/src/webapiapp"
# RUN dotnet build "webapiapp.csproj" -c Release -o /app/build


# # ================================
# # 2. PUBLISH STAGE
# # ================================
# FROM build AS publish

# RUN dotnet publish "webapiapp.csproj" \
#     -c Release \
#     -o /app/publish \
#     /p:UseAppHost=false


# # ================================
# # 3. RUNTIME STAGE
# # ================================
# FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final

# WORKDIR /app

# # Render uses the PORT environment variable.
# ENV ASPNETCORE_HTTP_PORTS=10000

# EXPOSE 10000

# # Copy published application
# COPY --from=publish /app/publish .

# # Start the application
# ENTRYPOINT ["dotnet", "webapiapp.dll"]