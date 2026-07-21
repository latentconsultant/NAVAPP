# Use the .NET SDK for building
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["navigateapp/navigateapp.csproj", "navigateapp/"]
RUN dotnet restore "navigateapp/navigateapp.csproj"
COPY . .
WORKDIR "/src/navigateapp"
RUN dotnet build "navigateapp.csproj" -c Release -o /app/build

# Publish the app
FROM build AS publish
RUN dotnet publish "navigateapp.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

# Create the data and uploads directories and ensure ownership by the built-in 'app' user
RUN mkdir -p /app/data /app/wwwroot/uploads && chown -R app:app /app/data /app/wwwroot/uploads

# Copy the published files and ensure they are owned by the 'app' user
COPY --from=publish --chown=app:app /app/publish .

# Switch to the non-root user
USER app

ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

# Expose the port the app listens on
EXPOSE 8080

# Define volumes for persistent data
VOLUME ["/app/data", "/app/wwwroot/uploads"]

ENTRYPOINT ["dotnet", "navigateapp.dll"]
