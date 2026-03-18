# ===== BUILD STAGE =====
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# copy toàn bộ project
COPY . .

# restore
RUN dotnet restore src/WebAPI/WebAPI.csproj

# publish
RUN dotnet publish src/WebAPI/WebAPI.csproj -c Release -o /app/publish

# ===== RUNTIME STAGE =====
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

COPY --from=build /app/publish .

# Railway sẽ cấp PORT env
ENV ASPNETCORE_URLS=http://+:$PORT

ENTRYPOINT ["dotnet", "WebAPI.dll"]
