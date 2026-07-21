# Publish script for Windows
# This script builds the application and prepares it for hosting

Write-Host "Restoring and Building..." -ForegroundColor Cyan
dotnet build -c Release

Write-Host "Publishing to ./publish folder..." -ForegroundColor Cyan
dotnet publish -c Release -o ./publish /p:UseAppHost=true

Write-Host "Publish complete! You can now copy the content of the './publish' folder to your host." -ForegroundColor Green
Write-Host "To run the app on the host, execute: ./navigateapp.exe" -ForegroundColor Yellow
